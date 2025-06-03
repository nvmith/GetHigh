using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI killCountText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateValue(string playTime, int killCount)
    {
        playTimeText.text = playTime;
        killCountText.text = killCount+" Έν";
    }
}
