using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.GlobalVariables;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class HistoryUI : MonoBehaviour
{
    public string[] names = { };

    //public TextMeshProUGUI playTimeText;

    private PersistentVariablesSource source;

    public int hour = 0;
    public int minute = 0;

    public int playTime = 0;
    public int killCount = 0;
    public int diaryCount = 0;
    public int characterCount = 0;
    public int deathCount = 0;
    public int clearCount = 0;

    void Start()
    {
        source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();

        playTime = GameManager.Instance.PlayTime;
        killCount = GameManager.Instance.KillCount;
        diaryCount = GameManager.Instance.DiaryCount;
        characterCount = GameManager.Instance.CharacterCount;
        deathCount = GameManager.Instance.DeathCount;
        clearCount = GameManager.Instance.ClearCount;

        ChangeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText()
    {
        //Debug.Log("playTime : " + playTime);

        //source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        //var myFloat = source["global"]["my-float"] as UnityEngine.Localization.SmartFormat.PersistentVariables.FloatVariable;
        hour = playTime / 3600;
        minute = (playTime % 3600) / 60;
        
        IntVariable hours = source["global"]["play-hour"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        hours.Value = hour;

        IntVariable minutes = source["global"]["play-minute"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        minutes.Value = minute;

        IntVariable kill = source["global"]["kill-count"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        kill.Value = killCount;

        IntVariable diary = source["global"]["diary-count"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        diary.Value = diaryCount;

        IntVariable character = source["global"]["character-count"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        character.Value = characterCount;

        IntVariable death = source["global"]["death-count"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        death.Value = deathCount;

        IntVariable clear = source["global"]["clear-count"] as UnityEngine.Localization.SmartFormat.PersistentVariables.IntVariable;
        clear.Value = clearCount;
    }

}
