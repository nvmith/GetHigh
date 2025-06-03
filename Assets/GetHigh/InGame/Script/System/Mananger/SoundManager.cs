using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum VolumeType
{
    masterVol, bgmVol, sfxVol
}

public enum BGM
{
    Main, NoneBattle, Battle, Boss, GameOver
}

public enum SFX
{
    Walk, Roll, Hit, AK_Reload, M870_Reload, AWP_Reload, Eagle_Reload, Anaconda_Reload,
    Knife_Shot, Punch_Shot, AK_Shot, M870_Shot, AWP_Shot, Eagle_Shot, Anaconda_Shot, 
    SoldOut, DrugPickUp, ItemPickUp, MoneyPickUp, UseBand, UseBFS, UseDrug, UseGaugeLock, UseGrenade, UseKey, UseMoney,
    Box_Open, Door_Open, Table_Kick, DiaryOpen, DiaryClose, HeartBeat, MouseClick
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    public BGM bgm;

    public AudioMixer mixer;

    [Header("Sounds")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    [Header("AudioSource")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource reloadSource;
    public AudioSource putSource;

    public bool[] toggleCheck;

    private void Awake()
    {
        Init();
        bgmSource.clip = bgmClips[(int)bgm];
        putSource.clip = sfxClips[(int)SFX.Walk];
    }

    private void Init()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //reloadSource.pitch = 1;
        SceneManager.sceneLoaded += SceneLoadFunc;
    }

    private void SceneLoadFunc(Scene scene, LoadSceneMode mode)
    {
        reloadSource.pitch = 1;
    }

    public float GetVolume(VolumeType volType)
    {
        float volume;
        mixer.GetFloat(volType.ToString(), out volume);

        return volume;
    }

    public void SetVolume(Slider slider, VolumeType volType)
    {
        mixer.SetFloat(volType.ToString(), slider.value);

        if (slider.value == slider.minValue)
        {
            mixer.SetFloat(volType.ToString(), -80);
        }
    }

    public bool GetToggle(int toggleNum)
    {
        return toggleCheck[toggleNum];
    }

    public void SetToggle(Toggle[] toggles)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggleCheck[i] = toggles[i].isOn;
        }
        PlaySFX(SFX.MouseClick);
    }

    public void PlaySFX(SFX sfx)
    {
        if (!toggleCheck[2]) return;
        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }

    public void PlayBGM(BGM bgm)
    {
        if(this.bgm != bgm)
        {
            this.bgm = bgm;
            bgmSource.clip = bgmClips[(int)bgm];
        }
        
        if (toggleCheck[1] == false) return;

        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void PlayReload(int index)
    {
        reloadSource.clip = sfxClips[index];
        reloadSource.Play();
    }

    public void StopReload()
    {
        reloadSource.Stop();
    }

    public void PlayPutSound()
    {
        putSource.Play();
    }

    public void StopPutSound()
    {
        putSource.Stop();
    }
}
