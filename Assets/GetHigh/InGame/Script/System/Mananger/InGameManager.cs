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

    public int playerWeaponType; // ĳ���� �⺻ ����

    public Material material;

    [SerializeField]
    private GameObject[] prefabs;

    // �ӽ� �÷��̾� ���
    public Player player;

    [SerializeField]
    private Slider drugBar;
    public float DrugGauge => drugBar.value;

    [SerializeField]
    private bool isPause = false; //�ϴ� ����� �ΰ��� �����̹Ƿ� �Ͻ����� ����
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
    public Weapons blueGunInven = null; // �Ķ� ���� Ȱ��ȭ ���� �κ�

    public Weapons pistolInven = null;

    public int curWeaponIndex = 4; // ���� ���� �ε���
    public int lastWeaponIndex = 4; // ���� ���� �ε���
    public int lastPistolIndex = -1; // ���� ���� �ε���
    public int curPistolIndex = -1; // ���� ���� �ε���

    // Items;
    public int money = 0;
    public int grenadeCount = 0;
    public int key = 0;
    public int[] magazines = { 0, 0 }; // �ֹ���, �������� źâ

    public int[] bulletMagazine = { 30, 12, 10, 15 }; // ������ źâ
    public int[] curBullet = { 0, 0, 0, 0 }; // ���� �ѿ� �ִ� źâ

    private bool isDead;
    public bool IsDead => isDead;

    // Item
    public List<Item> tempItems = new List<Item>();
    public int tempItemIndex = -1; // ��ó�� �ִ� ������ �ε���

    // �����ؾ���
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

        if (tempItems.Count != 0) // ��ó�� �������� �����Ѵٸ�
        {
            float minDistance = 999f;
            tempItemIndex = -1;

            for (int i = tempItems.Count - 1; i >= 0; i--)
            {
                //Debug.Log(i + "��° ������ ���� ���� üũ");

                tempItems[i].SilhouetteCheck(false);
                if (minDistance >= tempItems[i].distance)
                {
                    minDistance = tempItems[i].distance;
                    tempItemIndex = i;
                   // Debug.Log("������ �ε��� :" + tempItemIndex);
                }
                //Debug.Log(i + "��° ������ ���� ���");
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

        if (tempItems.Count != 0) // ��ó�� �������� �����Ѵٸ�
        {
            float minDistance = 999f;
            tempItemIndex = -1;

            for (int i = tempItems.Count - 1; i >= 0; i--)
            {
                Debug.Log(i + "��° ������ ���� ���� üũ");
                if (tempItems[i] == null) continue;

                if (minDistance > tempItems[i].distance)
                {
                    minDistance = tempItems[i].distance;
                    tempItemIndex = i;
                    Debug.Log("������ �ε��� :" + tempItemIndex);
                }
                Debug.Log(i + "��° ������ ���� ���");
            }
        }*/
        if (tempItems.Count == 0) return;
        tempItems[tempItemIndex].GetItem();
    }

    public void DrugUse()
    {
        if (tempDrug.Count != 0) // ��ó�� ������ �ִٸ�
        {
            float minDistance = 999f;
            tempDrugIndex = -1;

            for (int i = tempDrug.Count - 1; i >= 0; i--)
            {
                Debug.Log(i + "��° ���� ���� ���� üũ");
                if (tempDrug[i] == null) continue;

                if (minDistance >= tempDrug[i].distance)
                {
                    minDistance = tempDrug[i].distance;
                    tempDrugIndex = i;
                }
                Debug.Log(i + "��° ���� ���� ���");
            }
        }

        tempDrug[tempDrugIndex].UseItem();
    }

    // ���Ű� �ȵ� �� ������ ����ٰ� �����ִ� �Լ�
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
        // UI �Ŵ������� ����
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


    // ���� ����
    // �� ȹ�� ���� �� ��ü -> �̸� ���� �ؾ��ҵ�
    public void UpdateWeapon(EWeapons value, Weapons weapon)
    {
        int idx = -1; // ���� ���� (0 : �ֹ���, 1 : ��������)
        lastWeaponIndex = curWeaponIndex; // ���� ���̴� ���� �ε����� ���� ������ �ε����� ��ȯ
        lastPistolIndex = curPistolIndex; // �� ���� ������ �ε��� ����

        if (value == EWeapons.Revolver) // ������ ȹ���Ѵٸ�
        {
            idx = 1;
            pistolInven = weapon; // ȹ���� ���⸦ ���ѿ� ����
            curWeaponIndex = 3;
            curPistolIndex = pistolInven.index;
        }
        else // �ֹ��⸦ ȹ���Ѵٸ�
        {
            idx = 0;
            gunInven = weapon;
            curWeaponIndex = (int)value;
        }
        GetBullet(value, weapon.bulletCount); // ��ü�� ������ �Ѿ� ���
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
        // �̰͵� UI
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
        // UI���� �ʿ�
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

    public void GetBullet(EWeapons value, int count) // UI ź�� ���� ���� ���� �����ؾ���
    {
        curBullet[(int)value] = count;
    }

  
    public void PutBullet(EWeapons value)
    {
        if (value == EWeapons.Revolver) // ������ ���
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

    public void PutBulletLogic(int value) // 0: �ֹ��� 1: ��������
    {
        switch(value)
        {
            case 0:
                //Debug.Log("���� �ֹ��� �Ѿ� ���� : " + curBullet[(int)gunInven.eWeapons]);
                gunInven.gameObject.SetActive(true); // �ֹ��� ������Ʈ Ȱ��ȭ
                gunInven.bulletCount = curBullet[(int)gunInven.eWeapons]; // ���� �ֹ��⿡ �ش��ϴ� �Ѿ��� ����
                break;
            case 1:
                //Debug.Log("���� ���� �Ѿ� ���� : " + curBullet[3]);
                pistolInven.gameObject.SetActive(true); // ���� ������Ʈ Ȱ��ȭ
                pistolInven.bulletCount = curBullet[3]; // ���� ���Ѱ����� ���ѿ� ����
                break;
        }
    }


    public bool CheckReload(int a)
    {
        if (magazines[a] > 0)
        {
            Debug.Log("źâ�� ����");
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
        // ���� �� ���� ��� ������ ���� �����ؾ���
        maxHp += 2;
        UIManager.Instance.hpInit();
    }
    public void HealHp(int value)
    {
        //Debug.Log("ȸ���� : " + value);
        if (hp + value > maxHp)
        {
            hp = maxHp;
        }
        else hp += value;

        UIManager.Instance.hpUpdate();
    }

    public int PlayerBuffPower() // ���� �÷��̾� ����� ���� �Ŀ� ��ȯ
    {
        // �⺻ ���ݷ� + ���� ���ݷ� + ������ ����� ���ݷ� 
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

    /*public void CheckGirl() // �����رݿ��� üũ
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

        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Eunha); // ���� �ر�
        DataManager.Instacne.UpdateLock((int)ECharacters.Eunha);
    }*/

    public void UpdateKillCount()
    {
        killCount++;
        GameManager.Instance.UpdateKill();
    }

    public void Hit(int value)
    {
        //Debug.Log("�¾��� : " + value);
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

    // ���� ������� �߰��ϱ�
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
