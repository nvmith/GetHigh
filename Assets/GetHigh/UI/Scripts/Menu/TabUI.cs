using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
	private Animator animator;

	// 공격력, 정확성, 사거리, 기동력, 연사력
	public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI bulletAccuracy;
    public TextMeshProUGUI bulletDistance;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI attackSpeed;

	public TextMeshProUGUI characterName;

	public Image[] buffImage;
    public Image[] deBuffImage;
	public Image mirage;
	public Image[] line;
	public Image characterIllust;
	public Sprite[] characters;

    private void Awake()
	{
		animator = GetComponent<Animator>();
	}

    private void Start()
    {
        characterIllust.sprite = characters[(int)GameManager.Instance.selectCharacter];
    }

    public void Init()
    {
        characterIllust.sprite = characters[(int)GameManager.Instance.selectCharacter];
    }

    public void Close()
	{
		animator.SetBool("Open", false);
	}

	public void TabOff()
	{
		UIManager.Instance.IsTab = false;
		gameObject.SetActive(false);
	}

	public void TabOn()
	{
        gameObject.SetActive(true);

		int power = InGameManager.Instance.Power;
		int buffPower = InGameManager.Instance.PlayerBuffPower();

        float aim = InGameManager.Instance.Aim + DrugManager.Instance.aim;
		float distance = InGameManager.Instance.BulletDistance + DrugManager.Instance.playerAttackRange;
		float playerSpeed = InGameManager.Instance.Speed + DrugManager.Instance.speed;
		float playerAttackSpeed = InGameManager.Instance.AttackDelay + DrugManager.Instance.playerAttackDelay;

        attackDamage.text = (power+buffPower) + "( " + power
            + " + " + buffPower + " )";
        bulletAccuracy.text = aim + "( " + InGameManager.Instance.Aim
			+ " + " + DrugManager.Instance.aim + " )";
        bulletDistance.text = distance * 10 + "( " + InGameManager.Instance.BulletDistance * 10
            + " + " + DrugManager.Instance.playerAttackRange * 10 + " )"; // 표기만 10 곱해서 표현
        speed.text = playerSpeed + "( " + InGameManager.Instance.Speed
            + " + " + DrugManager.Instance.speed + " )";
        attackSpeed.text = playerAttackSpeed + "( " + InGameManager.Instance.AttackDelay
            + " + " + DrugManager.Instance.playerAttackDelay + " )";

		PositionChange();

		characterName.text = GameManager.Instance.selectCharacter.ToString();
    }

	public void PositionChange()
	{
		if(GameManager.Instance.languageIndex ==0)
		{
			attackDamage.rectTransform.localPosition = new Vector3(210, 0, 0);
			bulletAccuracy.rectTransform.localPosition = new Vector3(210, 0, 0);
            bulletDistance.rectTransform.localPosition = new Vector3(210, 0, 0);
            speed.rectTransform.localPosition = new Vector3(210, 0, 0);
            attackSpeed.rectTransform.localPosition = new Vector3(210, 0, 0);
        }
		else
		{
            attackDamage.rectTransform.localPosition = new Vector3(165, 0, 0);
            bulletAccuracy.rectTransform.localPosition = new Vector3(165, 0, 0);
            bulletDistance.rectTransform.localPosition = new Vector3(165, 0, 0);
            speed.rectTransform.localPosition = new Vector3(165, 0, 0);
            attackSpeed.rectTransform.localPosition = new Vector3(165, 0, 0);
        }
	}

	public void MirageOn()
	{
		for (int i = 0; i < 3; i++)
		{
			buffImage[i].enabled = false;
			deBuffImage[i].enabled = false;
		}

		line[0].enabled = false;
		line[1].enabled = false;

		mirage.enabled = true;
	}

	public void Onbuff(int level)
	{
		buffImage[level].enabled = true;
		deBuffImage[level].enabled = true;

		if (level == 1) line[0].enabled = true;
        else if(level == 2) line[1].enabled = true;
    }
}
