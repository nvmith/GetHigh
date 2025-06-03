using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBackDoor : MonoBehaviour
{
    public Door door;
    public Room room;

    public bool check = false; // 1회용 활성화 로직

    private BoxCollider2D boxcol;

    public bool doorCheck = false; // 다음방으로 넘어간 상태인지 체크

    public AI[] agent;


    private void Start()
    {
        boxcol = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!doorCheck)
        {
            StopAgent();
        }
    }
    public void StopAgent()
    {
        if (door.boxCol.isTrigger) return;

        doorCheck = true;
        if (room.gameObject.transform.position.x < InGameManager.Instance.player.transform.position.x) return;

        foreach (Agent a in agent)
        {
            a.DisPlayerRoom();
        }
    }

    public void DisCol()
    {
        boxcol.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (check) return;

        if(room.roomBGM == BGM.NoneBattle && collision.gameObject.CompareTag("Player"))
        {
            check = true;
            DisCol();
        }
    }
}
