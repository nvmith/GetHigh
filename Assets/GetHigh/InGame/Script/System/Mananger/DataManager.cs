using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 매니저로 JsonClass의 데이터를 넘겨줌
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
        Debug.Log("활성화 체크");
    }

    private void OnDisable()
    {
        Debug.Log("비활성화 체크");
    }*/

    private void Start()
    {
        //Debug.Log("함수 실행 여부");


        GetData();
        SceneManager.sceneLoaded += SceneLoadFunc;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 씬이 메인 메뉴 갈 때 마다 실행되는 함수
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

    // 락 해제, 추후 로직 변동 필요할듯

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
