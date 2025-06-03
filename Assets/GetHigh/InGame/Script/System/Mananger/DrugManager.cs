using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum EDrugColor
{
    red, orange, yellow, green, blue
}

public class DrugManager : MonoBehaviour
{
    private static DrugManager instance;
    public static DrugManager Instance => instance;

    // buff
    public EDrugColor[] buffSteps= {EDrugColor.red,EDrugColor.red,EDrugColor.red};
    public ColorBuff[] colorBuffs;
    public bool[] isBuffStepActive = { false, false, false};

    // nerf
    public Nerf[] nerfs;
    public int[] nerfsIndex = { 0, 0, 0 };

    public int[] tempStackDrug = { 0, 0, 0, 0, 0 }; // 현재 락에서 먹은 마약 개수 체크
    public int[] stackDrug = { 0, 0, 0, 0, 0 };// red, orange, yellow, green, blue 순, 현재 락 전까지 먹은 마약 개수 
    public int fullStackDrug = 0; // 이전 락까지의 마약 총 누적량
    public int curStackDrug = 0; // 현재 락의 마약 개수

    private int duffIndex = -1;

    public ShaderEffect shaderEffect;

    //Red 
    public bool red1;
    public bool red2;
    public bool red3;

    public bool MaxHPUp;
    public int power;
    public int powerUpValue;
    public bool isAnger = false;
    public int angerPower = 0;
    public float curAngryTimer = 0;
    public float angrtTimer = 2.5f;

    //Orange
    public bool orange1;
    public bool orange2;
    public bool orange3;

    public float aim;
    public bool isBulletSizeUp = false;
    public bool isBulletPass = false;
    public bool isExecution = false;

    //Yellow
    public bool yellow1;    
    public bool yellow2;
    public bool yellow3;
    
    public float playerAttackRange;
    public bool isDistanceDamage = false;
    public bool isBleeding = false;
    public bool isBomb = false;

    //Green
    public bool green1;
    public bool green2;
    public bool green3;

    public float speed;
    public bool isRollSpeedUp = false;
    public bool isBulletAvoid = false;
    public float timeValue = 1.0f;

    //Blue
    public bool blue1;
    public bool blue2;
    public bool blue3;

    public float playerAttackDelay;
    public bool islucianPassive = false;
    public bool isManyWeapon = false;
    public float reloadSpeed = 1.0f;
    public int maxBullet;

    // 보류 (단계별로 변수를 나누고 함수에서 활성화)
    public bool hostHateCheck = false;
    public bool bandNerf = false;
    public bool bombMissCheck = false;
    public bool gaugeUp= false;
    public bool colorBlindCheck = false;
    public bool aimMissCheck = false;
    public bool isRollBan = false;
    public bool itemBanCheck = false;
    public bool doubleDamageCheck = false;

    // 신기루
    public bool mirageCheck = false; // 신기루 터진지 체크
    public bool isCurse = false; // 피1 고정 체력회복 불가
    public bool isCrazy = false; // 좌우키 반전

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        //Debug.Log("1 / 2.0f 값 : " + (1 / 2.0f)); // 변수 테스트
        //Debug.Log("1 * 0.5f 값 : " + (1 * 0.5f)); // 변수 테스트
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnger)
        {
            if (angerPower == 0) return;

            curAngryTimer += Time.deltaTime; // 퀵실버와 같은 3단계이므로 언스케일 칠 필요x

            if (curAngryTimer >= angrtTimer)
            {
                angerPower = 0;
                InGameManager.Instance.player.RageUpdate(angerPower);
            }
        }
    }

    // DrugGague Check
    public void LockCheck(float gauge)
    {
        Debug.Log("락 체크");

        if (gauge >= 75)
        {
            duffIndex = 2;
        }
        else if (gauge >= 50)
        {
            duffIndex = 1;
        }
        else if (gauge >= 25)
        {
            duffIndex = 0;
        }
        else duffIndex = -1;

        if(duffIndex != -1 && !isBuffStepActive[duffIndex])
        {
            isBuffStepActive[duffIndex] = true;
            Debug.Log("락 실행");

            for (int i=0; i<tempStackDrug.Length; i++)
            {
                curStackDrug += tempStackDrug[i];   
            }

            LockActive();
        }

        if (gauge == 100) Mirage(); 
    }

    // 락 활성화, 만약 숫자가 겹침(빨 0~10까지, 주황 10~20까지, 값이 10이면 순차적으로 빨 실행)
    private void LockActive()
    {
        float curGauge = 0;// 시작 게이지 값
        float stackGauge = 0; // 현재 누적된 게이지 양
        float value = Random.Range(0.1f, 99.9f);

        Debug.Log("현재 랜덤 값 : " + value);

        for (int i=0; i<stackDrug.Length; i++)
        {
            stackGauge += 100 * ((tempStackDrug[i] + stackDrug[i] * 0.5f) / (curStackDrug + fullStackDrug * 0.5f));
            Debug.Log(i + " 인덱스의 범위 값 :  " + stackGauge);
            Debug.Log("curStackDrug + fullStackDrug * 0.5f : " + (curStackDrug + fullStackDrug * 0.5f));
            Debug.Log("tempStackDrug[i] + stackDrug[i] * 0.5f : " + (tempStackDrug[i] + stackDrug[i] * 0.5f));
 
            if (curGauge <= value && value <= stackGauge)
            {
                buffSteps[duffIndex] = (EDrugColor)i;
                colorBuffs[i].ExcuteBuff(duffIndex);
                Debug.Log("실행한 마약 :  " + (EDrugColor)i);
                nerfs[duffIndex].NerfOn();
                InGameManager.Instance.LockAnimationPlay(duffIndex);
                SoundManager.Instance.PlaySFX(SFX.HeartBeat);
                shaderEffect.StartDrugEffect(buffSteps[duffIndex]);
                UIManager.Instance.TabBuff((int)buffSteps[duffIndex], nerfsIndex[duffIndex], duffIndex);
                break;
            }
            curGauge = stackGauge;
        }

        for(int i=0; i<stackDrug.Length; i++)
        {
            stackDrug[i] += tempStackDrug[i];
            tempStackDrug[i] = 0;
        }

        fullStackDrug += curStackDrug;
        curStackDrug = 0;
    }
    
    // 락 해제기능 나올 시 UnBuff기능도 만들어야함

    // redbuffs (if문 추가 해야됨)
    public void RunRedBuff1()
    {
        InGameManager.Instance.MaxHPUpdate();
    }

    //체력이 깍이거나 회복할 때 로직 실행하게 구현
    public void RunRedBuff2()
    {
        switch (InGameManager.Instance.Hp)
        {
            case 4:
                powerUpValue = 50;
                break;
            case 3:
                powerUpValue = 100;
                break;
            case 2:
                powerUpValue = 150;
                break;
            case 1:
                powerUpValue = 250;
                break;
            default: 
                powerUpValue = 0;
                break;
        }
    }

    public void RunRedBuff3()
    {
        if (red3)
        {
            isAnger = true;

            foreach(Image i in InGameManager.Instance.player.RageImages)
            {
                i.gameObject.SetActive(true);
            }
        }
    }

    public void AngryCount()
    {
        if (angerPower < 5)
        {
            angerPower++;
            InGameManager.Instance.player.RageUpdate(angerPower);
        }
        curAngryTimer = 0;
    }

    // orangeBuff
    public void RunOrangeBuff1()
    {
        if(orange1)
            isBulletSizeUp = true;
    }

    public void RunOrangeBuff2()
    {
        if (orange2) isBulletPass = true;
    }

    public void RunOrangeBuff3()
    {
        if (orange3) isExecution = true;
    }

    //yellowBuff
    public void RunYellowBuff1()
    {
        if (yellow1) isDistanceDamage = true;
   
    }

    public void RunYellowBuff2()
    {
        if (yellow2) isBleeding = true;
    }
    public void RunYellowBuff3()
    {
        if (yellow3) isBomb = true;
    }

    //greenBuff
    public void RunGreenBuff1()
    {
        if (green1) isRollSpeedUp = true;
    }

    public void RunGreenBuff2()
    {
        if (green2) isBulletAvoid = true;

    }

    public void RunGreenBuff3()
    {
        if (green3)
        {
            Time.timeScale = 0.8f;
        }
    }

    public void RunBlueBuff1()
    {
        if(blue1)
        {
            islucianPassive = true;
        }
    }
    public void RunBlueBuff2()
    {
        if(blue2)
        {
            isManyWeapon = true;
        }
    }
    public void RunBlueBuff3()
    {
        if(blue3)
        {
            reloadSpeed = 0.5f;
            SoundManager.Instance.reloadSource.pitch = 2;

            for(int i=0; i<4;i++)
            {
                //InGameManager.Instance.magazineInven[i] *= 2;
                InGameManager.Instance.bulletMagazine[i] *= 2;
            }

            if (InGameManager.Instance.curWeaponIndex == 4) return;

            UIManager.Instance.inGameUI.BulletTextInput(
                InGameManager.Instance.curBullet[InGameManager.Instance.curWeaponIndex],
                InGameManager.Instance.bulletMagazine[InGameManager.Instance.curWeaponIndex]);
        }
    }

    public void Mirage()
    {
        if (mirageCheck) return;
        mirageCheck = true;
        int index = Random.Range(0, 2);
        Debug.Log("신기루 인덱스 : " + index);
        InGameManager.Instance.LockAnimationPlay(3);
        SoundManager.Instance.PlaySFX(SFX.HeartBeat);
        shaderEffect.StartMirage();
        UIManager.Instance.TabUI.MirageOn();

        switch (index)
        {
            case 0:
                isCurse = true;
                InGameManager.Instance.Hit(InGameManager.Instance.Hp -1);
                UIManager.Instance.hpUpdate();
                break;
            case 1:
                isCrazy = true;
                break;
        }

        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Mirage);
    }
}
