using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuStart : MonoBehaviour
{
    public GameObject signUI;

    public TextMeshProUGUI clearPlayTimeText;
    public TextMeshProUGUI clearDrugCountText;
    public TextMeshProUGUI clearEnemyCountText;
    public TextMeshProUGUI clearRoomCountText;
    public TextMeshProUGUI clearMoneyText;

    void Start()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(BGM.Main);
        }

        if(GameManager.Instance.clearCheck)
        {
            signUI.gameObject.SetActive(true);

            clearPlayTimeText.text = "" + GameManager.Instance.clearPlayTime;
            clearDrugCountText.text = "" + GameManager.Instance.clearDrugCount;
            clearEnemyCountText.text = "" + GameManager.Instance.clearEnemyCount;
            clearRoomCountText.text = "" + GameManager.Instance.clearRoomCount;
            clearMoneyText.text = "" + GameManager.Instance.clearMoney;
        }
    }

}
