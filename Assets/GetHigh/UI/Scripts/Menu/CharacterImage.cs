using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImage : MonoBehaviour
{
    private Image characterImage;
    [SerializeField]
    private Sprite[] characterSprites;
    private int curIndex = 0;
    public int CurIndex => curIndex;

    private int maxIndex = 99;
    //[SerializeField]
    //private Image[] buttons;
    [SerializeField]
    private Text[] buttonText;


    [SerializeField]
    private Sprite[] sprites;

    private bool[] lockCheck;

    private void Awake()
    {
        characterImage = GetComponent<Image>();
        Init();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        curIndex = 0;
        maxIndex = characterSprites.Length - 1;
        characterImage.sprite = characterSprites[0];
        buttonText[0].enabled = false;
    }

    public void UpdateIndex(int num)
    {
        /*
        if (num < 0)
        {
            if (curIndex > 0) curIndex--;
        }
        else
        {
            if (curIndex < maxIndex) curIndex++;
        }

        if (curIndex == 0) buttons[0].sprite = sprites[1];
        else buttons[0].sprite = sprites[0];

        if (curIndex == maxIndex) buttons[1].sprite = sprites[1];
        else buttons[1].sprite = sprites[0];

        // 메인 캐릭터 이미지 갱신
        if(!DataManager.Instacne.JsonClass._PlayerData.playerLock[curIndex]) characterImage.sprite = characterSprites[maxIndex+1];
        else characterImage.sprite = characterSprites[curIndex];*/
 
        if (num < 0)
        {
            if (curIndex > 0) curIndex--;
            else return;
        }
        else
        {
            if (curIndex < maxIndex) curIndex++;
            else return;
        }

        SoundManager.Instance.PlaySFX(SFX.MouseClick);

        if(curIndex == 2) characterImage.rectTransform.sizeDelta = new Vector2(600, 600);
        else characterImage.rectTransform.sizeDelta = new Vector2(500, 500);


        if (curIndex == 0) buttonText[0].enabled = false;
        else buttonText[0].enabled = true;

        if (curIndex == maxIndex) buttonText[1].enabled = false;
        else buttonText[1].enabled = true;

        characterImage.sprite = characterSprites[curIndex];

        // 메인 캐릭터 이미지 갱신
        if (!DataManager.Instacne.JsonClass._PlayerData.playerLock[curIndex])
        {
            characterImage.color = new Color(0, 0, 0, 1);
        }
        else
        {
            characterImage.color = new Color(1, 1, 1, 1);
        }
    }

    public void SelectButton()
    {
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[curIndex])
        {
            DataManager.Instacne.DefaultData.SettingValue(curIndex);
            GameManager.Instance.playerCheck = true;
            GameManager.Instance.selectCharacter = (ECharacters)curIndex;
        }
    }//생략해도 될듯

    // 임시용 현재 UI화면 lock 풀기
    public void UnLockButton()
    {
        DataManager.Instacne.UpdateLock(curIndex);
        characterImage.sprite = characterSprites[curIndex];
        characterImage.color = new Color(1, 1, 1, 1);
    }
}
