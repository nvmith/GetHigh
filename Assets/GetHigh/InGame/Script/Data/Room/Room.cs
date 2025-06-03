using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public enum RoomState // ���� ���� ����
{
    being,
    notBeen,
    been
}

public class Room : MonoBehaviour
{
    public BGM roomBGM;
    protected RoomState roomState;
    public RoomState RoomState => roomState;


    // Fog
    [SerializeField]
    private SpriteRenderer fog;

    [SerializeField]
    private GameObject roomIcon;

    // Agents
    [SerializeField]
    private List<AI> agents;
    public List<AI> Agents => agents;
    private bool isPlayerRoom = false;

    // Room Index
    [SerializeField]
    private int roomIndex;
    public int RoomIndex => roomIndex;

    [SerializeField]
    private Door[] doors;

    // Room Status
    private bool clearCheck = false; // ���� Ŭ������ ������ üũ
    public bool ClearCheck => clearCheck;
    private int curEnemyCnt = 0; // óġ�� �� ��
    private int fullEnemyCnt = 0; // ���� �� ��

    private BoxCollider2D boxCol;

    private void Awake()
    {
        InitRoom();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void InitRoom()
    {
        //roomIcon = transform.GetChild(transform.childCount - 1).gameObject;

        fullEnemyCnt = agents.Count;
        RoomStateUpdate(RoomState.notBeen);
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.size -= new Vector2(1f, 1f);
    }

    public void RoomStateUpdate(RoomState state)
    {
        switch (state)
        {
            case RoomState.notBeen:
                roomIcon.SetActive(false);
                fog.color = new Color(0, 0, 0, 1);
                break;
            case RoomState.being:
                roomIcon.SetActive(true);
                fog.color = new Color(0, 0, 0, 0);
                break;
            case RoomState.been:
                fog.color = new Color(0, 0, 0, 0.7f);
                break;
        }
        roomState = state;
    }

    // �� Ȱ��ȭ ����
    public void ActiveRoom()
    {
        PlayerRoom(true);
        AgentActive(isPlayerRoom);
        ActiveDoor();
    }

    public void FirstRoom()
    {
        ActiveRoom();
        clearCheck = true;
        DisableDoor();
    }

    // ���� �÷��̾ �濡 ���� ����
    private void PlayerRoom(bool check)
    {
        isPlayerRoom = check;
        RoomController.Instance.ChangePlayerRoom(roomIndex);
    }

    // ���� �濡 �ִ� ���͵��� Ȱ��ȭ ��Ű�� ����, �� �� ������ �ڵ� ���������� ������ ����
    public void AgentActive(bool check)
    {
        if (!clearCheck && check && agents.Count != 0)
        {
            foreach (AI a in agents)
                a.PlayerRoom();
        }
        else // �߰�����
        {
            ClearCheckRoom();
        }
    }

    public void RoomAgent()
    {
        CameraController.Instance.UpdateAgent(agents);
    }

    public void ClearCheckRoom()
    {
        curEnemyCnt++;

        if (curEnemyCnt >= fullEnemyCnt)
        {
            if(!clearCheck) GameManager.Instance.clearRoomCount++;
            clearCheck = true;
            DisableDoor();
            roomBGM = BGM.NoneBattle;
            BGMChange();
        }
    }

    // �� �κ��� ���� ���� ������ �ش� ������ ����
    public void ActiveDoor()
    {
        if (!clearCheck)
        {
            foreach (Door door in doors) door.DoorLock();
        }
        foreach (Door door in doors) door.nextClearCheck();
	}

    private void DisableDoor()
    {
        if (clearCheck)
        {
            foreach (Door door in doors) door.DoorUnlock();

        }
    }
    public void UpdateDoor()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].QMOff();
            doors[i].nextQMOn();
        }
    }

    public void BGMChange()
    {
        if (SoundManager.Instance.bgm == roomBGM) return;

        SoundManager.Instance.PlayBGM(roomBGM);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ActiveRoom();
            BGMChange();
        }
    }
}
