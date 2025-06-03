using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance => instance;

    [SerializeField]
    //private Player player; // ���� �÷��̾� ���� ����
    private Vector3 targetPos, refVel = Vector3.zero;
    private Camera cam;
    public Camera Cam => cam;


    [SerializeField]
    private float zOffset; // ī�޶� �⺻ ������
    public float ZOffset => zOffset;
    private Vector3 mousePoint;
    [SerializeField]
    private float camDist = 2.0f;
    [SerializeField]
    private float smoothTime = 0.2f;
    private bool activeCam = true; // ī�޶� ������ �� �ִ��� ����

    // Status
    PlayerVetor playerVecStatus;

    // Mouse Pointer
    private Vector2 pointer;
    public Vector2 Pointer => pointer;

    // Anagle
    private float playerAngle;
    public float PlayerAngle => playerAngle;

    private Vector2 mouseVecValue;
    public Vector2 MouseVecValue => mouseVecValue;

    bool isReverse = false;

    // ���� �濡 �ִ� agents
    private List<AI> agents;
    public List<AI> Agents => agents;


    private void Awake()
    {
        Init();
        cam = GetComponent<Camera>();

    }

    void Start()
    {
        targetPos = InGameManager.Instance.player.transform.position;
        zOffset = transform.position.z;
        agents = new List<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeCam || !InGameManager.Instance.IsPause)
        {
            mousePoint = CheckMousePointer();
            targetPos = UpdateTargetPos();
            UpdateCamPos();
            PlayerPos();
        }
    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
       //else Destroy(this.gameObject);
    }

    private Vector3 CheckMousePointer()
    {
        // ���콺 ����Ʈ ã��
        Vector2 refVec = cam.ScreenToViewportPoint(Input.mousePosition) * 2;
        refVec *= 2;
        refVec -= Vector2.one;
        float max = 0.9f;

        // �ִ� ���� ���� 1�� ����
        if (Mathf.Abs(refVec.x) > max || Mathf.Abs(refVec.y) > max)
            refVec = refVec.normalized;

        return refVec;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mousePos = mousePoint * camDist;
        Vector3 refVec = InGameManager.Instance.player.transform.position + mousePos;
        refVec.z = zOffset;
        return refVec;
    }

    private void UpdateCamPos()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, targetPos,
                                    ref refVel, smoothTime);
        transform.position = tempPos;
    }

    // Player ���� ����
    private void PlayerPos()
    {
        pointer = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseVecValue = pointer - (Vector2)InGameManager.Instance.player.transform.localPosition;

        playerAngle = Mathf.Atan2(mouseVecValue.y, mouseVecValue.x) * Mathf.Rad2Deg;

        AngleCalculate(playerAngle);
    }

    // ���� ����
    private void AngleCalculate(float angleValue)
    {
        // �ĸ�(�� ����)
        if (angleValue < 120 && angleValue > 60)
            playerVecStatus = PlayerVetor.Back;
        // ���� �밢
        else if (angleValue <= 60 && angleValue >= 10)
            playerVecStatus = PlayerVetor.Cross;
        // ����
        else if (angleValue < 10 && angleValue >= -60)
            playerVecStatus = PlayerVetor.Side;
        // ����(�Ʒ� ����)
        else if (angleValue < -60 && angleValue > -120)
            playerVecStatus = PlayerVetor.Front;
        // ����(�����ʿ��� ������)
        else if (angleValue <= -120 || angleValue > 170)
            playerVecStatus = PlayerVetor.Side;
        // ���� �밢(�����ʿ��� ������)
        else if (angleValue <= 170 || angleValue >= 120)
            playerVecStatus = PlayerVetor.Cross;

        if (!isReverse) // �� ������ ����
        {
            if (angleValue >= 105 || angleValue <= -105) isReverse = true;
        }
        else
        {
            if (angleValue >= -75 && angleValue <= 75) isReverse = false;
        }

        //Debug.Log(angleValue);

        InGameManager.Instance.player.ChangeVector(playerVecStatus, isReverse);
    }

    public void CameraActive(bool value)
    {
        activeCam = value;
    }

    public void CameraPos(float x, float y)
    {
        transform.position = new Vector3(x, y, ZOffset);
    }

    public void UpdateAgent(List<AI> a)
    {
        //agents = new List<Agent>(); //����Ʈ �ʱ�ȭ, list.Capacity
        agents.Clear();
        
        foreach(AI value in a)
        {
            if (value != null) agents.Add(value);
        }

        
        //Debug.Log("���� ����Ʈ ����� �� : " + agents.Capacity);
    }
}
