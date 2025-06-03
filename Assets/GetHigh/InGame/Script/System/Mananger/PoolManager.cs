using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField]
    private GameObject[] drug;
    [SerializeField]
    private Transform[] drugPos;


    [SerializeField]
    private GameObject[] playerBullet;
    [SerializeField]
    private GameObject agentBullet;

    [SerializeField]
    private Transform[] playerBulletPos;
    [SerializeField]
    private Transform agentBulletPos;

    [SerializeField]
    private GameObject[] activeItem;
    [SerializeField]
    private Transform[] activeItemPos;

    [SerializeField]
    private GameObject money;
    [SerializeField]
    private Transform moneyPos;

    [SerializeField]
    private GameObject grenadeObjects;
    [SerializeField]
    private Transform grenadeObjectsPos;

    [SerializeField]
    private GameObject[] magazine;
    [SerializeField]
    private Transform[] magazinePos;

    [SerializeField]
    private GameObject bossGrenade;
    [SerializeField]
    private Transform bossGrenadePos;

    [SerializeField]
    private GameObject knifeBullet;
    [SerializeField]
    private Transform knifeBulletPos;

    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private Transform fireEffectPos;

    [SerializeField]
    private int initCount;

    public Queue<Drug>[] poolingDrug = { new Queue<Drug>(), new Queue<Drug>(), new Queue<Drug>(), new Queue<Drug>(), new Queue<Drug>() };
    public Queue<Bullet>[] poolingPlayerBullet = { new Queue<Bullet>(), new Queue<Bullet>(), new Queue<Bullet>(), new Queue<Bullet>() };
    public Queue<Bullet> poolingEnemyBullet = new Queue<Bullet>();

    // 즉시 엑티브 전용 아이템
    public Queue<Item>[] poolingActiveItem = { new Queue<Item>(), new Queue<Item>(), new Queue<Item>(),
            new Queue<Item>()};
    // 붕대, 열쇠, 방탄, 수류탄 존재

    public Queue<Item> poolingMoney = new Queue<Item>();

    public Queue<GrenadeObject> poolingGrenadeObject = new Queue<GrenadeObject>(); // 날라가는 수류탄
    public Queue<Magazine>[] poolingMagazine = { new Queue<Magazine>(), new Queue<Magazine>() };

    public Queue<BossGrenade> poolingBossGrenade = new Queue<BossGrenade>();
    // 총은 각각 자신의 총알을 가지므로 사용하기 안좋음

    public Queue<KnifeBullet> poolingKnifeBulletBoss = new Queue<KnifeBullet>();

    public Queue<ParticleSystem> poolingFireEffect = new Queue<ParticleSystem>();

    private void Awake()
    {
        Instance = this;


        Initialize(initCount);
    }

    private void Initialize(int initCount)
    {
        int i = 0;

        for (i = 0; i < initCount; i++)
        {
            CreateDrug(EDrugColor.red);
            CreateDrug(EDrugColor.orange);
            CreateDrug(EDrugColor.yellow);
            CreateDrug(EDrugColor.green);
            CreateDrug(EDrugColor.blue);

            CreateNewBullet(EUsers.Player, EBullets.Rifle);
            CreateNewBullet(EUsers.Player, EBullets.Shotgun);
            CreateNewBullet(EUsers.Player, EBullets.Sniper);
            CreateNewBullet(EUsers.Player, EBullets.Revolver);
            CreateNewBullet(EUsers.Enemy, EBullets.Revolver);

            CreateActiveItem(EActiveItems.Band);
            CreateActiveItem(EActiveItems.Key);
            CreateActiveItem(EActiveItems.Bulletproof);
            CreateActiveItem(EActiveItems.Grenade);

            CreateMoney();

            CreateGrenadeObject();

            CreateMagzine(0);
            CreateMagzine(1);

            CreateBossGrenade();
            CreateKnifeBullet();

            CreateFireEffect();
        }

        /*
        for (i = 0; i < initCount; i++)
        {
            CreateNewBullet(EUsers.Player, EBullets.Rifle);
            CreateNewBullet(EUsers.Player, EBullets.Shotgun);
            CreateNewBullet(EUsers.Player, EBullets.Sniper);
            CreateNewBullet(EUsers.Player, EBullets.Revolver);
            CreateNewBullet(EUsers.Enemy, EBullets.Revolver);
        }

        for (i = 0; i < initCount; i++)
        {
            CreateActiveItem(EActiveItems.Band);
            CreateActiveItem(EActiveItems.Key);
            CreateActiveItem(EActiveItems.Bulletproof);
            CreateActiveItem(EActiveItems.Grenade);
            CreateActiveItem(EActiveItems.Money);
        }*/

        // 예시
        /*
        for (i = 0; i < initCount; i++)
        {
            GetActiveItem(EActiveItems.Band);
            GetActiveItem(EActiveItems.Key);
            GetActiveItem(EActiveItems.Bulletproof);
            GetActiveItem(EActiveItems.Grenade);
            GetActiveItem(EActiveItems.Money);

            GetDrug(EDrugColor.red);
            GetDrug(EDrugColor.orange);
            GetDrug(EDrugColor.yellow);
            GetDrug(EDrugColor.green);
            GetDrug(EDrugColor.blue);
        }*/
    }

    private void CreateDrug(EDrugColor value)
    {
        int a = (int)value;

        Drug d = Instantiate(drug[a]).GetComponent<Drug>();
        d.transform.SetParent(drugPos[a]);
        d.name = value.ToString() + "Drug";
        d.gameObject.SetActive(false);
        poolingDrug[a].Enqueue(d);
    }

    public Drug GetDrug(EDrugColor value)
    {
        int index = (int)value;
        if (poolingDrug[index].Count <= 0)
        {
            CreateDrug(value);
        }
        Drug d = poolingDrug[index].Dequeue();
        d.gameObject.SetActive(true);
        return d;
    }
    public void ReturnDrug(Drug d, EDrugColor value)
    {
        d.gameObject.SetActive(false);

        poolingDrug[(int)value].Enqueue(d);
    }


    private void CreateNewBullet(EUsers eUser, EBullets eBullet)
    {
        if (eUser == EUsers.Enemy)
        {
            Bullet newObj = Instantiate(agentBullet).GetComponent<Bullet>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(agentBulletPos);
            newObj.name = eBullet.ToString();
            poolingEnemyBullet.Enqueue(newObj);
        }
        else
        {
            Bullet newObj = Instantiate(playerBullet[(int)eBullet]).GetComponent<Bullet>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(playerBulletPos[(int)eBullet]);
            newObj.name = eBullet.ToString();
            poolingPlayerBullet[(int)eBullet].Enqueue(newObj);
        }
    }

    public Bullet GetBullet(EUsers eUser, EBullets eBullet, Quaternion q)
    {
        if (eUser == EUsers.Player)
        {
            //Debug.Log("poolingPlayerBullet : " + )

            if (poolingPlayerBullet[(int)eBullet].Count <= 0)
            {
                CreateNewBullet(eUser, eBullet);
            }
            Bullet obj = poolingPlayerBullet[(int)eBullet].Dequeue();
            obj.gameObject.transform.rotation = q;

            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            if (poolingEnemyBullet.Count <= 0)
            {
                CreateNewBullet(eUser, eBullet);
            }

            Bullet obj = poolingEnemyBullet.Dequeue();
            obj.gameObject.transform.rotation = q; // 추가함
            obj.gameObject.SetActive(true);
            return obj;
        }

    }

    public void ReturnBullet(Bullet obj, EUsers eUser, EBullets eBullet)
    {
        obj.gameObject.SetActive(false);

        if (eUser == EUsers.Enemy)
        {
            poolingEnemyBullet.Enqueue(obj);
        }
        else
        {
            //테스트용 코드
            //obj.transform.SetParent(playerBulletPos[(int)eBullet]);
            poolingPlayerBullet[(int)eBullet].Enqueue(obj);
        }
        //테스트 2
        //obj.gameObject.SetActive(false);
    }

    private void CreateActiveItem(EActiveItems value)
    {
        int a = (int)value;

        Item i = Instantiate(activeItem[a]).GetComponent<Item>();
        i.transform.SetParent(activeItemPos[a]);
        i.name = value.ToString() + "Item";
        i.gameObject.SetActive(false);
        poolingActiveItem[a].Enqueue(i);
    }

    public Item GetActiveItem(EActiveItems value)
    {
        int index = (int)value;
        if (poolingActiveItem[index].Count <= 0)
        {
            CreateActiveItem(value);
        }

        Item i = poolingActiveItem[index].Dequeue();
        i.gameObject.SetActive(true);
        return i;
    }
    public void ReturnActiveItem(Item i, EActiveItems value)
    {
        i.gameObject.SetActive(false);
        poolingActiveItem[(int)value].Enqueue(i);
    }

    private void CreateMoney()
    {
        Item i = Instantiate(money).GetComponent<Item>();
        i.transform.SetParent(moneyPos);
        i.gameObject.SetActive(false);
        poolingMoney.Enqueue(i);
    }

    public Item GetMoney()
    {
        if (poolingMoney.Count <= 0)
        {
            CreateMoney();
        }

        Item i = poolingMoney.Dequeue();
        i.gameObject.SetActive(true);
        return i;
    }
    public void ReturnMoney(Item i)
    {
        i.gameObject.SetActive(false);
        poolingMoney.Enqueue(i);
    }

    private void CreateGrenadeObject()
    {
        GrenadeObject g = Instantiate(grenadeObjects).GetComponent<GrenadeObject>();
        g.transform.SetParent(grenadeObjectsPos);
        g.name += "Item";
        g.gameObject.SetActive(false);
        poolingGrenadeObject.Enqueue(g);
    }

    public GrenadeObject GetGrenadeObject()
    {
        if (poolingGrenadeObject.Count <= 0)
        {
            CreateGrenadeObject();
        }

        GrenadeObject g = poolingGrenadeObject.Dequeue();
        g.gameObject.SetActive(true);
        return g;
    }
    public void ReturnGrenadeObject(GrenadeObject g)
    {
        g.gameObject.SetActive(false);
        poolingGrenadeObject.Enqueue(g);
    }

    private void CreateMagzine(int value)
    {
        Magazine m = Instantiate(magazine[value]).GetComponent<Magazine>();

        m.transform.SetParent(magazinePos[value]);
        m.name = (value == 1 ? "Sub" : "Main") + value.ToString();
        m.gameObject.SetActive(false);
        poolingMagazine[value].Enqueue(m);
    }

    public Magazine GetMagazine(int value)
    {
        if (poolingMagazine[value].Count <= 0)
        {
            CreateMagzine(value);
        }
        Magazine m = poolingMagazine[value].Dequeue();
        m.gameObject.SetActive(true);
        return m;
    }
    public void ReturnMagzine(Magazine m, int value)
    {
        m.gameObject.SetActive(false);
        poolingMagazine[value].Enqueue(m);
    }

    private void CreateBossGrenade()
    {
        BossGrenade g = Instantiate(bossGrenade).GetComponent<BossGrenade>();
        g.transform.SetParent(bossGrenadePos);
        g.name += "Item";
        g.gameObject.SetActive(false);
        poolingBossGrenade.Enqueue(g);
    }

    public BossGrenade GetBossGrenade()
    {
        if (poolingBossGrenade.Count <= 0)
        {
            CreateBossGrenade();
        }

        BossGrenade g = poolingBossGrenade.Dequeue();
        g.gameObject.SetActive(true);
        return g;
    }
    public void ReturnBossGrenade(BossGrenade g)
    {
        g.gameObject.SetActive(false);
        poolingBossGrenade.Enqueue(g);
    }

    private void CreateKnifeBullet()
    {
        KnifeBullet newObj = Instantiate(knifeBullet).GetComponent<KnifeBullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(knifeBulletPos);
        poolingKnifeBulletBoss.Enqueue(newObj);
    }

    public KnifeBullet GetKnifeBullet(Quaternion q)
    {
        if (poolingKnifeBulletBoss.Count <= 0)
        {
            CreateKnifeBullet();
        }

        KnifeBullet obj = poolingKnifeBulletBoss.Dequeue();
        obj.gameObject.transform.rotation = q;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnKnifeBullet(KnifeBullet obj)
    {
        obj.gameObject.SetActive(false);

        poolingKnifeBulletBoss.Enqueue(obj);
    }

    private void CreateFireEffect()
    {
        ParticleSystem newObj = Instantiate(fireEffect).GetComponent<ParticleSystem>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(fireEffectPos);
        poolingFireEffect.Enqueue(newObj);
    }

    public ParticleSystem GetFireEffect(Quaternion q)
    {
        if (poolingFireEffect.Count <= 0)
        {
            CreateFireEffect();
        }

        ParticleSystem obj = poolingFireEffect.Dequeue();
        obj.gameObject.transform.rotation = q;
        obj.gameObject.SetActive(true);
        obj.Play();
        return obj;
    }

    public void ReturnFireEffect(ParticleSystem obj)
    {
        obj.gameObject.SetActive(false);

        poolingFireEffect.Enqueue(obj);
    }
}
