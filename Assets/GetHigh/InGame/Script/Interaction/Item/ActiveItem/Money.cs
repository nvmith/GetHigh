using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    protected override void Awake()
    {
        base.Awake();
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
        if (DrugManager.Instance.itemBanCheck) return;

        SoundManager.Instance.PlaySFX(SFX.MoneyPickUp);
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Money);
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        GetMoney();

        PoolManager.Instance.ReturnMoney(this);
    }

    public void GetMoney()
    {
        // ���� ����
        InGameManager.Instance.UpdateMoney(1);
    }
    // �ݶ��̴� �߰�
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //base.OnTriggerEnter2D(collision);

        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(true);

            GetItem();
            //InGameManager.Instance.tempItems.Add(this);
            //playerCheck = true;
            //Debug.Log("����Ʈ üũ(�߰�) : " + InGameManager.Instance.tempItems.Count);
        }
        else if (collision.tag == "Wall" || collision.tag == "MapObject")
        {
            itemRigid.velocity = Vector3.zero;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        //base.OnTriggerExit2D(collision);
    }
}
