using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Doctor : MonoBehaviour
{
    public Room room;

    bool isDoctor;
    bool touchDoctor;
    bool firstCheck;
    bool completeCure;
    bool chatPrevent = false;
    [SerializeField]
    bool dieDoctor = false;

    string[] firstDoctorText = { "���� �� �� �غ���?", "You wanna try?"};
    string[] maxGuageText = {"�� ������ ����...", "You're hopeless."};
    string[] acceptText = { "�����, �׷��� ���ŵȴ�.", "Quit it, jerk."};
    string[] refuseText = { "�� ����? �׷� ����", "Flat broke? Then get the hell out."};


    public int languageIndex = 0;
    /*string englishFirstDoctorText = "You wanna try";
    string englishMaxGuageText = "You're hopeless.";
    string englishAcceptText = "Quit it, jerk.";
    string englishRefuseText = "Flat broke? Then get the hell out."*/


    /*
     string firstDoctorText = "���� �� �� �غ���?"; You wanna try?
string maxGuageText = "�� ������ ����..."; You're hopeless.
string acceptText = "�����, �׷��� ���ŵȴ�."; Quit it, jerk.
string refuseText = "�� ����? �׷� ����"; Flat broke? Then get the hell out. 
     * */
    public TextMeshPro chatText;
    public float doctorDistance = 3;
    Coroutine chatCoroutine;

    void Update()
    {
        if (dieDoctor) return;

        if (!completeCure)
        {
            CheckNeedDoctor();
            InteractDoctor();
            CheckLanguage();

            if (Vector2.Distance(InGameManager.Instance.player.transform.position, this.transform.position) > doctorDistance)
            {

                if (!completeCure && chatCoroutine != null)
                {
                    StopCoroutine(chatCoroutine);
                    chatCoroutine = null;
                    chatPrevent = false;
                }
                else
                {
                    chatText.text = "";
                    firstCheck = false;
                }
            }
        }
        DieDoctor();
    }

    void CheckNeedDoctor()
    {
        if (completeCure) return;
        
        isDoctor = Input.GetKeyDown(KeyCode.E);
    }

    public void InteractDoctor()
    {
        if (!completeCure && isDoctor && touchDoctor && !chatPrevent)
        {
            chatCoroutine = StartCoroutine(ChatDoctor());
        }
    }

    public void CheckLanguage()
    {
        if (GameManager.Instance.languageIndex == languageIndex) return;
        else
        {
            languageIndex = GameManager.Instance.languageIndex;

            if (chatCoroutine != null)
            {
                StopCoroutine(chatCoroutine);
            }

            chatText.text = "";
            chatCoroutine = null;
            chatPrevent = false;

            chatCoroutine = StartCoroutine(ChatDoctor());
        }
    }

    public void RemoveGuage()
    {
        InGameManager.Instance.DecreaseDrug();
        completeCure = true;
    }

    IEnumerator ChatDoctor()
    {
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Unlicensed_Doctor);

        if (InGameManager.Instance.drugGauge == 100) //�ű��� ���� �� �ؽ�Ʈ
        {
            chatPrevent = true;
            chatText.text = "";
            for (int i = 0; i < maxGuageText[languageIndex].Length; i++)
            {
                chatText.text += maxGuageText[languageIndex][i];
                yield return new WaitForSeconds(0.1f);
            }
            chatPrevent = false;
        }

        else
        {
            chatPrevent = true;
            if (!firstCheck)
            {
                for (int i = 0; i < firstDoctorText[languageIndex].Length; i++) // ó�� ���� �� �ؽ�Ʈ
                {
                    chatText.text += firstDoctorText[languageIndex][i];
                    yield return new WaitForSeconds(0.1f);
                }
                firstCheck = true;
            }
            else
            {
                chatText.text = "";
                if (InGameManager.Instance.money < 10)
                {
                    for (int i = 0; i < refuseText[languageIndex].Length; i++) // �� ���� �� �ؽ�Ʈ
                    {
                        chatText.text += refuseText[languageIndex][i];
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    chatText.text = "";
                    RemoveGuage();
                    for (int i = 0; i < acceptText[languageIndex].Length; i++) // ġ�� �޾��� �� �ؽ�Ʈ
                    {
                        chatText.text += acceptText[languageIndex][i];
                        yield return new WaitForSeconds(0.1f);
                    }

                    yield return new WaitForSeconds(3.0f);
                    chatText.text = "";
                }

            }
            chatPrevent = false;
        }

    }

    public void DieDoctor()
    {
        if (completeCure && room.RoomState != RoomState.being)
        {
            gameObject.SetActive(false);
            dieDoctor = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchDoctor = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchDoctor = false;
        }
    }
}
