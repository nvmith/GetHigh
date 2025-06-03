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
    // 추후 gameManager의 player를 받아올것
    [SerializeField]
    protected Transform target;

    // Component
    protected NavMeshAgent agent;
    protected Rigidbody2D rigid;
    protected CircleCollider2D cirCollider2D;
    protected SpriteRenderer spritesRenderer;
    protected Animator anim;

    // 추후 애니메이션으로 변경

    [SerializeField]
    protected Transform muzzle;


    // 현재 AI 활성화 상태인지 고려, 랜덤생성 or 다음 칸 배치 같은 문제에서 적용함
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
    protected EnemyVetor curVec = EnemyVetor.Front; // 추가1

    protected bool isDie = false;
    [SerializeField]
    protected bool isDetect = false;
    protected bool isMoveLean = false;
    protected bool isLean = false;
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    // AI Attack
    [SerializeField]
    protected float attackDelay; // 공격 딜레이
    [SerializeField]
    protected float curAttackDelay; // 현재 공격 딜레이 시간
    [SerializeField]
    protected float attackDistance; // 공격 사거리

    // Object Interaction
    protected bool tableMove;
    protected TableArrow curTableArrow;

    // Agent Vector;
    protected float agentAngleValue;
    protected int agentAngleIndex;
    protected bool isReverse = false;

    protected Vector3 moveVec; // lean상태에서 움직일 방향
    protected Vector3 tableVec; // table 위치
    protected Vector3 playerPosVec;

    // 열쇠방 전용 AI 체크
    public bool keyRoom = false; // 활성화 될 시 AI 움직임 제어
    // AI 임시 삭제용
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
        // anim = GetComponent<Animator>(); AI 그림 나오면 적용할거임
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
        // AI 삭제 임시용
        if (TestAgent) Destroy(this.gameObject);
    }

    // 떨림 방지
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

    // 플레이어의 방향 계산
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        AngleCalculate(agentAngleValue);
    }

    protected float AgentVector()
    {
        playerPosVec = InGameManager.Instance.player.transform.position - transform.position;
        // 현재 자기 자신을 기점으로 플레이어의 위치를 계산하여 어느 방향을 바라봐야하는지 보여줌
        float angle;
        angle = Mathf.Atan2(playerPosVec.y, playerPosVec.x) * Mathf.Rad2Deg; // 범위가 180 ~ -180

        return angle;
    }

    protected virtual void AngleCalculate(float angleValue)
    {
        // 후면(윗 방향)
        if (angleValue < 120 && angleValue > 60)
            curVec = EnemyVetor.Back;
        // 오른 대각
        else if (angleValue <= 60 && angleValue >= 10)
            curVec = EnemyVetor.Cross;
        // 오른
        else if (angleValue < 10 && angleValue >= -60)
            curVec = EnemyVetor.Side;
        // 정면(아랫 방향)
        else if (angleValue < -60 && angleValue > -120)
            curVec = EnemyVetor.Front;
        // 왼쪽(오른쪽에서 뒤집기)
        else if (angleValue <= -120 || angleValue > 170)
            curVec = EnemyVetor.Side;
        // 왼쪽 대각(오른쪽에서 뒤집기)
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

    // 플레이어가 들어온지 탐지
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
        //Debug.Log("추격 실행");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            //Debug.Log("추격 종료");
        }
        agent.SetDestination(target.position);
    }

    protected virtual void Attack()
    {
        if (IsAttack || HitCheck) return;

        //Debug.Log("코루틴 시작 1");
        isAttack = true;
        isDetect = false;
        agent.isStopped = true;
        curAttackDelay = 0;

        StartCoroutine(IAttack());
    }

    protected virtual IEnumerator IAttack()
    {
        //Debug.Log("공격 실행");
        AttackLogic();
        yield return new WaitForSeconds(attackDelay);


        //Debug.Log("공격 끝");

        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
    }

    protected abstract void AttackLogic();

    // 연호가 해당 프레임까지 그려주기 힘들면 다른 방면 생각
    // 그려준다면 방향별로 고개만 빙글빙글 3프레임정도
    protected void Stun()
    {

    }

    public void Damage(int damage, WeaponValue value)
    {
        if (isDie) return;

        if(DrugManager.Instance.aimMissCheck)
        {
            int a = Random.Range(0, 100);
            Debug.Log("값 : " + a);
            if(a < 15)
            {
                Debug.Log("조준 미스");
                return;
            }
        }

        StartCoroutine(EDamage());

        if (value == WeaponValue.Gun)
        {
            damage += InGameManager.Instance.bulletPower;

            Debug.Log("총알 포함 데미지 : " + damage);
        }
        else
        {
            damage += InGameManager.Instance.player.initMeleePower;
        }

        if (DrugManager.Instance.red2)
        {
            damage = damage + damage * DrugManager.Instance.powerUpValue / 100;

            Debug.Log("광전사 포함 데미지 : " + damage);
        }
        else if (DrugManager.Instance.isBleeding && damage >= maxHp * 0.3f)
        {
            StartCoroutine(Bleed());
        }

        if (DrugManager.Instance.isAnger)
        {
            // 데미지를 주고 카운팅, 첫 타격일 땐 분노가 없으니 0으로 카운팅 되어 계산
            damage += (int)(damage * DrugManager.Instance.angerPower * 0.4f);
            DrugManager.Instance.AngryCount();

            Debug.Log("분노 포함 데미지 : " + damage);
        }

        if (DrugManager.Instance.islucianPassive) damage = damage * 7 / 10;

        hp -= damage;
        Debug.Log("들어온 데미지 : " + damage);
        Debug.Log("몬스터 남은 체력: " + hp);

        if(DrugManager.Instance.isExecution)
        {
            if(maxHp * 0.2 >= hp) 
            {
                hp = 0;
                Debug.Log("처형 발동 : " + maxHp * 0.2);
                // 처형 발동
            }
        }

        if (hp <= 0)
        {
            RoomController.Instance.ClearRoomCount();
            Die();
        }
    }

    // 맞았을 때 깜빡임
    public virtual IEnumerator EDamage()
    {
        int i = 0;
        HitCheck = true;
        for (i = 0; i < 3; i++)  // 추후 실루엣 스프라이트로 변경
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
            Debug.Log("출혈 중, 몬스터 남은 체력: " + hp);
            yield return new WaitForSeconds(1f);

            if (hp <= 0)
            {
                RoomController.Instance.ClearRoomCount();
                Destroy(gameObject);
                break;
            }
        }
    }


    // 나중에 죽었을때 기능 구현
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
        Debug.Log("ActiveItem : " + value);// 붕대, 열쇠, 방탄, 수류탄 존재

        // ActiveItem (0~4:붕대 / 5~9:열쇠 / 10~14:방탄 / 15~19 : 수류탄)
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
        // 1. Bullet 테그별로 나누기(총알이 다 다를경우)
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
