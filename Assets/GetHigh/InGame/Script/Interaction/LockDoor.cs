using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockDoor : MonoBehaviour
{
    public Door door;
    public Door backDoor;
    public LockBackDoor lockBackDoor;
    public Room room;

    public bool isTouch;
    public bool doorCheck = false; // ���������� �Ѿ �������� üũ
    private BoxCollider2D boxcol;
    public AI[] agent;
    public GameObject closeDoor; // ���� ������ ��α� ���

    public string[] texts = { "���谡 �ʿ��ϴ�...", "Need a Key..." };
    Coroutine chatCoroutine;
    public float distance = 0;
    public int languageIndex = 0;
    public bool isOpen = false;
    public bool isChat = false;
    public TextMeshPro chatText;

    private void Awake()
    {
        boxcol = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        door.Doorcol(false);

        distance = Vector2.Distance(InGameManager.Instance.player.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (isTouch)
            {
                CheckLanguage();
                Chat();
                Interact();
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

        if(!doorCheck)
        {
            StopAgent();
        }
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InGameManager.Instance.key >= 1)
            {
                StopCoroutine(chatCoroutine);
                chatText.text = "";

                InGameManager.Instance.UpdateKey(-1);
                SoundManager.Instance.PlaySFX(SFX.UseKey);
                door.Doorcol(true);
                backDoor.Doorcol(true);
                lockBackDoor.DisCol();
                door.CheckDoor();
                boxcol.enabled = false;
                closeDoor.SetActive(false);
                isOpen = true;
            }
        }
    }

    public void StopAgent()
    {
        if (door.boxCol.isTrigger) return;

        Debug.Log("���� ��?");
        doorCheck = true;
        
        Debug.Log("���� �� ���� ���� ��: " + (room.gameObject.transform.position.x - InGameManager.Instance.player.transform.position.x));
        if (room.gameObject.transform.position.x > InGameManager.Instance.player.transform.position.x) return;

        foreach(Agent a in agent)
        {
            a.DisPlayerRoom();
        }
    }

    public void Chat()
    {
        if (!isChat) chatCoroutine = StartCoroutine(ChatDoor());
    }

    public void CheckLanguage()
    {
        if (isOpen) return;

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

        for (int i = 0; i < texts[languageIndex].Length; i++) // ó�� ���� �� �ؽ�Ʈ
        {
            chatText.text += texts[languageIndex][i];
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   isTouch = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouch = false;
    }
}
