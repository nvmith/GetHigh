using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.timerUI.SaveTimer();

            GameManager.Instance.UpdateClearCount();
            GameManager.Instance.SaveData();
            Time.timeScale = 1f;

            GameManager.Instance.clearPlayTime = UIManager.Instance.timerUI.getSixDigitTime();
            GameManager.Instance.clearEnemyCount = InGameManager.Instance.killCount;
            GameManager.Instance.clearMoney = InGameManager.Instance.money;
            GameManager.Instance.clearCheck = true;

            SceneManager.LoadScene(0);
        }
    }
}
