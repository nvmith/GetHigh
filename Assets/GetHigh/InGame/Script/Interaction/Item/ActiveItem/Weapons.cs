using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    public EWeapons eWeapons;
    public int index; //각 무기의 인덱스 값
    public bool checkMagazine = false; // 무기 첫 획득 판단
    public int bulletCount = 0; // 각 무기의 탄창의 탄알 개수
    public float reloadSpeed = -1;
    public int power = 0; // 단순 UI 표기용

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
        InGameManager.Instance.player.isAttack = false;
        if (InGameManager.Instance.player.isReload)
        {
            InGameManager.Instance.player.CancleReload();
        }


        if (isProduct)
        {
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Merchant);
            isProduct = false;
            ItemUIPlay(false);
            shop.ItemSoldout(shopIndex);
        } // 상점 판매인지 체크

        if (eWeapons == EWeapons.Revolver) // 현재 획득무기가 보조무기라면
        {
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Desert_Eagle + index);

            if (InGameManager.Instance.pistolInven != null) // 현재 보조무기를 보유 중인지 확인
            {
                // 수정 로직
                InGameManager.Instance.PutBullet(eWeapons); // 현재 플레이어의 권총에 탄을 저장
                InGameManager.Instance.pistolInven.PutWeapon(); // 보유 중인 권총을 밖으로 뺌
            }
        }
        else
        {
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.AK47 + index);

            if (InGameManager.Instance.gunInven != null) // 현재 획득무기가 주무기라면
            {
                // 수정 로직
                InGameManager.Instance.PutBullet(eWeapons); //  현재 플레이어의 주무기에 탄을 저장

                // 마약 버프가 활성화 된 상태인지 확인
                if (DrugManager.Instance.isManyWeapon)
                {
                    if (InGameManager.Instance.blueGunInven == null) // 마약 무기 인벤이 비어있다면
                    {
                        InGameManager.Instance.blueGunInven = InGameManager.Instance.gunInven; // 현재 주 무기를 마약 인벤으로 저장
                        InGameManager.Instance.blueGunInven.gameObject.SetActive(false); // 교체된 주무기 숨기기
                    }
                    else
                    {
                        InGameManager.Instance.gunInven.PutWeapon(); // 현재 장착 무기 반환
                    }

                }
                else //마약 버프가 활성화 된 상태가 아니라면
                {
                    InGameManager.Instance.gunInven.PutWeapon(); // 보유 주무기를 밖으로 뺌
                }
            }
        }

        SoundManager.Instance.PlaySFX(SFX.ItemPickUp);
        GameManager.Instance.CheckEunha();
        //GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Desert_Eagle + (int)eWeapons);
        UseItem();
    }

    public override void UseItem()
    {
        GetWepaon();
    }

    public void GetWepaon()
    {
        if (!checkMagazine)
        {
            checkMagazine = true;
            
            if(DrugManager.Instance.blue3) bulletCount *= 2;
            Debug.Log("지금 총알 카운트 : " + bulletCount);
        }

        InGameManager.Instance.UpdateWeapon(eWeapons, this);
        gameObject.SetActive(false);
    }
   
    public void PutWeapon()
    {
        gameObject.SetActive(true);

        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }

    public void PutWeapon(float power)
    {
        itemRigid.AddForce(Vector2.up * power, ForceMode2D.Impulse);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
