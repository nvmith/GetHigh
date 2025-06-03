using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;
	public static UIManager Instance => instance;

	public PauseUI pauseUI;
	public TabUI TabUI;
	public DictionaryUI DictUI;
	public GameObject ExitUI;
	public InGameUI inGameUI;
    public GameObject TempUI;
    public TimerUI timerUI;
    public GameOverUI gameOverUI;

    public Image[] heartImages;
    public Sprite[] heartSprites;
    public Image bulletProof;
    public bool isBulletProof = false;
    private int hp;

    [HideInInspector]
    public int IsPopup = 0;
    [HideInInspector]
	public bool IsTab = false;
	[HideInInspector]
	public bool IsDict = false;

	//InputKey
	private bool isEscKey;
	private bool isPauseKey;
	private bool isTabKey;
	private bool isInventoryKey;

    private void Awake()
	{
		Init();
		hpInit();

    }

	void Start()
	{
        GameManager.Instance.dictionaryUI = DictUI;
	}

	// Update is called once per frame
	void Update()
	{
		//InputKey();

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !IsDict)
        {
            //Debug.Log("지금: " + IsPopup);
            OpenPause();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !IsTab)
        {
            OpenTab();
            //Debug.Log("Open");
        }

        if (Input.GetKeyUp(KeyCode.Tab) && IsTab)
        {
            OpenTab();
            //Debug.Log("Close");
        }

        if (Input.GetKeyDown(KeyCode.I) && IsPopup == 0)
        {
            OpenDict();
        }

    }

    private void Init()
	{
		if (Instance == null)
		{
			instance = this;
		}
	}

	//public void InitPlayer

	/*private void InputKey()
	{
		isEscKey = Input.GetKeyDown(KeyCode.Escape);
		isPauseKey = Input.GetKeyDown(KeyCode.P);
		isTabKey = Input.GetKeyDown(KeyCode.Tab);
		isInventoryKey = Input.GetKeyDown(KeyCode.I);
    }*/

	private void OpenDict() //도감
	{
		//if (!isInventoryKey) return;

		if (!IsDict)
		{
            IsDict = true;
			DictUI.gameObject.SetActive(true);
            //DictUI.ContentUpdate(); // 잠시 주석, 궂이 킬때마다 다 갱신할 필요없을듯
			PauseTime(true);
        }
		else
		{
            PauseTime(false);
			DictUI.Close();
        }
	}

	private void OpenTab() //탭
	{
		//if (!isTabKey) return;

		if (!IsTab)
		{
			IsTab = true;
            TabUI.TabOn();
            //TabUI.gameObject.SetActive(true);
		}
		else
		{
			TabUI.Close();
		}
	}


    private void OpenPause() //일시정지
	{
        switch (IsPopup)
        {
            case 0:
                PauseTime(true);
                pauseUI.gameObject.SetActive(true);
                break;
            case 1:
                PauseTime(false);
                pauseUI.Close();
                IsPopup--;
                break;
            case 2:
                TempUI.SetActive(false);
                break;
        }

    }

    public void hpInit()
    {
        int hp = InGameManager.Instance.MaxHp;

        for(int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }

        for (int i = hp / 2; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(false);
        }
        hpUpdate();
    }

    public void hpUpdate()
    {
        int hp = InGameManager.Instance.Hp;

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = heartSprites[2];
        }
        for (int i = 0; i < hp / 2; i++)
        {
            heartImages[i].sprite = heartSprites[0];
        }
        if (hp % 2 == 1)
        {
            heartImages[hp / 2].sprite = heartSprites[1];
        }
    }

    public void BulletproofUpdate(bool isActive)
    {
        isBulletProof = isActive;

        Color c = bulletProof.color;
        c.a = isActive ? 1 : 0;
        bulletProof.color = c;
        BulletproofPosUpdate();
    }

    public void BulletproofPosUpdate()
    {
        int index = (InGameManager.Instance.Hp - 1) / 2;
        bulletProof.rectTransform.anchoredPosition
            = new Vector2(30 + index * 72, -24.75f);
    }

    public void TabBuff(int buffIndex, int deBuffIndex, int buffLevel)
    {
        buffIndex = ((int)EDiaryValue.Extra_Health + (buffIndex * 3 + buffLevel));

        TabUI.Onbuff(buffLevel);

        // buff
        TabUI.buffImage[buffLevel].sprite
            = DictUI.Contents[buffIndex].iconImage.sprite;

        // deBuff
        TabUI.deBuffImage[buffLevel].sprite
            = DictUI.Contents[deBuffIndex].iconImage.sprite;
    }
    public void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.UpdateValue(timerUI.getSixDigitTime(), InGameManager.Instance.killCount);
        timerUI.SaveTimer();
    }

    public void PauseTime(bool IsPause)
	{
		if(IsPause)
		{
            if (IsTab)
            {
                TabUI.Close();
            }
            InGameManager.Instance.Pause(true);
			Time.timeScale = 0f;
		}
		else
		{
            InGameManager.Instance.Pause(false);
            Time.timeScale = DrugManager.Instance.timeValue;
		}
		//Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
