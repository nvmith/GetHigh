using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    public EWeapons eWeapons;
    public int index; //�� ������ �ε��� ��
    public bool checkMagazine = false; // ���� ù ȹ�� �Ǵ�
    public int bulletCount = 0; // �� ������ źâ�� ź�� ����
    public float reloadSpeed = -1;
    public int power = 0; // �ܼ� UI ǥ���

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
        } // ���� �Ǹ����� üũ

        if (eWeapons == EWeapons.Revolver) // ���� ȹ�湫�Ⱑ ����������
        {
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Desert_Eagle + index);

            if (InGameManager.Instance.pistolInven != null) // ���� �������⸦ ���� ������ Ȯ��
            {
                // ���� ����
                InGameManager.Instance.PutBullet(eWeapons); // ���� �÷��̾��� ���ѿ� ź�� ����
                InGameManager.Instance.pistolInven.PutWeapon(); // ���� ���� ������ ������ ��
            }
        }
        else
        {
            GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.AK47 + index);

            if (InGameManager.Instance.gunInven != null) // ���� ȹ�湫�Ⱑ �ֹ�����
            {
                // ���� ����
                InGameManager.Instance.PutBullet(eWeapons); //  ���� �÷��̾��� �ֹ��⿡ ź�� ����

                // ���� ������ Ȱ��ȭ �� �������� Ȯ��
                if (DrugManager.Instance.isManyWeapon)
                {
                    if (InGameManager.Instance.blueGunInven == null) // ���� ���� �κ��� ����ִٸ�
                    {
                        InGameManager.Instance.blueGunInven = InGameManager.Instance.gunInven; // ���� �� ���⸦ ���� �κ����� ����
                        InGameManager.Instance.blueGunInven.gameObject.SetActive(false); // ��ü�� �ֹ��� �����
                    }
                    else
                    {
                        InGameManager.Instance.gunInven.PutWeapon(); // ���� ���� ���� ��ȯ
                    }

                }
                else //���� ������ Ȱ��ȭ �� ���°� �ƴ϶��
                {
                    InGameManager.Instance.gunInven.PutWeapon(); // ���� �ֹ��⸦ ������ ��
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
            Debug.Log("���� �Ѿ� ī��Ʈ : " + bulletCount);
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
