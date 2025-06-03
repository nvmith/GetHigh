using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SecondNerf : Nerf
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
                GuageIncrease();
                break;
            case 1:
                AimMiss();
                break;
            case 2:
                BlackDrug();
                break;
        }

        isActive = true;
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Drug_Addiction + value);
        DrugManager.Instance.nerfsIndex[1] = (int)EDiaryValue.Drug_Addiction + value;
    }

    /*
    public void RunFirstNerf()
    {
        if (!isActive) return;
        secondNerfValue = (ESecondNerf)Random.Range(0, System.Enum.GetValues(typeof(ESecondNerf)).Length);

        switch (secondNerfValue)
        {
            case ESecondNerf.GuageIncrease:
                GuageIncrease();
                break;
            case ESecondNerf.BlackDrug:
                BlackDrug();
                break;
            case ESecondNerf.AimMiss:
                AimMiss();
                break;
        }

        isActive = true;
    }*/

    public  void GuageIncrease() // ���� ������ ����
    {
        DrugManager.Instance.gaugeUp = true;
        Debug.Log("���� ������ ����");
    }

    public void AimMiss() // ���� �̽�
    {
        DrugManager.Instance.aimMissCheck = true;
        Debug.Log("���� �̽� Ȱ��ȭ");
    }

    public void BlackDrug() // ���� ����
    {
        DrugManager.Instance.colorBlindCheck = true;
        Debug.Log("���� ���� Ȱ��ȭ");
    }


}
