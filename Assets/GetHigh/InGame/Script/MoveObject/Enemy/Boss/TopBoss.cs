using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBoss : Boss
{
    //nt cnt = 0;
    protected override void Awake()
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
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void DropItem()
    {
        base.DropItem();
        PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0, 3)).ThrowItem(transform.position);
        PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0, 3)).ThrowItem(transform.position);
        PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0, 3)).ThrowItem(transform.position);
        DropWeapon(4);
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.FullMoon);
    }
    protected override IEnumerator IAttack()
    {
        //Debug.Log("공격 실행");
        AttackLogic();
        //yield return new WaitForSeconds(attackDelay);

        switch (selectPivot)
        {
            case 1:
                BP1();
                yield return new WaitForSeconds(1);
                break;
            case 2:
                yield return StartCoroutine("BP2");
                break;
            case 3:
                BP3();
                yield return new WaitForSeconds(1);
                break;
            case 4:
                yield return StartCoroutine("BP4");
                break;
            case 5:
                yield return StartCoroutine("BP5");
                break;
  /*          
    case 6:
        BP6();
        break;
*/
            case 7:
                BP7();
                yield return new WaitForSeconds(1);
                break;
        
            case 8:
                yield return StartCoroutine("BP8");
                break;

            case 10:
                yield return StartCoroutine("BP10");
                break;

            case 11:
                yield return StartCoroutine("BP11");
                break;

            case 12:
                yield return StartCoroutine("BP12");
                break;
        }

        curAttackDelay = 0;
        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
        bossAttack = false;
    }

    protected override void SelectBP()
    {
        //int[] numbers = { 5 };
        int[] numbers = { 1, 2, 3, 4, 5, 7, 8, 10, 11, 12 }; // 6, 7야함
        //selectPivot = Random.Range(1, 14);
        selectPivot = numbers[Random.Range(0, numbers.Length)];
        Debug.Log("보스 스킬 사용");
        muzzle.localRotation = Quaternion.Euler(0, 0, -90);
    }
}