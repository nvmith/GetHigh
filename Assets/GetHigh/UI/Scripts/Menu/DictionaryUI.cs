using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;

public class DictionaryUI : MonoBehaviour
{
    private Animator animator;

    /*[SerializeField]
    private Sprite[] icons;*/
    [SerializeField]
    private Vector2[] spriteSize;
    [SerializeField]
    private Sprite[] characterIllrust;

    public LocalizeStringEvent localName;

    public LocalizeStringEvent localDesc;
    [SerializeField]
    private Image image;
    [SerializeField]
    private DictionaryContents[] contents;
    public DictionaryContents[] Contents => contents;

    [SerializeField]
    private Scrollbar scrollbar;

    public float[] scrollvalue;

    public bool diaryOpenCheck;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(SoundManager.Instance != null) SoundManager.Instance.PlaySFX(SFX.DiaryOpen);
    }

    private void Start()
    {
        GameManager.Instance.dictionaryUI = this;

        for (int i = 1; i < contents.Length; i++)
        {
            contents[i].isUnLock = GameManager.Instance.DiaryDataCheck[i - 1];
            contents[i].LockUpdate();
        }

        int j = 1;
        if (!contents[j].isUnLock) j = 0;

        localName.StringReference.TableEntryReference = contents[j].value.ToString();
        localDesc.StringReference.TableEntryReference = contents[j].value.ToString();
        UpdateImage(1); // 빨간 마약으로 일단 띄우기
        //image.sprite = contents[j+1].iconImage.sprite;
        //image.color = contents[j + 1].iconImage.color;
        //image.sprite = ilusts[j];
    }

    public void ShowInformation()
    {
        int i = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        int j = i;
        SoundManager.Instance.PlaySFX(SFX.MouseClick);

        if (!contents[i].isUnLock)
        {
            j = 0;
        }

        localName.StringReference.TableEntryReference = contents[j].value.ToString();
        localDesc.StringReference.TableEntryReference = contents[j].value.ToString();

        UpdateImage(i);
        //image.sprite = ilusts[i];
        //image.sprite = contents[j+1].iconImage.sprite;
        //image.color = contents[j + 1].iconImage.color;

        //Debug.Log("아이템 번호: " + i);
    }

    public void UpdateImage(int i)
    {
        // 캐릭터 일러스트 인덱스는 수정이 필요함

        image.rectTransform.sizeDelta = spriteSize[i-1];

        if(i >= 22 && i<= 30)
        {
            image.sprite = characterIllrust[i-22];
        }
        else
        {
            image.sprite = contents[i].iconImage.sprite;
        }
        image.color = contents[i].iconImage.color;
    }

    public void UpdateContent(int i) // 단일 업데이트
    {
        Debug.Log("단일 업데이트 실행");

        contents[i].UnLockContent();

    }


    public void MoveBookMark(int contentType)
    {
        scrollbar.value = scrollvalue[contentType];
        SoundManager.Instance.PlaySFX(SFX.MouseClick);
    }

    public void Close()
    {
        SoundManager.Instance.PlaySFX(SFX.DiaryClose);
        animator.SetBool("Open", false);
    }
    public void DictOff()
    {
        if (UIManager.Instance != null) UIManager.Instance.IsDict = false;
        gameObject.SetActive(false);
    }

    public IEnumerator CloseLogic()
    {
        diaryOpenCheck = true;
        Close();
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        diaryOpenCheck = false;
    }
}