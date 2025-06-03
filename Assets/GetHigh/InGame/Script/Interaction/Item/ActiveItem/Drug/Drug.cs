using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Drug : Item
{
    public int drugGuage;
    private SpriteRenderer drugSprite;
    [SerializeField]
    protected EDrugColor value;
    public Sprite blackDrugSprite;
    public Sprite curDrugSprite;

    public bool mapDrug = false; // �̹� ������ �������� üũ, �ʿ� �ִ� ������
    // �̹� ������ �����̹Ƿ� ���͸���� ������ ����ҷ���

    public int drugIndex = 0; // ���̾�� �� �巯�� �ε���

    protected override void Awake()
    {
        base.Awake();
        drugSprite = GetComponent<SpriteRenderer>();
        curDrugSprite = drugSprite.sprite;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (DrugManager.Instance == null) return;
        else if (DrugManager.Instance.colorBlindCheck && !mapDrug)
        {
            drugSprite.sprite = blackDrugSprite;
            curDrugSprite = blackDrugSprite;
            //Debug.Log("������");
            drugIndex = 6;
        }
        else
        {
            drugSprite.sprite = curDrugSprite;
            //Debug.Log("���� �������");
            mapDrug = true; // ���� ���� �� �������� ���
            drugIndex = (int)value + 1;
        }

        // ������ ���߿� ���� ������ ��� ���ķ� Ȯ���غ�����
    }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void GetItem()
    {
        if (isProduct)
        {
            if (DrugManager.Instance.hostHateCheck) curPrice = (price * 6) / 5; // ���� 20�������, 1.2�� ����
            else curPrice = price;
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Merchant);

            isProduct = false;
            ItemUIPlay(false);
            shop.DrugSoldout(shopIndex);
        }

        SoundManager.Instance.PlaySFX(SFX.DrugPickUp);
        if (InGameManager.Instance.drugInven != null)
        {
            //InGameManager.Instance.drugInven.gameObject.SetActive(true);
            InGameManager.Instance.drugInven.PutDrug();     
        }

        InGameManager.Instance.drugInven = this;
        UIManager.Instance.inGameUI.DrugInven(drugSprite.sprite);
        GameManager.Instance.UpdateDiaryDate(drugIndex);

        base.GetItem();
    }

    public override void UseItem()
    {
        if (isProduct) return;

        SoundManager.Instance.PlaySFX(SFX.UseDrug);
        GetDrug();
        InGameManager.Instance.UpdateDrug(drugGuage);
        DrugAbility();
        mapDrug = false;
        GameManager.Instance.UpdateDiaryDate(drugIndex);

        PoolManager.Instance.ReturnDrug(this, value);
    }

    public void GetDrug()
    {
        DrugManager.Instance.tempStackDrug[(int)value]++;
        GameManager.Instance.clearDrugCount++;

        if(DrugManager.Instance.gaugeUp)
        {
            drugGuage = Random.Range(5, 8);
            return;
        }
        drugGuage = Random.Range(3, 5);
    }

    public void PutDrug()
    {
        mapDrug = true;
        gameObject.SetActive(true);
        drugSprite.sprite = curDrugSprite;
        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }
    protected virtual void DrugAbility()
    {

    }

    // �ݶ��̴� �߰�

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(true);

            InGameManager.Instance.tempItems.Add(this);
            InGameManager.Instance.tempDrug.Add(this);
            playerCheck = true;
            Debug.Log("���� ���� ������ : " + gameObject.name);
        }
        else if (collision.tag == "Wall" || collision.tag == "MapObject")
        {
            itemRigid.velocity = Vector3.zero;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(false);
            SilhouetteCheck(false);
            Debug.Log("���� ���� ������ : " + gameObject.name);
            InGameManager.Instance.tempItems.Remove(this);
            InGameManager.Instance.tempDrug.Remove(this);
            distance = 999f;
            playerCheck = false;
        }
    }

}
