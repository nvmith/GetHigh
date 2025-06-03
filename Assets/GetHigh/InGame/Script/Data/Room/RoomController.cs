using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;
    public List<Room> Rooms => rooms;


    private static RoomController instance;
    public static RoomController Instance => instance;

    [SerializeField]
    private int curIndex = 0; // 현재 룸 인덱스
    public int CurIndex => curIndex;


    private void Awake()
    {
        Init();
    }

    void Start()
    {
        rooms[0].FirstRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        instance = this;
    }

    // 미니맵 수정
    public void ChangePlayerRoom(int num)
    {
        rooms[curIndex].RoomStateUpdate(RoomState.been);
        curIndex = num;
        rooms[curIndex].RoomStateUpdate(RoomState.being);
        rooms[curIndex].UpdateDoor();
    }

    public void ClearRoomCount() // 적을 처지할 때 마다 방이 클리어 상태인지 확인
    {
        rooms[curIndex].ClearCheckRoom();
    }

    public Room CurRoom()
    {
        return Rooms[CurIndex];
    }
 

    public void BombLogic(Vector3 pos, ParticleSystem p) // getcomponent를 안 쓰고 적에게 폭발 데미지를 못줘서 여기다 구현
    {
        foreach (AI a in rooms[curIndex].Agents)
        {
            if (Vector3.Distance(pos, a.gameObject.transform.position) < 3f)
            {
                a.Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Knife);
                Debug.Log("폭발탄 : " + a.gameObject.name);
            }
        }

        StartCoroutine(StopEffect(p));
    }

    private IEnumerator StopEffect(ParticleSystem p)
    {
        yield return new WaitForSeconds(1.5f);
        p.transform.localScale = new Vector3(1.6f, 1.6f, 1);
        PoolManager.Instance.ReturnFireEffect(p);
    }
    /*
    private void CheckEnemy(int num)
    {
        rooms[num]?.AgentActive(false);

     }
    */


}
