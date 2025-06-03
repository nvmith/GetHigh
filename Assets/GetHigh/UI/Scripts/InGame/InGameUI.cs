using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public SkillUI skillUI;
    public Image DrugUI;
    public Image WeaponUI;

    public Sprite[] weaponSprites;
    public int[] weaponWidth;
    public int[] weaponHeight;

    public TextMeshProUGUI bulletText;
    public int curBulletCount;
    public int maxBulletCount;
    protected readonly string kinfeSing = "��"; // �Ѿ� ���� ǥ�� ����

    public TextMeshProUGUI grenadeText;
    public TextMeshProUGUI[] magazinesText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI keyText;

    private void Awake()
    {

    }

    void Start()
    {
        InitUI();
        WeaponInven(5 + InGameManager.Instance.playerWeaponType); // ���� ĳ���͸��� Į�� �ٸ��ٸ� player���� �ʱ�ȭ�� ��Ű���� ����
        KnifeTextUpdate();
        DrugInven(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ü��, ����, ��ų ��Ÿ��, �� �� �÷��̾� �⺻ UI ��ġ ����
    private void InitUI()
    {
        skillUI.coolTime = InGameManager.Instance.player.SkillDelay;

        MoneyUpdate(0);
        GrenadeUpdate(0);
        MagazineUpdate(0, 0);
        MagazineUpdate(1, 0);
        KeyUpdate(0);
    }

    public void DrugInven(Sprite s)
    {
        if (s == null) DrugUI.enabled = false;
        else
        {
            DrugUI.enabled = true;
            DrugUI.sprite = s;
        }
    }

    public void WeaponInven(int index)
    {
        WeaponUI.sprite = weaponSprites[index];
        WeaponUI.rectTransform.sizeDelta = new Vector2(weaponWidth[index], weaponHeight[index]);
    }

    public void BulletTextUpdate(int bulletCunt)
    {
        bulletText.text = bulletCunt + " / " + maxBulletCount;
    }

    public void BulletTextInput(int curBulletCnt, int maxBulletCnt)
    {
        maxBulletCount = maxBulletCnt;

        BulletTextUpdate(curBulletCnt);
    }

    public void KnifeTextUpdate()
    {
        bulletText.text = kinfeSing;
    }

    public void GrenadeUpdate(int a)
    {
        grenadeText.text = a.ToString();
    }

    public void MagazineUpdate(int value, int cnt)
    {
        magazinesText[value].text = cnt.ToString();
    }

    public void MoneyUpdate(int cnt)
    {
        moneyText.text = cnt.ToString();
    }

    public void KeyUpdate(int cnt)
    {
        keyText.text = cnt.ToString();
    }
}
