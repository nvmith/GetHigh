using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
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
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Key);
        //InGameManager.Instance.CheckGirl(); // 수정 전 로직 -> 모든 아이템 + 무기 
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        UseKey();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseKey()
    {
        // 로직 수정
        //InGameManager.Instance.numKey++;
        Debug.Log("열쇠 먹음");
        InGameManager.Instance.UpdateKey(1);
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
