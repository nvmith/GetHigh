using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
	public float Sec = 0f;
	private int Min = 0;

    private TextMeshProUGUI timeText;

	private void Awake()
	{
		timeText = GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update()
    {
		Timer();

	}

    void Timer()
    {
		Sec += Time.deltaTime;

		timeText.text = string.Format("{0:D2}:{1:D2}", Min, (int)Sec);

        if ((int)Sec > 59)
        {
            Sec = 0;
            Min++;
        }
	}
    public string getSixDigitTime()
    {
        int calculatedHour = Min / 60;
        int remainingMin = Min % 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", calculatedHour, remainingMin, (int)Sec);
    }

    public void SaveTimer()
    {
        GameManager.Instance.UpdateTime(Min * 60 + (int)Sec);
    }
}
