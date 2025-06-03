using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour
{
	public Button FirstButton;

    private void OnEnable()
    {
        UIManager.Instance.TempUI = this.gameObject;
        UIManager.Instance.IsPopup++;
        FirstButton.Select();
    }

    private void OnDisable()
    {
        UIManager.Instance.pauseUI.SelectButtonOn();
        UIManager.Instance.IsPopup--;
    }
}
