using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInputManager : MonoBehaviour
{
    public DictionaryUI Dictionary;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dictionary.gameObject.activeSelf) return;
        if ((Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)) && !Dictionary.diaryOpenCheck)
        {
            Dictionary.Close();
        }
    }

    public void SoundOn()
    {
        SoundManager.Instance.PlaySFX(SFX.MouseClick);
    }
}
