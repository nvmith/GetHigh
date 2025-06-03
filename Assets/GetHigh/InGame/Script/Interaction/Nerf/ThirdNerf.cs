using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;


public class ThirdNerf : Nerf
{

    private void Awake()
    {
       
    }
    private void Start()
    {
        nerfCount = 3;
    }
    private void Update()
    {
        
    }

    protected override void ActiveLogic(int value)
    {
        if (isActive) return;
        //firstNerfValue = (EFirstNerf)Random.Range(0, System.Enum.GetValues(typeof(EFirstNerf)).Length);

        switch (value)
        {
            case 0:
                RollBan();
                break;
            case 1:
                ItemBan();
                break;
            case 2:
                DoubleDamage();
                break;
        }

        isActive = true;
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Roll_Restriction + value);
        DrugManager.Instance.nerfsIndex[2] = (int)EDiaryValue.Roll_Restriction + value;
    }

    /*
    public void RunThirdNerf()
    {
        if (!isActive) return;

        thirdNerfValue = (EThirdNerf)Random.Range(0, System.Enum.GetValues(typeof(EThirdNerf)).Length);

        switch (thirdNerfValue)
        {
            case EThirdNerf.RollBan:
                RollBan();
                break;
            case EThirdNerf.ItemBan:
                ItemBan();
                break;
            case EThirdNerf.DoubleDamage:
                DoubleDamage();
                break;
        }

        isActive = true;
    }*/

    public void RollBan() // ������ ����
    {
        DrugManager.Instance.isRollBan = true;
        Debug.Log("������ ���� Ȱ��ȭ");
    }

    public void ItemBan() // ���� ���⸸ ��ü �� ���밡��
    {
        DrugManager.Instance.itemBanCheck = true;
        Debug.Log("������ �Ұ� Ȱ��ȭ");
    }

    public void DoubleDamage() // ������ 2��
    {
        DrugManager.Instance.doubleDamageCheck = true;
        Debug.Log("������ 2�� Ȱ��ȭ");
    }
}
