using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Player Status
    [SerializeField]
    private int playerHp = 0;
    public int PlayerHp => playerHp;

    [SerializeField]
    private int playerAttackPower;
    public int PlayerAttackPower => playerAttackPower;

    [SerializeField]
    private float playerBulletDistance;
    public float PlayerBulletDistance => playerBulletDistance;

    [SerializeField]
    private float playerAimAccuracy;
    public float PlayerAimAccuracy => playerAimAccuracy;

    [SerializeField]
    private float playerSpeed;
    public float PlayerSpeed => playerSpeed;

    [SerializeField]
    private float playerAttackDelay;
    public float PlayerAttackDelay => playerAttackDelay;

    public ECharacters selectCharacter;

    public bool playerCheck = false;

    [SerializeField]
    private bool[] diaryDataCheck = new bool[55];
    public bool[] DiaryDataCheck => diaryDataCheck;

    public DictionaryUI dictionaryUI = null;

    // PlayerInformation
    private int playTime = 0;
    public int PlayTime => playTime;

    private int killCount = 0;
    public int KillCount => killCount;

    private int diaryCount = 0;
    public int DiaryCount => diaryCount;

    private int characterCount = 0;
    public int CharacterCount => characterCount;

    private int deathCount = 0;
    public int DeathCount => deathCount;

    private int clearCount = 0;
    public int ClearCount => clearCount;

    // 해금여부
    public bool kuiperOn = false;
    public bool eunhaOn = false;

    // System
    public int languageIndex = 0; // 번역 인덱스 0:한국 1: 미국

    // ClearData
    public string clearPlayTime = null;
    public int clearDrugCount = 0;
    public int clearEnemyCount = 0;
    public int clearRoomCount = 0;
    public int clearMoney = 0;
    public bool clearCheck = false;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void Init()
    {
        if(Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);

    }
    public void GetPlayerValue(int hp, int power, float aim, float distance, float speed, float attackDelay)
    {
        playerHp = hp;
        playerAttackPower = power;
        playerAimAccuracy = aim;
        playerBulletDistance = distance;
        playerSpeed = speed;
        playerAttackDelay = attackDelay;
    }

    public void GetDiaryData(bool[] d)
    {
        for(int i=0; i<diaryDataCheck.Length; i++)
        {
            diaryDataCheck[i] = d[i];
        }
    }

    public void GetPlayerInformation(int time, int killCnt, int deathCnt, int clearCnt)
    {
        playTime = time;
        killCount = killCnt;
       
        diaryCount = 0;
        characterCount = 0;

        for(int i=0; i <diaryDataCheck.Length; i++) 
            if (diaryDataCheck[i]) diaryCount++;


        for (int i=21; i<24; i++)
            if (diaryDataCheck[i]) characterCount++;

        deathCount = deathCnt;
        clearCount = clearCnt;
    }

    public void UpdateDiaryDate(int index)
    {
        if (DiaryDataCheck[index-1]) return;

        diaryDataCheck[index-1] = true;
        dictionaryUI.UpdateContent(index);
    }

    public void UpdateKill()
    {
        killCount++;
        if(killCount >= 300) CheckKuiper();
    }

    public void UpdateTime(int t)
    {
        playTime += t;
    }

    public void UpdateDeathCount()
    {
        deathCount++;
    }
    public void UpdateClearCount()
    {
        clearCount++;
    }

    public void CheckEunha()
    {
        if (eunhaOn) return;

        for (int i = 6; i < 22; i++)
        {
            if (!diaryDataCheck[i]) return;
            if (i == 12) i = 16;
        }

        DataManager.Instacne.JsonClass.UpdateLock((int)ECharacters.Eunha);
        UpdateDiaryDate((int)EDiaryValue.Eunha);

        eunhaOn = true;
    }

    public void CheckKuiper()
    {
        if (kuiperOn) return;
        DataManager.Instacne.JsonClass.UpdateLock((int)ECharacters.Kuiper);
        UpdateDiaryDate((int)EDiaryValue.Kuiper);
        kuiperOn = true;
    }

    public void ResetClearData()
    {
        clearPlayTime = null;
        clearDrugCount = 0;
        clearEnemyCount = 0;
        clearRoomCount = 0;
        clearMoney = 0;
        clearCheck = false;
    }

    // 죽을때 or 게임 종료할 때
    public void SaveData()
    {
        DataManager.Instacne.JsonClass.SavePlayerData();
    }
}
