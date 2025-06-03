using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyStatus
{
    Idle,
    Chase,
    Attack,
    Stun,
    Lean,
    Die
}

public enum EnemyVetor
{
    Front, Cross, Side, Back
}

public abstract class AI : MonoBehaviour
{
    // ���� gameManager�� player�� �޾ƿð�
    [SerializeField]
    protected Transform target;

    // Component
    protected NavMeshAgent agent;
    protected Rigidbody2D rigid;
    protected CircleCollider2D cirCollider2D;
    protected SpriteRenderer spritesRenderer;
    protected Animator anim;

    // ���� �ִϸ��̼����� ����

    [SerializeField]
    protected Transform muzzle;


    // ���� AI Ȱ��ȭ �������� ���, �������� or ���� ĭ ��ġ ���� �������� ������
    protected bool activeRoom = false;
    public bool ActiveRoom => activeRoom;

    // AI Stats
    [SerializeField]
    protected int hp = 100;
    [SerializeField]
    protected int maxHp = 100;

    // AI State
    [SerializeField]
    protected EnemyStatus curStatus;
    public EnemyStatus CurStatus => curStatus;
    protected EnemyVetor curVec = EnemyVetor.Front; // �߰�1

    protected bool isDie = false;
    [SerializeField]
    protected bool isDetect = false;
    protected bool isMoveLean = false;
    protected bool isLean = false;
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    // AI Attack
    [SerializeField]
    protected float attackDelay; // ���� ������
    [SerializeField]
    protected float curAttackDelay; // ���� ���� ������ �ð�
    [SerializeField]
    protected float attackDistance; // ���� ��Ÿ�

    // Object Interaction
    protected bool tableMove;
    protected TableArrow curTableArrow;

    // Agent Vector;
    protected float agentAngleValue;
    protected int agentAngleIndex;
    protected bool isReverse = false;

    protected Vector3 moveVec; // lean���¿��� ������ ����
    protected Vector3 tableVec; // table ��ġ
    protected Vector3 playerPosVec;

    // ����� ���� AI üũ
    public bool keyRoom = false; // Ȱ��ȭ �� �� AI ������ ����
    // AI �ӽ� ������
    public bool TestAgent = false;
    protected bool HitCheck;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rigid = GetComponent<Rigidbody2D>();
        curStatus = EnemyStatus.Idle;
        cirCollider2D = GetComponent<CircleCollider2D>();
        spritesRenderer = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>(); AI �׸� ������ �����Ұ���
    }

    protected virtual void Start()
    {
        target = InGameManager.Instance.player.transform;
        hp = maxHp;
    }

    protected virtual void Update()
    {
        if (!InGameManager.Instance.IsPause)
        {
            //if (isDetect) UpdateState(curStatus);
            UpdateState(curStatus);
        }
        // AI ���� �ӽÿ�
        if (TestAgent) Destroy(this.gameObject);
    }

    // ���� ����
    protected virtual void FixedUpdate()
    {
        if (!InGameManager.Instance.IsPause)
        {
            rigid.velocity = Vector2.zero;
        }
    }

    protected virtual void UpdateState(EnemyStatus enemy)
    {
        
    }

    // �÷��̾��� ���� ���
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        AngleCalculate(agentAngleValue);
    }

    protected float AgentVector()
    {
        playerPosVec = InGameManager.Instance.player.transform.position - transform.position;
        // ���� �ڱ� �ڽ��� �������� �÷��̾��� ��ġ�� ����Ͽ� ��� ������ �ٶ�����ϴ��� ������
        float angle;
        angle = Mathf.Atan2(playerPosVec.y, playerPosVec.x) * Mathf.Rad2Deg; // ������ 180 ~ -180

        return angle;
    }

    protected virtual void AngleCalculate(float angleValue)
    {
        // �ĸ�(�� ����)
        if (angleValue < 120 && angleValue > 60)
            curVec = EnemyVetor.Back;
        // ���� �밢
        else if (angleValue <= 60 && angleValue >= 10)
            curVec = EnemyVetor.Cross;
        // ����
        else if (angleValue < 10 && angleValue >= -60)
            curVec = EnemyVetor.Side;
        // ����(�Ʒ� ����)
        else if (angleValue < -60 && angleValue > -120)
            curVec = EnemyVetor.Front;
        // ����(�����ʿ��� ������)
        else if (angleValue <= -120 || angleValue > 170)
            curVec = EnemyVetor.Side;
        // ���� �밢(�����ʿ��� ������)
        else if (angleValue <= 170 || angleValue >= 120)
            curVec = EnemyVetor.Cross;

        if (angleValue <= -90 || angleValue > 90) isReverse = true;
        else isReverse = false;

        //return index;

        if (isDetect && curStatus != EnemyStatus.Lean)
        {
            for (int i = 0; i < 4; i++)
            {
                EnemyVetor a = (EnemyVetor)i;
                anim.SetBool(a.ToString(), false);
            }

            anim.SetBool(curVec.ToString(), true);
        }
    }

    protected void Idle()
    {
        if (isDetect)
        {
            curStatus = EnemyStatus.Chase;
            return;
        }
    }

    // �÷��̾ ������ Ž��
    public void PlayerRoom()
    {
        isDetect = true;
    }

    public void DisPlayerRoom()
    {
        isDetect = false;
    }    

    protected void Chase()
    {
        //if (!isDetect) return;

        if (!isDetect)
        {
            curStatus = EnemyStatus.Idle;
            return;
        }
        //Debug.Log("�߰� ����");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            //Debug.Log("�߰� ����");
        }
        agent.SetDestination(target.position);
    }

    protected virtual void Attack()
    {
        if (IsAttack || HitCheck) return;

        //Debug.Log("�ڷ�ƾ ���� 1");
        isAttack = true;
        isDetect = false;
        agent.isStopped = true;
        curAttackDelay = 0;

        StartCoroutine(IAttack());
    }

    protected virtual IEnumerator IAttack()
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

    protected abstract void AttackLogic();

    // ��ȣ�� �ش� �����ӱ��� �׷��ֱ� ����� �ٸ� ��� ����
    // �׷��شٸ� ���⺰�� ���� ���ۺ��� 3����������
    protected void Stun()
    {

    }

    public void Damage(int damage, WeaponValue value)
    {
        if (isDie) return;

        if(DrugManager.Instance.aimMissCheck)
        {
            int a = Random.Range(0, 100);
            Debug.Log("�� : " + a);
            if(a < 15)
            {
                Debug.Log("���� �̽�");
                return;
            }
        }

        StartCoroutine(EDamage());

        if (value == WeaponValue.Gun)
        {
            damage += InGameManager.Instance.bulletPower;

            Debug.Log("�Ѿ� ���� ������ : " + damage);
        }
        else
        {
            damage += InGameManager.Instance.player.initMeleePower;
        }

        if (DrugManager.Instance.red2)
        {
            damage = damage + damage * DrugManager.Instance.powerUpValue / 100;

            Debug.Log("������ ���� ������ : " + damage);
        }
        else if (DrugManager.Instance.isBleeding && damage >= maxHp * 0.3f)
        {
            StartCoroutine(Bleed());
        }

        if (DrugManager.Instance.isAnger)
        {
            // �������� �ְ� ī����, ù Ÿ���� �� �г밡 ������ 0���� ī���� �Ǿ� ���
            damage += (int)(damage * DrugManager.Instance.angerPower * 0.4f);
            DrugManager.Instance.AngryCount();

            Debug.Log("�г� ���� ������ : " + damage);
        }

        if (DrugManager.Instance.islucianPassive) damage = damage * 7 / 10;

        hp -= damage;
        Debug.Log("���� ������ : " + damage);
        Debug.Log("���� ���� ü��: " + hp);

        if(DrugManager.Instance.isExecution)
        {
            if(maxHp * 0.2 >= hp) 
            {
                hp = 0;
                Debug.Log("ó�� �ߵ� : " + maxHp * 0.2);
                // ó�� �ߵ�
            }
        }

        if (hp <= 0)
        {
            RoomController.Instance.ClearRoomCount();
            Die();
        }
    }

    // �¾��� �� ������
    public virtual IEnumerator EDamage()
    {
        int i = 0;
        HitCheck = true;
        for (i = 0; i < 3; i++)  // ���� �Ƿ翧 ��������Ʈ�� ����
        {
            spritesRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spritesRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        HitCheck = false;
        yield return null;
    }

    public IEnumerator Bleed()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 5; i++)
        {
            hp -= maxHp / 100;
            Debug.Log("���� ��, ���� ���� ü��: " + hp);
            yield return new WaitForSeconds(1f);

            if (hp <= 0)
            {
                RoomController.Instance.ClearRoomCount();
                Destroy(gameObject);
                break;
            }
        }
    }


    // ���߿� �׾����� ��� ����
    protected virtual void Die()
    {
        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;

        DropItem();
        gameObject.SetActive(false);
        InGameManager.Instance.UpdateKillCount();
        //GameManager.Instance.UpdateKill();
    }

    protected virtual void DropItem()
    {
        int value = Random.Range(1, 101);

        Debug.Log("Money : " + value);
        // Money
        if (value <= 10) DropMoney(3);
        else if (value <= 60) DropMoney(2);
        else if (value <= 90) DropMoney(1);


        value = Random.Range(1, 101);
        Debug.Log("Drug : " + value);
        // Drug (0~9:red / 10~19:orange / 20~29:yellow / 30~39: green / 40~49:blue)
        if (value <= 50)
        {
            Drug drug = PoolManager.Instance.GetDrug((EDrugColor)((value - 1)/10));
            drug.ThrowItem(transform.position);
        }

        value = Random.Range(1, 101);
        Debug.Log("Magazine : " + value);
        // Magazine
        if (value <=15)
        {
            Magazine magazine = PoolManager.Instance.GetMagazine(0);
            magazine.ThrowItem(transform.position);
        }
        else if(value <= 30)
        {
            Magazine magazine = PoolManager.Instance.GetMagazine(1);
            magazine.ThrowItem(transform.position);
        }

        value = Random.Range(0, 100);
        Debug.Log("ActiveItem : " + value);// �ش�, ����, ��ź, ����ź ����

        // ActiveItem (0~4:�ش� / 5~9:���� / 10~14:��ź / 15~19 : ����ź)
        if (value <= 19)
        {
            if (!RoomController.Instance.Rooms[3].ClearCheck)
            {
                bool returnItem = true;
                while (returnItem)
                {
                    if (value >= 5 && value <= 9)
                    {
                        value = Random.Range(0, 20);
                    }
                    else returnItem = false;
                }
            }

            Item activeItem = PoolManager.Instance.GetActiveItem((EActiveItems)(value / 5));
            activeItem.ThrowItem(transform.position);
        }
    }

    public void DropMoney(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            Item money = PoolManager.Instance.GetMoney();
            money.ThrowItem(transform.position);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bullet �ױ׺��� ������(�Ѿ��� �� �ٸ����)
        if (collision.CompareTag("PlayerBullet"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Gun);
        }
        else if (collision.CompareTag("Knife"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Knife);
        }
    }
}
