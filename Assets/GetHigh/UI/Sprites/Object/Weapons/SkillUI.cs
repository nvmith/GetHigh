using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
	public Sprite[] PlayerSkillIcon;
	public Image SkillIcon;

	public Image skillFilter; 
	public TextMeshProUGUI coolTimeCounter;
	public float coolTime;     
	private float currentCoolTime; 
	private bool canUseSkill = true;
	public bool CanUseSkill => canUseSkill;
	void Start() 
	{
		SkillIcon.sprite = PlayerSkillIcon[
			(int)GameManager.Instance.selectCharacter];
		skillFilter.sprite = SkillIcon.sprite;

        skillFilter.fillAmount = 0; 
	}

	void Update()
	{

	}
	public void UseSkill()    
	{
		if (canUseSkill)
		{
			Debug.Log("Use Skill");
			skillFilter.fillAmount = 1;         

			StartCoroutine("Cooltime"); currentCoolTime = coolTime;
			coolTimeCounter.text = "" + currentCoolTime;
			StartCoroutine("CoolTimeCounter");
			canUseSkill = false;        
		}
		else 
		{ 
			Debug.Log("아직 스킬을 사용할 수 없습니다."); 
		}    
	}

	IEnumerator Cooltime()
	{
		while (skillFilter.fillAmount > 0)
		{
			skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
			yield return null;
		}
		canUseSkill = true;        
		yield break;
	}   

	IEnumerator CoolTimeCounter()    
	{        
		while(currentCoolTime > 0)        
		{            
			yield return new WaitForSeconds(1.0f);             
			currentCoolTime -= 1.0f;
			coolTimeCounter.text = "" + currentCoolTime;
		}
		coolTimeCounter.text = "";
		yield break;    
	}
}
