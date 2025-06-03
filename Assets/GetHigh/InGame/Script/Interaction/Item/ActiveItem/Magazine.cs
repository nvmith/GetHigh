using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item
{
    public int value = -1; // 0은 주무기 탄창, 1은 보조무기 탄창

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
        if (DrugManager.Instance.itemBanCheck && !isProduct) return;

        if (isProduct)
        {
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Merchant);
            ItemUIPlay(false);
            isProduct = false;
            shop.ItemSoldout(shopIndex);
        }
        SoundManager.Instance.PlaySFX(SFX.ItemPickUp);
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.MainMagazine + value);
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        UseMagazine();
        PoolManager.Instance.ReturnMagzine(this, value);
    }

    public void UseMagazine()
    {
        InGameManager.Instance.GetMagazine(value);
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
