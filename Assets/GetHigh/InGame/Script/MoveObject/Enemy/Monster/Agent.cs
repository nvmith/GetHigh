using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public enum EBasicEnemy
{
    Strike_Team1, Strike_Team2, Strike_Team3
}

public class Agent : AI
{
    public float[] tableValue = { 0, 0 }; // x,y�� �̵� �Ÿ� ��

    public Table table = null;

    public EBasicEnemy enemyValue;

    protected override  void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Test();
    }

    // ���� ����
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void DropItem()
    {
        base.DropItem();
    }

    public override IEnumerator EDamage()
    {
        /*Debug.Log("PlayerVec : " + playerPosVec);

        if (!isLean) // �и��� ����
        {

            //agent.isStopped = false;
            //rigid.AddForce(-playerPosVec.normalized * 5f, ForceMode2D.Impulse);
            if (!isAttack || isDetect)
            {
                agent.isStopped = true;
            }
        }*/
        yield return base.EDamage();

        /*if (!isAttack || isDetect)
        {
            agent.isStopped = false;
        }*/
    }

    public void Test()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //agent.isStopped = false;
            //rigid.AddForce(-playerPosVec.normalized * 5f, ForceMode2D.Impulse);
            //rigid.MovePosition(-playerPosVec.normalized);
        }    
    }

    protected override void AngleCalculate(float angleValue)
    {
        base.AngleCalculate(angleValue);
    }

    protected override void UpdateState(EnemyStatus enemy)
    {
        if (isDie) return;

        AgentAngle();

        if (isReverse) transform.localScale = new Vector3(-1, 1, 1);

        else transform.localScale = new Vector3(1, 1, 1);

        curAttackDelay += Time.deltaTime;

        switch (enemy)
        {
            case EnemyStatus.Idle:
                Idle();
                break;
            case EnemyStatus.Chase:
                Chase();
                break;
            case EnemyStatus.Attack:
                Attack();
                break;
            case EnemyStatus.Lean:
                UpLean();
                break;
            case EnemyStatus.Die:
                Die();
                break;
            default:
                Debug.Log("Not Function");
                break;
        }
    }

    protected override IEnumerator IAttack()
    {
        //Debug.Log("���� ����");
        AttackLogic();
        yield return new WaitForSeconds(attackDelay);


        //Debug.Log("���� ��");

        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
    }

    protected override void AttackLogic()
    {

    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Strike_Team1 + (int)enemyValue);
    }

    public void UpLean() // ���̺� �̵� �� ���ݱ���
    {
        // ����
        if (curTableArrow != TableArrow.none && isLean)
            transform.position = Vector3.MoveTowards(transform.position, tableVec, 1.5f * Time.deltaTime);
        // ����
        else
        {
            LeanAiming();
        }
        
    }

    public void TableValue(Vector3 vec, TableArrow arrow, Table t)
    {
        tableVec = vec;
        curTableArrow = arrow;
        curStatus = EnemyStatus.Lean;
        isLean = true;
        table = t;

        isDetect = false; //�߰���
        agent.isStopped = true;
        StartCoroutine(LeanCount());
    }

    protected IEnumerator LeanCount()
    {
        //Debug.Log("����.");
        anim.SetTrigger("Lean");

        Vector3 playerVec = InGameManager.Instance.player.transform.position - transform.position;

        moveVec = Vector3.zero; // ������ ����

        switch (curTableArrow)
        {
            case TableArrow.up:
                if (playerVec.x <= 0)
                {
                    moveVec = new Vector3(-tableValue[0], 0,1);
                }
                else
                {
                    moveVec = new Vector3(tableValue[0], 0,1);
                }
                break;
            case TableArrow.down:
                if (playerVec.x <= 0)
                {
                    moveVec = new Vector3(-tableValue[0], 0,1);
                }
                else
                {
                    moveVec = new Vector3(tableValue[0], 0,1);
                }
                break;
            case TableArrow.left:
                if (playerVec.y <= 0)
                {
                    moveVec = new Vector3(0, -tableValue[1],1);
                }
                else
                {
                    moveVec = new Vector3(0, tableValue[1],1);
                }
                break;
            case TableArrow.right:
                if (playerVec.y <= 0)
                {
                    moveVec = new Vector3(0, -tableValue[1],1);
                }
                else
                {
                    moveVec = new Vector3(0, tableValue[1],1);
                }
                break;
        }

        yield return new WaitForSeconds(1.0f);
        isLean = false;

        yield return new WaitForSeconds(0.5f); // ���ر��� �ɾ�� �ð�

        /*if(table.curAgent == null)
        {
            isDetect = true;
            agent.isStopped = false;
            curStatus = EnemyStatus.Chase;
            table = null;
        }
        else
        {
            table.LeanAgent(transform.position);
        }*/
        isDetect = true;
        agent.isStopped = false;
        curStatus = EnemyStatus.Chase;
        table = null;
    }
    protected void LeanAiming()
    {
        transform.position = Vector3.MoveTowards(transform.position, (transform.position + moveVec), 5.0f * Time.unscaledDeltaTime);
        //Debug.Log("������ ���� moveVec" + moveVec);
        //Debug.Log("��� ���� :" + (transform.position + moveVec));
        //Debug.Log("���� ��ǥ :" + transform.position);
    }
}
