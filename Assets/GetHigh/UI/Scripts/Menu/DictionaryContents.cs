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
      "���� ���� ����..",
        //���� 6��
        "���� ����", "��Ȳ ����", "��� ����", "�ʷ� ����", "�Ķ� ����", "���� ����",
        //������ 7��
        "��ó�� ġ������ ��� ��� ����", "�ִ� �� �� ���ο�", "�Ⱦ��� �� �Ⱦ���",
        "������ �����̴�", "1000���� �ູ", "�� źâ", "���� źâ",
        //����
        "��ù�", "���� ������", "�ָ�", "����Ʈ �̱�", "�Ƴ��ܴ�",  "AK-47", "M870", "AWP",
        //ĳ����
        "�ؼ�", "����", "ī����",
        //��
        "1�� �ൿ��", "2�� �ൿ��", "3�� �ൿ��", "��ī��Ű", "���̸�", "����",
        //NPC
        "����", "������ �ǻ�",
        //����
        "�߰� ü��", "������", "�ݺ�",
        "Ȯ��ź", "����", "ó��",
        "���ݼ�", "����", "����ź",
        "����ݱ�", "ȸ��", "�ð� ��â",
        "����", "�߰� ����", "����������",
        //�����
        "���� ������", "������ �ش�", "�ҹ� ����ź",
        "���� �ߵ�", "���� �̽�", "���� ����",
        "������ ����", "�����ұ�", "���� ����",
        "�ű��"
   };

    private string[] names = new string[]
    {
        "���ر�",
        //���� 6��
        "���� ����", "��Ȳ ����", "��� ����", "�ʷ� ����", "�Ķ� ����", "���� ����",
        //������ 7��
        "�ش�","����","��ź��","����ź", "��", "�ֹ��� źâ", "�������� źâ",
        //����
        "��ù�", "���� ������", "�ָ�", "����Ʈ �̱�", "�Ƴ��ܴ�", "AK-47", "M870", "AWP",
        //ĳ����
        "�ؼ�", "����", "ī����",
        //��
        "1�� �ൿ��", "2�� �ൿ��", "3�� �ൿ��", "��ī��Ű", "���̸�", "����",
        //NPC
        "����", "������ �ǻ�",
        //����
        "�߰� ü��", "������", "�ݺ�",
        "Ȯ��ź", "����", "ó��",
        "���ݼ�", "����", "����ź",
        "����ݱ�", "ȸ��", "�ð� ��â",
        "����", "�߰� ����", "����������",
        //�����
        "���� ������", "������ �ش�", "�ҹ� ����ź",
        "���� �ߵ�", "���� �̽�", "���� ����",
        "������ ����", "�����ұ�", "���� ����",
        "�ű��"
   };

    //private Image iconImage;

    public Image iconImage;
    public bool isUnLock; // �ر����� üũ
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
