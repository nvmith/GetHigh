using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public LockDoor lockDoor;

    public bool isTouch;
    public string[] texts = { "문이 막혀있다...", "The Door is blocked..." };
    Coroutine chatCoroutine;
    public float distance = 0;
    public int languageIndex = 0;
    public bool isChat = false;
    public TextMeshPro chatText;

    void Start()
    {

        distance = Vector2.Distance(InGameManager.Instance.player.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            CheckLanguage();
            Chat();
        }
        else if (isChat)
        {
            distance = Vector2.Distance(InGameManager.Instance.player.transform.position, transform.position);

            if (distance > 2.0f)
            {
                isChat = false;
                chatText.text = "";
                StopCoroutine(chatCoroutine);
            }
        }
    }

    public void Chat()
    {
        if (!isChat) chatCoroutine = StartCoroutine(ChatDoor());
    }

    public void CheckLanguage()
    {
        if (lockDoor.isOpen) return;

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

            chatCoroutine = StartCoroutine(ChatDoor());
        }
    }

    IEnumerator ChatDoor()
    {
        isChat = true;

        for (int i = 0; i < texts[languageIndex].Length; i++) // 처음 말걸 때 텍스트
        {
            chatText.text += texts[languageIndex][i];
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouch = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouch = false;
    }
}
