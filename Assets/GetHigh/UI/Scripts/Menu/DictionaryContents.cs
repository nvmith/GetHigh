using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EDiaryValue
{
    None, RedDrug, OrangeDrug, YellowDrug, GreenDrug, BlueDrug, BlackDrug,
    Band, Key, BulletProof, Grenade, Money, MainMagazine, SubMagazine,
    Sashimi, Combat_Knife, Fist,
    Desert_Eagle, Anaconda,
    AK47, M870, AWP,
    Haeseong, Eunha, Kuiper,
    Strike_Team1, Mikazuki, Deimos, FullMoon,
    Merchant, Unlicensed_Doctor,
    Extra_Health, Berserker, Rage,
    Expanded_Bullet, Piercing, Execution,
    Sniper, Bleeding, Explosion,
    Dash, Evasion, Time_Dilation,
    Burst, Extra_Weapon, Weapon_Master,
    Merchant_Hostility, Contaminated_Bandage, Dud_Grenade,
    Drug_Addiction, Missed_Shot, Drug_Induced_Colorblindness,
    Roll_Restriction, Excess, Weakness_Exposure,
    Mirage
}

public class DictionaryContents : MonoBehaviour
{
    public EDiaryValue value;

    private string[] descs = new string[]
    {
      "아직 뭔지 몰라..",
        //마약 6개
        "빨간 마약", "주황 마약", "노랑 마약", "초록 마약", "파란 마약", "검정 마약",
        //아이템 7개
        "상처를 치료해줄 사람 어디 없나", "넣는 건 늘 새로워", "안아파 나 안아파",
        "폭발은 예술이다", "1000원의 행복", "주 탄창", "보조 탄창",
        //무기
        "사시미", "군용 나이프", "주먹", "데저트 이글", "아나콘다",  "AK-47", "M870", "AWP",
        //캐릭터
        "해성", "은하", "카이퍼",
        //적
        "1번 행동대", "2번 행동대", "3번 행동대", "미카즈키", "데이모스", "만월",
        //NPC
        "상인", "무면허 의사",
        //버프
        "추가 체력", "광전사", "격분",
        "확대탄", "관통", "처형",
        "저격수", "출혈", "폭발탄",
        "도움닫기", "회피", "시간 팽창",
        "점사", "추가 무기", "웨폰마스터",
        //디버프
        "상인 적대적", "오염된 붕대", "불발 수류탄",
        "마약 중독", "조준 미스", "마약 색맹",
        "구르기 금지", "과유불급", "약점 노출",
        "신기루"
   };

    private string[] names = new string[]
    {
        "미해금",
        //마약 6개
        "빨간 마약", "주황 마약", "노랑 마약", "초록 마약", "파란 마약", "검정 마약",
        //아이템 7개
        "붕대","열쇠","방탄복","수류탄", "돈", "주무기 탄창", "보조무기 탄창",
        //무기
        "사시미", "군용 나이프", "주먹", "데저트 이글", "아나콘다", "AK-47", "M870", "AWP",
        //캐릭터
        "해성", "은하", "카이퍼",
        //적
        "1번 행동대", "2번 행동대", "3번 행동대", "미카즈키", "데이모스", "만월",
        //NPC
        "상인", "무면허 의사",
        //버프
        "추가 체력", "광전사", "격분",
        "확대탄", "관통", "처형",
        "저격수", "출혈", "폭발탄",
        "도움닫기", "회피", "시간 팽창",
        "점사", "추가 무기", "웨폰마스터",
        //디버프
        "상인 적대적", "오염된 붕대", "불발 수류탄",
        "마약 중독", "조준 미스", "마약 색맹",
        "구르기 금지", "과유불급", "약점 노출",
        "신기루"
   };

    //private Image iconImage;

    public Image iconImage;
    public bool isUnLock; // 해금인지 체크
    public int itemNum;

    // Start is called before the first frame update
    private void Awake()
    {
        //itemNum = int.Parse(gameObject.name);
        if (itemNum == 0) return;

        value = (EDiaryValue)itemNum;
        //iconImage = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UnLockContent()
    {
        isUnLock = true;
        LockUpdate();
    }

    public void LockUpdate()
    {
        if (isUnLock)
            iconImage.color = Color.white;
        else
            iconImage.color = Color.black;
    }

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    /*public string GetName()
    {
        return names[itemNum];
    }
    public string GetDescription()
    {
        return descs[itemNum];
    }*/
}
