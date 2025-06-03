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

    public float rushPower; //�ӽ�
    bool isRush = false;

    //public bool bossKey;
    public int selectPivot = 0;
    public bool knifeThrow = false; // ��ô ������ �÷��̾ �ѵ��� �߰��� ����
    public bool bossAttack = false; // ���� ���� �� �ü� ���� ���� ����

    public Vector2 playerVec;
    public int moneySize = 0; // �� ������ ��� ��

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

        //Debug.Log("�ڷ�ƾ ���� 1");
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
        // �߰� ���� isRush
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

    // ���� ����
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
                Debug.Log("�̱��� ���");
                break;
        }
    }

    protected override IEnumerator IAttack()
    {
        yield return base.IAttack();
        /*
        //Debug.Log("���� ����");
        AttackLogic();
        //yield return new WaitForSeconds(attackDelay);

        //yield return StartCoroutine()

        //Debug.Log("���� ��");

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

        // �������� �� ������ �ܰ迡�� �ٽ�
    }

    protected void DropWeapon(int index)
    {
        int mainCheck = Random.Range(1, 5); // 1,2,3,4
        mainCheck += index; // mainCheck < 5�� ��������
        Debug.Log("mainCheck : " + mainCheck);

        if(mainCheck < 5) // ��������
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

    public void BP1() // �߰�����, ��������
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
    public IEnumerator BP2() // ���亸��, ��������
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


    public void BP3() // �߰����� ���� �ʿ� - �Ѿ� �߻� ��
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


    public IEnumerator BP4() // ��������
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


    public IEnumerator BP5() // �߰�����, �������� ���� �ʿ� - ������Ʈ�� �̿���ҿ���, position �̵����Ѽ� ���������� ����Ʈ�� ���� ���� �̻�, �浹�� ������ �̱���
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

    public void BP6() // ��������
    {
        //��ġ ���� �ʿ��� ��
        Instantiate(load, new Vector2(boss.transform.position.x - 5, boss.transform.position.y), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y + 5), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y - 5), boss.transform.rotation);
        Debug.Log("BP6");
    }
    */

    
    public void BP7() // �߰�����
    {
        //����ź �޾ƿ���

        for(int i=0; i<3;i++)
        {
            BossGrenade bg = PoolManager.Instance.GetBossGrenade();
            bg.gameObject.transform.position = transform.position;
            bg.gameObject.transform.rotation = transform.rotation;
        }

        Debug.Log("BP7");
        curAttackDelay = 3f;
    }
    

    public IEnumerator BP8() // ��������
    {
        //�Ѿ˵��� �������� �ƴ� ���ÿ� ����

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

    public IEnumerator BP9() // ���亸��
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

    public IEnumerator BP10() // �������� �����ؾ���������, �ﰢ�Լ� ������.
    {
        Vector3 dirVec = muzzle.up; // ���� ������
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

    public IEnumerator BP11() // ��������
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

    public IEnumerator BP12() // �������� ���� �ʿ� �ڵ� ������
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
