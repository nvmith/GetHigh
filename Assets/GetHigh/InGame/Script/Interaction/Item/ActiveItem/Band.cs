using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Band : Item
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
        if (DrugManager.Instance.isCurse) return;
        if (DrugManager.Instance.itemBanCheck && !isProduct) return;

        if(isProduct)
        {
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Merchant);
            ItemUIPlay(false);
            isProduct = false;
            shop.ItemSoldout(shopIndex);
        }

        SoundManager.Instance.PlaySFX(SFX.ItemPickUp);
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Band);
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        UseBand();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseBand()
    {
        // 로직 수정
        if (DrugManager.Instance.bandNerf)
        {
            int infect = Random.Range(1, 11);
            Debug.Log("값 : " + infect);
            if (infect == 1)
                return;
        }

        SoundManager.Instance.PlaySFX(SFX.UseBand);
        InGameManager.Instance.HealHp(1);
        UIManager.Instance.BulletproofPosUpdate();
    }
    // 콜라이더 추가
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }


}
