using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� �Ŵ����� JsonClass�� �����͸� �Ѱ���
public class DataManager : MonoBehaviour
{
    private static DataManager instacne;
    public static DataManager Instacne => instacne;

    // JsonData
    private JsonClass jsonClass;
    public JsonClass JsonClass => jsonClass;

    // DefaultData
    private DefaultData defaultData;
    public DefaultData DefaultData => defaultData;

    // DateValue
    /// Cursor
    private int mouseIndex = -1;
    public int MouseIndex => mouseIndex;

    public Sprite[] pointerSprites;
    public Texture2D[] pointerTextures;
    Vector2 hotSpot;

    // Generacte Value
    public DataManager()
    {
        if(jsonClass == null) jsonClass = new JsonClass();
        if(defaultData == null) defaultData = new DefaultData();
    }

    private void Awake()
    {
        Init();

        jsonClass.StartPlayerData();

        for (int i = 0; i < jsonClass._PlayerData.mousePointer.Length; i++)
        {
            if (jsonClass._PlayerData.mousePointer[i] == true)
            {
                UpdatePointer(i);
                break;
            }
        }
    }

    private void Init()
    {
        if (Instacne == null)
        {
            instacne = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    /*private void OnEnable()
    {
        Debug.Log("Ȱ��ȭ üũ");
    }

    private void OnDisable()
    {
        Debug.Log("��Ȱ��ȭ üũ");
    }*/

    private void Start()
    {
        //Debug.Log("�Լ� ���� ����");


        GetData();
        SceneManager.sceneLoaded += SceneLoadFunc;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� ���� �޴� �� �� ���� ����Ǵ� �Լ�
    private void SceneLoadFunc(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) GetData();
    }

    private void GetData()
    {
        GameManager.Instance.GetDiaryData(jsonClass._DiaryData.checkDiary);
        GameManager.Instance.GetPlayerInformation(
            jsonClass._PlayerData.playTime, jsonClass._PlayerData.killEnemy,
            jsonClass._PlayerData.deathCount, jsonClass._PlayerData.clearCount);
    }

    // �� ����, ���� ���� ���� �ʿ��ҵ�

    public void UpdateLock(int index)
    {
        if (jsonClass._PlayerData.playerLock[index]) return;

        jsonClass.UpdateLock(index);
    }

    public void UpdatePointer(int index)
    {
        mouseIndex = index;
        jsonClass.UpdatePointer(mouseIndex);

        hotSpot.x = pointerTextures[mouseIndex].width / 2;

        if (index == 3) hotSpot.y = 0;
        else if (index == 4) { hotSpot.x = 0; hotSpot.y = 0; }
        else hotSpot.y = pointerTextures[mouseIndex].height / 2;

        Cursor.SetCursor(pointerTextures[mouseIndex], hotSpot, CursorMode.Auto);
    }

}
