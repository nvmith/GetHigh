using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
	private Animator animator;
	public Button[] selectButton;
	private int buttonIndex = 0;
	

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
        SelectButtonOn();
	}

	public void SelectButtonOn()
	{
		selectButton[buttonIndex].Select();
	}

	public void UpdateIndex(int i)
	{
		buttonIndex = i;
	}

	public void Restart() //재시작
	{
        UIManager.Instance.timerUI.SaveTimer();

        GameManager.Instance.SaveData();
        Time.timeScale = 1f;
        GameManager.Instance.ResetClearData();

        SceneManager.LoadScene(1);
    }
	public void Setting() //설정
	{
		Debug.Log("설정");
	}
	public void GoMenu()
	{
		UIManager.Instance.timerUI.SaveTimer();

        GameManager.Instance.SaveData();
		Time.timeScale = 1f;
        SceneManager.LoadScene(0);
		//Debug.Log("게임매니저 타임  = " + GameManager.Instance.PlayTime);
	}

	public void Close()
	{
		animator.SetBool("Open", false);
	}

	public void PauseOff()
	{
		UIManager.Instance.IsPopup = 0;
		gameObject.SetActive(false);
	}

	public void PauseOn()
	{
		UIManager.Instance.IsPopup = 1;
	}

	public void SoundOn()
	{
        SoundManager.Instance.PlaySFX(SFX.MouseClick);
    }

}
