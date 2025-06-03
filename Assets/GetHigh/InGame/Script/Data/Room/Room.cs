using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public enum RoomState // 현재 방의 상태
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
    private bool clearCheck = false; // 현재 클리어한 방인지 체크
    public bool ClearCheck => clearCheck;
    private int curEnemyCnt = 0; // 처치한 적 수
    private int fullEnemyCnt = 0; // 현재 적 수

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

    // 룸 활성화 로직
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

    // 현재 플레이어가 방에 들어온 상태
    private void PlayerRoom(bool check)
    {
        isPlayerRoom = check;
        RoomController.Instance.ChangePlayerRoom(roomIndex);
    }

    // 현재 방에 있는 몬스터들을 활성화 시키는 로직, 추 후 몬스터의 자동 움직임으로 구현을 변경
    public void AgentActive(bool check)
    {
        if (!clearCheck && check && agents.Count != 0)
        {
            foreach (AI a in agents)
                a.PlayerRoom();
        }
        else // 추가로직
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

    // 이 부분은 이제 문이 나오면 해당 문에서 수정
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
