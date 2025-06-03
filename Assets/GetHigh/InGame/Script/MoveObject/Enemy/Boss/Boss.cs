using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public abstract class Boss : AI
{
    [SerializeField]
    private EnemyGun gun;
    [SerializeField]
    protected Transform Line;

    public SpriteRenderer gunRenderer;
    public Animator bossAnim;

    public float rushPower; //임시
    bool isRush = false;

    //public bool bossKey;
    public int selectPivot = 0;
    public bool knifeThrow = false; // 투척 전까지 플레이어를 쫓도록 추가한 변수
    public bool bossAttack = false; // 보스 공격 중 시선 변경 차단 로직

    public Vector2 playerVec;
    public int moneySize = 0; // 각 보스별 드랍 돈

    public Item[] guns;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Line = muzzle.GetChild(0);
    }

    protected override void Update()
    {
        base.Update();

        //BossInputKey();
        //SelectBP();
        //BossRush();
    }
    /*public void BossInputKey()
    {
        bossKey = Input.GetKeyDown(KeyCode.B);
    }*/

    protected override void Attack()
    {
        if (IsAttack) return;

        //Debug.Log("코루틴 시작 1");
        isAttack = true;
        isDetect = false;
        agent.isStopped = true;
        curAttackDelay = 0;

        StartCoroutine(IAttack());
    }

    public override IEnumerator EDamage()
    {
        yield return base.EDamage();
    }
    protected override void AngleCalculate(float angleValue)
    {
        // 추가 로직 isRush
        if (bossAttack || isRush) return;

        base.AngleCalculate(angleValue);
    }

    protected void BossRush()
    {
        if (!isRush) return;

        //playerVec = (target.position - transform.position).normalized;
        rigid.MovePosition(rigid.position + playerVec * rushPower * Time.fixedDeltaTime);
        //transform.position = transform.position + (dir * rushPower * Time.deltaTime);
    }

    // 떨림 방지
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        BossRush();
    }

    protected override void UpdateState(EnemyStatus enemy)
    {
        if (isDie) return;

        AgentAngle();

        if (isReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        /*if (isRush)
        {
            if(isReverse) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }*/

        if(!isAttack) curAttackDelay += Time.deltaTime;

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
            case EnemyStatus.Die:
                Die();
                break;
            default:
                Debug.Log("미구현 기능");
                break;
        }
    }

    protected override IEnumerator IAttack()
    {
        yield return base.IAttack();
        /*
        //Debug.Log("공격 실행");
        AttackLogic();
        //yield return new WaitForSeconds(attackDelay);

        //yield return StartCoroutine()

        //Debug.Log("공격 끝");

        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
        */
    }

    protected override void AttackLogic()
    {
        bossAttack = true;
        SelectBP();
        curAttackDelay = 0;
    }

    protected override void DropItem()
    {
        DropMoney(moneySize);

        int drugValue1 = Random.Range(0, 5);
        int drugValue2 = drugValue1;
        while(drugValue1 == drugValue2)
        {
            drugValue2 = Random.Range(0, 5);
        }

        for(int i=0; i<5;i++)
        {
            if (i == drugValue1 || i == drugValue2) continue;
            else PoolManager.Instance.GetDrug((EDrugColor)i).ThrowItem(transform.position);
        }

        PoolManager.Instance.GetMagazine(0).ThrowItem(transform.position);
        PoolManager.Instance.GetMagazine(1).ThrowItem(transform.position);

        // 아이템은 각 보스별 단계에서 다시
    }

    protected void DropWeapon(int index)
    {
        int mainCheck = Random.Range(1, 5); // 1,2,3,4
        mainCheck += index; // mainCheck < 5면 보조무기
        Debug.Log("mainCheck : " + mainCheck);

        if(mainCheck < 5) // 보조무기
        {
            index = Random.Range(3, 5);
        }
        else
        {
            index = Random.Range(0, 3);
        }
        
        Weapons weapon = Instantiate(guns[index], transform.position, transform.rotation).GetComponent<Weapons>();
        weapon.ThrowItem(transform.position);
    }


    protected override void Die()
    {
        base.Die();
    }

    protected virtual void SelectBP()
    {

    }

    public void BP1() // 중간보스, 최종보스
    {
        Debug.Log("BP1");

        for (int i = 0; i < 36; i++)
        {
            gun.ShotReady(transform.position, 10 * i, isReverse);

            /*
            Vector2 BP1bulletDir = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + (10 * i));

            GameObject BP1bulletcopy = Instantiate(bullet, muzzle.position, muzzle.rotation);
            //BP1bulletcopy.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;
            */
        }
        curAttackDelay = 2f;
    }
    public IEnumerator BP2() // 좆밥보스, 최종보스
    {
        knifeThrow = true;
        bossAttack = false;
        Line.gameObject.SetActive(true);
        gunRenderer.enabled = false;

        yield return new WaitForSeconds(1);

        Line.localScale = new Vector3(1, 1, 1);
        Line.gameObject.SetActive(false);

        gun.KnifeShotReady(isReverse);
        knifeThrow = false;
        Debug.Log("BP2");
        gunRenderer.enabled = true;

        yield return new WaitForSeconds(1);
        curAttackDelay = 3f;
    }


    public void BP3() // 중간보스 개선 필요 - 총알 발사 식
    {
        for (int angle = -30; angle <= 30; angle += 10)
        {
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * muzzle.up;

            gun.Shot(rotatedDirection, muzzle.position, angle);
            /*Bullet bullet = PoolManager.Instance.GetBullet(EUsers.Enemy, EBullets.Revolver, Quaternion.Euler(0, 0, angle % 360));
            bullet.transform.position = muzzle.position;
            bullet.MoveBullet(rotatedDirection * 8);*/
        }

        Debug.Log("BP3");
        curAttackDelay = 3f;
    }


    public IEnumerator BP4() // 최종보스
    {
        for (int i = 0; i < 60; i++)
        {
            int randomAngle = Random.Range(0, 36);
            gun.ShotReady(transform.position, 10 * randomAngle, isReverse);
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("BP4");
        yield return new WaitForSeconds(1f);
    }


    public IEnumerator BP5() // 중간보스, 최종보스 개선 필요 - 에이전트는 이용안할예정, position 이동시켜서 움직였으나 이펙트가 없어 뭔가 이상, 충돌시 데미지 미구현
    {
        //      EnemyStatus originStatus = CurStatus;
        //      float originSpd = agent.speed;

        //      isRush = true;
        //curStatus = EnemyStatus.Chase;
        //      agent.speed = rushPower;

        //      yield return new WaitForSeconds(0.3f);

        //      isRush = false;
        //      curStatus = originStatus;
        //      agent.speed = originSpd;

        isRush = true;
        //transform.localScale = new Vector3(1, 1, 1);

        bossAnim.SetTrigger("Rush");
        gunRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);

        InGameManager.Instance.bossRushCheck = true;
        playerVec = (target.position - transform.position).normalized;
        //rigid.velocity = Vector3.zero;

        yield return new WaitForSeconds(1f);

        isRush = false;
        InGameManager.Instance.bossRushCheck = false;

        yield return new WaitForSeconds(0.25f);
        Debug.Log("BP5");
        gunRenderer.enabled = true;
        //transform.position = Vector2.MoveTowards(transform.position, target.position, 80f * Time.deltaTime);
        curAttackDelay = 2f;
    }

    /*

    public void BP6() // 최종보스
    {
        //위치 조정 필요할 듯
        Instantiate(load, new Vector2(boss.transform.position.x - 5, boss.transform.position.y), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y + 5), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y - 5), boss.transform.rotation);
        Debug.Log("BP6");
    }
    */

    
    public void BP7() // 중간보스
    {
        //수류탄 받아오기

        for(int i=0; i<3;i++)
        {
            BossGrenade bg = PoolManager.Instance.GetBossGrenade();
            bg.gameObject.transform.position = transform.position;
            bg.gameObject.transform.rotation = transform.rotation;
        }

        Debug.Log("BP7");
        curAttackDelay = 3f;
    }
    

    public IEnumerator BP8() // 최종보스
    {
        //총알들이 일직선이 아닌 동시에 나감

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                gun.ShotReady(transform.position, 60 * j, isReverse);
            }
            yield return new WaitForSeconds(0.07f);
        }
        Debug.Log("BP8");
        yield return new WaitForSeconds(1f);
        curAttackDelay = 2f;
    }

    public IEnumerator BP9() // 좆밥보스
    {
        Debug.Log("BP9");

        for (int i = 0; i < 4; i++)
        {
            for (int angle = -15; angle <= 15; angle += 15)
            {

                Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * muzzle.up;

                gun.Shot(rotatedDirection, muzzle.position, angle);
                //Bullet bullet = PoolManager.Instance.GetBullet(EUsers.Enemy, EBullets.Revolver, Quaternion.Euler(0, 0, angle % 360));
                //bullet.transform.position = muzzle.position;
                //bullet.MoveBullet(rotatedDirection * 8);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        curAttackDelay = 2f;
    }

    public IEnumerator BP10() // 최종보스 개선해야할지도모름, 삼각함수 사용안함.
    {
        Vector3 dirVec = muzzle.up; // 궤적 고정용
        int dirNum = 1;

        for (int i = 0; i <= 2; i++)
        {
            for (int angle = -20; angle <= 20; angle += 4)
            {
                Vector3 rotatedDirection = Quaternion.Euler(0, 0, dirNum * angle) * dirVec;

                gun.Shot(rotatedDirection, muzzle.position, angle * dirNum);
                yield return new WaitForSeconds(0.07f);
            }
            dirNum *= -1;
        }

        //   yield return new WaitForSeconds(0.1f);
        //}
        Debug.Log("BP10");
        yield return new WaitForSeconds(1f);
        curAttackDelay = 1.5f;
    }

    public IEnumerator BP11() // 최종보스
    {
        Vector3 dirVec = muzzle.up;

        for (int i = 0; i < 20; i++)
        {
            int angle = Random.Range(-20, 21);
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * dirVec;

            gun.Shot(rotatedDirection, muzzle.position, angle);
            yield return new WaitForSeconds(0.15f);
        }


        Debug.Log("BP11");
        yield return new WaitForSeconds(1f);
        curAttackDelay = 2f;
    }

    public IEnumerator BP12() // 최종보스 개선 필요 코드 더러움
    {
        Vector3 dirVec = muzzle.up;

        for (int i = 0; i < 36; i++)
        {
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, i * 10) * Vector3.up;

            gun.Shot(rotatedDirection, transform.position, i * 10);
            gun.Shot(-rotatedDirection, transform.position, i * 10);

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("BP12");
        yield return new WaitForSeconds(1f);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        /*if (collision.tag == "Player" && !InGameManager.Instance.player.AvoidCheck
           && !InGameManager.Instance.player.IsHit && isRush)
        {

        }*/
    }
}
