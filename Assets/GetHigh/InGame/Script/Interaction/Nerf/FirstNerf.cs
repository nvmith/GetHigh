using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstNerf : Nerf
{

    protected void Awake()
    {
        
    }
    protected void Start()
    {
        nerfCount = 3;
    }
    protected void Update()
    {
        
    }

    protected override void ActiveLogic(int value)
    {
        if (isActive) return;
        //firstNerfValue = (EFirstNerf)Random.Range(0, System.Enum.GetValues(typeof(EFirstNerf)).Length);

        switch (value)
        {
            case 0:
                HostHate();
                break;
            case 1:
                InfectedBand();
                break;
            case 2:
                BombMiss();
                break;
        }

        isActive = true;
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Merchant_Hostility + value);
        DrugManager.Instance.nerfsIndex[0] = (int)EDiaryValue.Merchant_Hostility + value;
    }

    /*
    public void RunFirstNerf()
    {
        if (!isActive) return;
        firstNerfValue = (EFirstNerf)Random.Range(0, System.Enum.GetValues(typeof(EFirstNerf)).Length);

        switch (firstNerfValue)
        {
            case EFirstNerf.HostHate:
                HostHate();
                break;
            case EFirstNerf.InfectedBandage:
                InfectedBandage();
                break;
            case EFirstNerf.BombMiss:
                BombMiss();
                break;
        }

        isActive = true;
    }
    */


    public void HostHate() // 상점 비용증가
    {
        DrugManager.Instance.hostHateCheck = true;
        Debug.Log("상점가격증가");
    }

    public void InfectedBand() // 오염된 붕대
    {
        DrugManager.Instance.bandNerf = true;
        Debug.Log("붕대 오염 활성화");
    }

    public void BombMiss() // 불발수류탄
    {      
        DrugManager.Instance.bombMissCheck = true;
        Debug.Log("불발 수류탄 활성화");
    }
}
