using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateValue
{
    horizontal,
    vertical
}

public class GatePath : MonoBehaviour
{
    [SerializeField]
    private GateValue value;

    bool isActive; // 몬스터를 다 잡기 전 까진 못나감

    BoxCollider2D boxCol2D;

    private void Awake()
    {
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenDoor();
        }
    }

    public void Gateway(Vector3 vec)
    {
        
    }


    // 색 변경 추가하기
    public void OpenDoor()
    {
        boxCol2D.enabled = false;
    }

    public void CloseDoor()
    {

        boxCol2D.enabled = true;
    }
}
