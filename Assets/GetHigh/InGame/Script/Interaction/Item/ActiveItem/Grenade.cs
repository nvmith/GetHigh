using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
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
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Grenade);
        GameManager.Instance.CheckEunha();
        UseItem();
    }

    public override void UseItem()
    {
        UseGrenade();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseGrenade()
    {
        // 로직 수정
        InGameManager.Instance.UpdateGrenade(1);
        //InGameManager.Instance.numKey++;
        //StartCoroutine(Explode());
        /*
        GameObject grenade = Instantiate(InGameManager.Instance.grenadeObj
            , InGameManager.Instance.player.transform.position, InGameManager.Instance.player.transform.rotation);

        InGameManager.Instance.UpdateGrenade();*/
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
