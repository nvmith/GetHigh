using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletproof : Item
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
        if (UIManager.Instance.isBulletProof) return;

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
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.BulletProof);
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        if (UIManager.Instance.isBulletProof) return;

        UseBulletproof();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseBulletproof()
    {
        /*int index = (InGameManager.Instance.Hp - 1) / 2;
        UIManager.Instance.bulletProof.rectTransform.anchoredPosition
            = new Vector2(23 + index * 57, -16);*/


        //= UIManager.Instance.heartImages[index].rectTransform.position;
        SoundManager.Instance.PlaySFX(SFX.UseBFS);
        UIManager.Instance.BulletproofUpdate(true);
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