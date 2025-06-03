using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponValue
{
    Knife, Gun
}

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance => instance;

    public int playerWeaponType; // 캐릭터 기본 무기

    public Material material;

    [SerializeField]
    private GameObject[] prefabs;

    // 임시 플레이어 등록
    public Player player;

    [SerializeField]
    private Slider drugBar;
    public float DrugGauge => drugBar.value;

    [SerializeField]
    private bool isPause = false; //일단 현재는 인게임 상태이므로 일시정지 해제
    public bool IsPause => isPause;

    // Player Status
    [SerializeField]
    private int hp;
    public int Hp => hp;

    private int maxHp;
    public int MaxHp => maxHp;

    private int power;
    public int Power => power;

    private float aim;
    public float Aim => aim;

    private float bulletDistance;
    public float BulletDistance => bulletDistance;

    private float speed;
    public float Speed => speed;

    private float attackDelay;
    public float AttackDelay => attackDelay;

    public int bulletPower;

    // Player Data
    public int killCount = 0;

    public Weapons gunInven = null;
    public Weapons blueGunInven = null; // 파랑 마약 활성화 전용 인벤

    public Weapons pistolInven = null;

    public int curWeaponIndex = 4; // 현재 무기 인덱스
    public int lastWeaponIndex = 4; // 이전 무기 인덱스
    public int lastPistolIndex = -1; // 이전 권총 인덱스
    public int curPistolIndex = -1; // 현재 권총 인덱스

    // Items;
    public int money = 0;
    public int grenadeCount = 0;
    public int key = 0;
    public int[] magazines = { 0, 0 }; // 주무기, 보조무기 탄창

    public int[] bulletMagazine = { 30, 12, 10, 15 }; // 고정된 탄창
    public int[] curBullet = { 0, 0, 0, 0 }; // 현재 총에 있는 탄창

    private bool isDead;
    public bool IsDead => isDead;

    // Item
    public List<Item> tempItems = new List<Item>();
    public int tempItemIndex = -1; // 근처에 있는 아이템 인덱스

    // 수정해야함
    public Drug drugInven = null;
    public List<Drug> tempDrug = new List<Drug>();
    public int tempDrugIndex = -1;

    public int drugGauge;

    public GameObject grenadeObj;

    // Effect
    public GameObject basicWeaponPivot;
    public GameObject[] basicWeaponEffect;

    public bool bossRushCheck = false;

    // Diary
    public bool checkItem = false;
    public bool checkWeapon = false;
    public bool checkEunha = false;

    // Stage Door
    public GameObject stage1Door;

    [SerializeField]
    private LockAnimation[] lockAni;

    public TrailRenderer[] meleeRenderer;
    private void Awake()
    {
        Init();
        GeneratePlayer();
    }

    void Start()
    {
        //GameManager.Instance.UpdateDiaryDate(playerWeaponType + (int)EDiaryValue.Haeseong);
    }

    // Update is called once per frame
    private void Update()
    {
        if (tempItems.Count == 0) return;

        if (tempItems.Count != 0) // 근처에 아이템이 존재한다면
        {
            float minDistance = 999f;
            tempItemIndex = -1;

            for (int i = tempItems.Count - 1; i >= 0; i--)
            {
                //Debug.Log(i + "번째 아이템 실행 문제 체크");

                tempItems[i].SilhouetteCheck(false);
                if (minDistance >= tempItems[i].distance)
                {
                    minDistance = tempItems[i].distance;
                    tempItemIndex = i;
                   // Debug.Log("아이템 인덱스 :" + tempItemIndex);
                }
                //Debug.Log(i + "번째 아이템 실행 통과");
            }
        }

        tempItems[tempItemIndex].SilhouetteCheck(true);
    }

    private void Init()
    {
        if (Instance == null)
        {
            instance = this;
        }

        maxHp = GameManager.Instance.PlayerHp;
        hp = maxHp;
        power = GameManager.Instance.PlayerAttackPower;
        aim = GameManager.Instance.PlayerAimAccuracy;
        bulletDistance = GameManager.Instance.PlayerBulletDistance;
        speed = GameManager.Instance.PlayerSpeed;
        attackDelay = GameManager.Instance.PlayerAttackDelay;
    }

    private void GeneratePlayer()
    {
        int index = 0;
        GameObject playerObj;

        switch (GameManager.Instance.selectCharacter)
        {
            case ECharacters.Haeseong:
                index = (int)ECharacters.Haeseong;
                playerWeaponType = index;
                break;
            case ECharacters.Eunha:
                index = (int)ECharacters.Eunha;
                playerWeaponType = index;
                break;
            case ECharacters.Kuiper:
                index = (int)ECharacters.Kuiper;
                playerWeaponType = index;
                break;
                /*
                case ECharacters.c2:

                    break;
                case ECharacters.c3:

                    break;*/
        }
        
        playerObj = Instantiate(prefabs[index], transform.position, transform.rotation);
        player = playerObj.GetComponent<Player>();
    }

    public void Pause(bool check)
    {
        isPause = check;

        if (isPause) Time.timeScale = 0f;
        else Time.timeScale = DrugManager.Instance.timeValue;
    }

    public void ItemUse()
    {/*
        if (tempItems.Count == 0) return;

        if (tempItems.Count != 0) // 근처에 아이템이 존재한다면
        {
            float minDistance = 999f;
            tempItemIndex = -1;

            for (int i = tempItems.Count - 1; i >= 0; i--)
            {
                Debug.Log(i + "번째 아이템 실행 문제 체크");
                if (tempItems[i] == null) continue;

                if (minDistance > tempItems[i].distance)
                {
                    minDistance = tempItems[i].distance;
                    tempItemIndex = i;
                    Debug.Log("아이템 인덱스 :" + tempItemIndex);
                }
                Debug.Log(i + "번째 아이템 실행 통과");
            }
        }*/
        if (tempItems.Count == 0) return;
        tempItems[tempItemIndex].GetItem();
    }

    public void DrugUse()
    {
        if (tempDrug.Count != 0) // 근처에 마약이 있다면
        {
            float minDistance = 999f;
            tempDrugIndex = -1;

            for (int i = tempDrug.Count - 1; i >= 0; i--)
            {
                Debug.Log(i + "번째 마약 실행 문제 체크");
                if (tempDrug[i] == null) continue;

                if (minDistance >= tempDrug[i].distance)
                {
                    minDistance = tempDrug[i].distance;
                    tempDrugIndex = i;
                }
                Debug.Log(i + "번째 마약 실행 통과");
            }
        }

        tempDrug[tempDrugIndex].UseItem();
    }

    // 구매가 안될 때 상인이 못산다고 말해주는 함수
    public void NonBuy()
    {

    }

    public void Buy(int price)
    {
        UpdateMoney(-price);
        SoundManager.Instance.PlaySFX(SFX.UseMoney);
    }

    public void UpdateDrug(int value)
    {
        // UI 매니저에서 수정
        if (drugGauge + value > 100) drugGauge = 100;
        else drugGauge += value;
        
        drugBar.value = drugGauge;

        DrugManager.Instance.LockCheck(DrugGauge);
    }

    public void DecreaseDrug()
    {
        int randomGauge = Random.Range(0, 16);

        if (drugGauge - randomGauge <= 0)
        {
            drugGauge = 0;
        }

        else if (drugGauge >= 25 && drugGauge - randomGauge < 25)
        {
            drugGauge = 25;
        }

        else if (drugGauge >= 50 && drugGauge - randomGauge < 50)
        {
            drugGauge = 50;
        }

        else if (drugGauge >= 75 && drugGauge - randomGauge < 75)
        {
            drugGauge = 75;
        }

        else
        {
            drugGauge -= randomGauge;
        }

        drugBar.value = drugGauge;
        Buy(10);
    }


    // 수정 로직
    // 총 획득 로직 후 교체 -> 이름 수정 해야할듯
    public void UpdateWeapon(EWeapons value, Weapons weapon)
    {
        int idx = -1; // 무기 종류 (0 : 주무기, 1 : 보조무기)
        lastWeaponIndex = curWeaponIndex; // 장작 중이던 무기 인덱스를 전에 장작한 인덱스로 전환
        lastPistolIndex = curPistolIndex; // 각 권총 종류별 인덱스 대입

        if (value == EWeapons.Revolver) // 권총을 획득한다면
        {
            idx = 1;
            pistolInven = weapon; // 획득한 무기를 권총에 저장
            curWeaponIndex = 3;
            curPistolIndex = pistolInven.index;
        }
        else // 주무기를 획득한다면
        {
            idx = 0;
            gunInven = weapon;
            curWeaponIndex = (int)value;
        }
        GetBullet(value, weapon.bulletCount); // 교체한 무기의 총알 등록
        player.WeaponSwap(idx);
    }


    public void UpdateMoney(int value)
    {
        money += value;
        UIManager.Instance.inGameUI.MoneyUpdate(money);
    } 

    public void UpdateKey(int value)
    {
        key += value;
        UIManager.Instance.inGameUI.KeyUpdate(key);
        // 이것도 UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateGrenade(int value)
    {
        grenadeCount += value;
        UIManager.Instance.inGameUI.GrenadeUpdate(grenadeCount);
        //numBombUI.text = "Bomb: " + numBomb;
    }

    public void UseGrenade()
    {
        // UI연동 필요
        if (grenadeCount > 0)
        {
            PoolManager.Instance.GetGrenadeObject();
            //Instantiate(grenadeObj, player.transform.position, player.transform.rotation);
            UpdateGrenade(-1);
            UIManager.Instance.inGameUI.GrenadeUpdate(grenadeCount);
        }
    }
    
    public void GetMagazine(int value)
    {
        magazines[value]++;
        UIManager.Instance.inGameUI.MagazineUpdate(value, magazines[value]);
    }

    public void GetBullet(EWeapons value, int count) // UI 탄알 갯수 수정 로직 포함해야함
    {
        curBullet[(int)value] = count;
    }

  
    public void PutBullet(EWeapons value)
    {
        if (value == EWeapons.Revolver) // 권총일 경우
        {
            PutBulletLogic(1);
            if (curWeaponIndex < 3) PutBulletLogic(0);
        }
        else
        {
            PutBulletLogic(0);
            if (curWeaponIndex == 3) PutBulletLogic(1);
        }
    }

    public void PutBulletLogic(int value) // 0: 주무기 1: 보조무기
    {
        switch(value)
        {
            case 0:
                //Debug.Log("현재 주무기 총알 개수 : " + curBullet[(int)gunInven.eWeapons]);
                gunInven.gameObject.SetActive(true); // 주무기 오브젝트 활성화
                gunInven.bulletCount = curBullet[(int)gunInven.eWeapons]; // 현재 주무기에 해당하는 총알을 저장
                break;
            case 1:
                //Debug.Log("현재 권총 총알 개수 : " + curBullet[3]);
                pistolInven.gameObject.SetActive(true); // 권총 오브젝트 활성화
                pistolInven.bulletCount = curBullet[3]; // 현재 권총개수를 권총에 저장
                break;
        }
    }


    public bool CheckReload(int a)
    {
        if (magazines[a] > 0)
        {
            Debug.Log("탄창은 있음");
            if(a==0) return bulletMagazine[(int)gunInven.eWeapons] > curBullet[(int)gunInven.eWeapons];
            else return bulletMagazine[(int)pistolInven.eWeapons] > curBullet[(int)pistolInven.eWeapons];
        }
        else return false;
    }

    public void ReloadBullet(int a)
    {
        if (a == 0)
        {
            GetBullet(gunInven.eWeapons, bulletMagazine[(int)gunInven.eWeapons]);
        }
        else GetBullet(EWeapons.Revolver, bulletMagazine[3]);

        magazines[a]--;
        UIManager.Instance.inGameUI.MagazineUpdate(a, magazines[a]);
    }

    public void BasicWeaponCheck(bool check)
    {
        basicWeaponEffect[playerWeaponType].SetActive(check);
    }

    public void MaxHPUpdate()
    {
        // 만약 락 해제 기능 나오면 로직 수정해야함
        maxHp += 2;
        UIManager.Instance.hpInit();
    }
    public void HealHp(int value)
    {
        //Debug.Log("회복함 : " + value);
        if (hp + value > maxHp)
        {
            hp = maxHp;
        }
        else hp += value;

        UIManager.Instance.hpUpdate();
    }

    public int PlayerBuffPower() // 현재 플레이어 적용된 버프 파워 반환
    {
        // 기본 공격력 + 무기 공격력 + 버프별 적용된 공격력 
        int curPower = DrugManager.Instance.power + power + player.weaponPower;

        if(DrugManager.Instance.red2)
        {
            curPower = curPower + curPower * DrugManager.Instance.powerUpValue / 100;
        }
        if(DrugManager.Instance.isAnger)
        {
            curPower = curPower + (int)(curPower * DrugManager.Instance.angerPower * 0.4f);
        }

        return curPower - power;
    }

    /*public void CheckGirl() // 윤하해금여부 체크
    {
        if (checkEunha) return;

        for (int i = (int)EDiaryValue.Band - 1; i < (int)EDiaryValue.SubMagazine; i++)
        {
            if (!GameManager.Instance.DiaryDataCheck[i]) return;
        }

        for (int i = (int)EDiaryValue.Desert_Eagle - 1; i < (int)EDiaryValue.AWP; i++)
        {
            if (!GameManager.Instance.DiaryDataCheck[i]) return;
        }

        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Eunha); // 윤하 해금
        DataManager.Instacne.UpdateLock((int)ECharacters.Eunha);
    }*/

    public void UpdateKillCount()
    {
        killCount++;
        GameManager.Instance.UpdateKill();
    }

    public void Hit(int value)
    {
        //Debug.Log("맞았음 : " + value);
        if (hp - value <= 0)
        {
            hp = 0;
            GameOver();
        }
        else hp -= value;
    }

    public void TrailCheck(bool check)
    {
        meleeRenderer[playerWeaponType].enabled = check;
    }
    public void LockAnimationPlay(int index)
    {
        //int index = (drugGauge / 25) - 1;

        lockAni[index].gameObject.SetActive(true);
        lockAni[index].AniPlay(index);
    }

    // 추후 사망로직 추가하기
    private void GameOver()
    {
        player.GetComponent<BoxCollider2D>().enabled = false;
        CameraController.Instance.transform.position = new Vector3(
            player.transform.position.x, player.transform.position.y, -10);
        Pause(true);
        player.dieAnim.SetTrigger("Die");
        player.DisableRenderer();


        GameManager.Instance.UpdateDeathCount();

        StartCoroutine(IOver());
    }

    private IEnumerator IOver()
    {
        yield return new WaitForSecondsRealtime(1f);
        UIManager.Instance.GameOver();
    }
}
