using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum TableArrow
{
    up, down, left, right, none
}

public class Table : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private SpriteRenderer[] lineObj;

    bool playerCheck = false;
    Vector3 moveVec;
    Rigidbody2D rigid;
    bool enemyCheck = false;
    public bool tableActive = false;
    
    TableArrow curArrow;
    TableArrow agentArrow; // agent�� ��� �� �ִ� ����
    int lineIndex = -1;
    private bool isMove = false;
    private Vector2 mVec; // ������ ����

    private Vector3[] distance = new Vector3[4];

    public Agent curAgent = null;

    [SerializeField]
    private BoxCollider2D triggerCol;

    int playerLine = -1;
    int agentLine = -1;
    float playerVec = 0;
    float agentVec = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        curArrow = TableArrow.none;
    }

    // Update is called once per frame
    void Update()
    {
        CurPos(); 
    }

    private void FixedUpdate()
    {
        if (isMove) rigid.MovePosition(rigid.position + mVec * Time.fixedDeltaTime * 1.5f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�÷��̾� ����");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!tableActive)
        {
            if (curArrow == TableArrow.none && collision.gameObject.tag.Equals("Player"))
            {
                playerCheck = true;
            }
        }
        else
        {
            if (!enemyCheck && curArrow != TableArrow.none && collision.gameObject.tag.Equals("Agent") && curAgent == null)
            {
                enemyCheck = true;
                curAgent = collision.gameObject.GetComponent<Agent>();
                LeanAgent(curAgent.gameObject.transform.position);

            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!tableActive)
        {
            if (curArrow == TableArrow.none && collision.gameObject.tag.Equals("Player"))
            {
                playerCheck = false;
                ActiveLine(false);
            }
        }
        else
        {
            if (enemyCheck && curArrow != TableArrow.none && collision.gameObject.tag.Equals("Agent") && curAgent != null)
            {
                enemyCheck = false;
                curAgent = null;
                //Debug.Log("���̺� Ż��");
            }
        }
    }

    // ��ġ ���(1ȸ�� ����)
    private void CurPos()
    {
        if (tableActive || !playerCheck) return;


        moveVec = InGameManager.Instance.player.transform.position - transform.position;

        float tableAngle = VectorValue(moveVec);

        lineIndex = AngleCalculate(tableAngle);
        if(lineIndex != -1) ActiveLine(true);


        if (!Input.GetKeyDown(KeyCode.E)) return;

        switch ((TableArrow)lineIndex)
        {
            case TableArrow.up:
                agentArrow = TableArrow.down;
                mVec = Vector2.down;
                break;

            case TableArrow.down:
                agentArrow = TableArrow.up;
                mVec = Vector2.up;
                break;

            case TableArrow.left:
                agentArrow = TableArrow.right;
                mVec = Vector2.right;
                break;

            case TableArrow.right:
                agentArrow = TableArrow.left;
                mVec = Vector2.left;
                break;
        }


        curArrow = (TableArrow)lineIndex;

        anim.SetTrigger(agentArrow.ToString());
        SoundManager.Instance.PlaySFX(SFX.Table_Kick);

        tableActive = true;
        lineObj[lineIndex].enabled = false;
        StartCoroutine(MoveTable());
        ActiveLine(false);
        gameObject.tag = "Table";
        triggerCol.size = new Vector2(1, 2);
    }

    private IEnumerator MoveTable()
    {
        isMove = true;
        yield return new WaitForSeconds(0.5f);
        isMove = false;
    }

    // ���� ���
    private float VectorValue(Vector3 value)
    {
        float angle;
        angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // ������ 180 ~ -180

        return angle;
    }

    // ���� ����
    private int AngleCalculate(float angleValue)
    {
        int Index = -1;
        // 1��и�, �� �� �밢���� �켱����
        if (angleValue <= 135f && angleValue > 45f)
        {
            Index = 0; // up
            distance[Index] = transform.position + new Vector3(0, 1, 0);
        }
        // 2��и�, ���� �� �밢���� �켱����
        else if (angleValue <= 45f && angleValue > -45f)
        {
            Index = 3; // right
            distance[Index] = transform.position + new Vector3(1, 0, 0);
        }
        // 3��и�, ���� �Ʒ��밢���� �켱
        else if (angleValue <= -45f && angleValue > -135f)
        {
            Index = 1; // down 
            distance[Index] = transform.position + new Vector3(0, -1, 0);
        }
        // 4��и�, ���� �Ʒ��밢���� �켱
        else if (angleValue <= -135f || angleValue > 135f)
        {
            Index = 2; // left
            distance[Index] = transform.position + new Vector3(-1, 0, 0);
        }

        return Index;
    }

    // ���� ���̺� ���� ǥ�� �뵵
    private void ActiveLine(bool activeTrue)
    {
        if (tableActive) return;

        foreach (SpriteRenderer g in lineObj)
            g.enabled = false;

        if (activeTrue) lineObj[lineIndex].enabled = true;
    }

    // AI ���̺� ���� ���� ���
    //private void LeanAgent(Vector3 vec, GameObject agentObj)
    public void LeanAgent(Vector3 vec)
    {
        moveVec = InGameManager.Instance.player.transform.position;

        playerLine = -1;
        agentLine = -1;

        playerVec = VectorValue(moveVec - transform.position);
        agentVec = VectorValue(vec - transform.position);
        //Debug.Log("agentVec : " + agentVec);

        playerLine = AngleCalculate(playerVec);
        agentLine = AngleCalculate(agentVec);

        // Debug.Log("��� üũ");
        // Debug.Log("playerLine : " + playerLine);
        // Debug.Log("agentLine : " + agentLine);

        //Debug.Log("agentArrow : " + agentArrow);
        //Debug.Log("curagentArrow : " + (TableArrow)agentLine);

        if (StateCheck())
        {
            curAgent = null;
            enemyCheck = false;

            return;
        }
        //Debug.Log("���� üũ");

        curAgent.TableValue(distance[(int)agentArrow], agentArrow, this);

        // ���� �ڵ�
        //agentObj.GetComponent<Agent>().TableValue(distance[(int)agentArrow] ,agentArrow);
    }

    public bool StateCheck()
    {
        return (agentArrow != (TableArrow)agentLine);
        //playerLine == agentLine�� ������
    }
}
