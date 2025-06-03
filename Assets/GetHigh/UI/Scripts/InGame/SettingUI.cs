using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider[] sliders; //0: ALL, 1: BGM, 2: SFX
    [SerializeField]
    private Toggle[] toggles;

    private bool isUpdating = false;

    public bool uiManagerCheck = false;


    public Image mousePointer;
    /*
    [SerializeField]
    private Sprite[] mousePointerImages;
    */


    private void OnDisable()
    {
        if (uiManagerCheck) Close();
    }

    private void Start()
    {
        Init();

        if (UIManager.Instance != null) uiManagerCheck = true;
        else uiManagerCheck = false;

        ChangePointer(DataManager.Instacne.MouseIndex);
    }

    public void SoundOn()
    {
        SoundManager.Instance.PlaySFX(SFX.MouseClick);
    }

    public void Open()
    {
        //Debug.Log("üũ");
        //if (uiManagerCheck)
        //{
            UIManager.Instance.TempUI = this.gameObject;
            UIManager.Instance.IsPopup++;
        //}
    }

    public void Close()
    {
        if (uiManagerCheck)
        {
            UIManager.Instance.pauseUI.SelectButtonOn();
            UIManager.Instance.IsPopup--;
        }
    }

    private void Init()
    {
        isUpdating = true;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = SoundManager.Instance.GetVolume((VolumeType)i);
            toggles[i].isOn = SoundManager.Instance.GetToggle(i);
        }
        isUpdating = false;
    }

    public void SliderUpdate(int volNum)
    {
        SoundManager.Instance.SetVolume(sliders[volNum], (VolumeType)volNum);
    }

    public void ToggleUpdate(int volNum)
    {
        if (isUpdating)
            return;

        isUpdating = true;

        switch (volNum)
        {
            case 0:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.sfxSource.mute = false;
                    SoundManager.Instance.reloadSource.mute = false;
                    SoundManager.Instance.putSource.mute = false;
                    if (!SoundManager.Instance.bgmSource.isPlaying)
                        SoundManager.Instance.bgmSource.Play();
                    toggles[1].isOn = true;
                    toggles[2].isOn = true;
                }
                else
                {
                    SoundManager.Instance.sfxSource.mute = true;
                    SoundManager.Instance.reloadSource.mute = true;
                    SoundManager.Instance.putSource.mute = true;
                    SoundManager.Instance.bgmSource.Pause();
                    toggles[1].isOn = false;
                    toggles[2].isOn = false;
                }
                break;
            case 1:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.bgmSource.Play();
                    if (toggles[2].isOn)
                    {
                        toggles[0].isOn = true;
                    }
                }
                else
                {
                    SoundManager.Instance.bgmSource.Pause();
                    toggles[0].isOn = false;
                }
                break;
            case 2:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.sfxSource.mute = false;
                    SoundManager.Instance.reloadSource.mute = false;
                    SoundManager.Instance.putSource.mute = false;
                    if (toggles[1].isOn)
                    {
                        toggles[0].isOn = true;
                    }
                }
                else
                {
                    SoundManager.Instance.sfxSource.mute = true;
                    SoundManager.Instance.reloadSource.mute = true;
                    SoundManager.Instance.putSource.mute = true;
                    toggles[0].isOn = false;
                }
                break;
        }
        SoundManager.Instance.SetToggle(toggles);
        isUpdating = false;
    }

    public void SetMousePointer(int value)
    {
        ChangePointer(DataManager.Instacne.MouseIndex + value);
    }

    public void ChangePointer(int value)
    {
        //Debug.Log("지금 포인터 값 : " + value);

        if (value < 0 || value > DataManager.Instacne.pointerSprites.Length - 1) return;

        SoundOn();
        DataManager.Instacne.UpdatePointer(value);
        mousePointer.sprite = DataManager.Instacne.pointerSprites[value];
    }

    public void SetLanguage(int index)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];

        GameManager.Instance.languageIndex = index; // 나중에 번역을 설정하면 마우스 포인터처럼 저장힐지도 생각

        SoundOn();
    }
    /*
    public enum ELanguage 
    { 
    Korean, English
    }
    public ELanguage languageValue;
    Locale currentSelectedLocale;
     currentSelectedLocale = LocalizationSettings.SelectedLocale;
        languageValue = (ELanguage)LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        Debug.Log("languageIndex : " + languageValue);

    languageValue = (ELanguage)num; 

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[num];
     */
}
