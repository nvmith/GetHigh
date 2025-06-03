/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
*/

using System.IO;
using UnityEngine;

//public enum 

public class JsonClass
{
    // JsonClasses
    private PlayerData _playerData;
    public PlayerData _PlayerData => _playerData;

    private DiaryData _diaryData;
    public DiaryData _DiaryData => _diaryData;

    // DefaultPath
    private string defaultPath;
    private const string pDJsonName = "PlayerData.json";
    private const string dDJsonName = "DiaryData.json";

    private string pdPath;
    private string ddPath;

    private FileInfo pdFileInfo;
    private FileInfo ddFileInfo;

    // Init Data Settings
    public JsonClass()
    {
        _playerData = new PlayerData();
        _diaryData = new DiaryData();
    }

    // Check Data
    public void StartPlayerData()
    {
        defaultPath = Application.persistentDataPath + '/';
        pdPath = defaultPath + pDJsonName;
        ddPath = defaultPath + dDJsonName;

        pdFileInfo = new FileInfo(pdPath);
        ddFileInfo = new FileInfo(ddPath);

        string readJsonData = null;

        if (pdFileInfo.Exists)
        {
            readJsonData = File.ReadAllText(pdPath);
            _playerData = JsonUtility.FromJson<PlayerData>(readJsonData);
        }

        if(ddFileInfo.Exists)
        {
            readJsonData = File.ReadAllText(ddPath);
            _diaryData = JsonUtility.FromJson<DiaryData>(readJsonData);
        }
    }

    // Save PlayerData 저장(죽을때, 게임을 종료할 때 해당 조건을 게임매니저에서 실행)
    public void SavePlayerData()
    {
        UpdateDiaryCheck();
        UpdateHistory();

        File.WriteAllText(pdPath, JsonUtility.ToJson(_playerData));
        File.WriteAllText(ddPath, JsonUtility.ToJson(_diaryData));
    }

    public void UpdateLock(int index)
    {
        _playerData.playerLock[index] = true;
    }

    public void UpdatePointer(int index)
    {
        for(int i=0; i < _playerData.mousePointer.Length; i++)
        {
            _playerData.mousePointer[i] = false;
        }

        _playerData.mousePointer[index] = true;
    }

    public void UpdateDiaryCheck()
    {
        for(int i=0;i<_diaryData.checkDiary.Length; i++)
        {
            _diaryData.checkDiary[i] = GameManager.Instance.DiaryDataCheck[i];
        }
    }

    public void UpdateHistory()
    {
        _playerData.playTime = GameManager.Instance.PlayTime;
        _playerData.killEnemy = GameManager.Instance.KillCount;
        _playerData.diaryCount = GameManager.Instance.DiaryCount;
        _playerData.deathCount = GameManager.Instance.DeathCount;
        _playerData.clearCount = GameManager.Instance.ClearCount;
    }
}

public class PlayerData
{
    public bool[] mousePointer = { true, false, false, false, false }; 
    public int playTime = 0;
    public int killEnemy = 0;
    public int diaryCount = 0;
    public bool[] playerLock = { true, false, false}; // true : 해제, false : 잠금
    public int deathCount = 0;
    public int clearCount = 0;
}

public class DiaryData
{
    public bool[] checkDiary =
    {
        false, false, false, false, false, false, // 마약
        false, false, false, false, false, false, false, // 아이템
        false, false, false, // 근접 무기
        false, false, false, false, false, // 보조무기, 주무기
        true, false, false, // 캐릭터
        false, // 적
        false, false, false, // 보스
        false, false, // Npc
        false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, // 버프
        false, false, false, false, false, false, false, false, false, false // 디버프
    };
    /*public bool[] checkDrug = { false, false, false, false, false, false };
    public bool[] checkItem = { false, false, false, false, false, false, false };
    public bool[] checkMelee = { false, false, false };
    public bool[] checkWeapon = { false, false, false, false, false };
    public bool[] checkCharacter = { false, false, false };
    public bool[] checkEnemy = { false, false, false };
    public bool[] checkBoos = { false, false, false };
    public bool[] checkNpc = { false, false, false };
    public bool[] checkBuff = 
        { false, false, false, 
        false, false, false , 
        false, false, false,
        false, false, false, 
        false, false, false};
    public bool[] checkNerf =
        {false, false, false,
        false, false, false,
        false, false, false,
        false};*/
}