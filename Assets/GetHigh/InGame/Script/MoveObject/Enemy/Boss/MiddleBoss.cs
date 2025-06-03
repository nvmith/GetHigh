using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Boss
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

        int value = Random.Range(1, 11);
        Debug.Log("�� : " + value);

        PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0, 3)).ThrowItem(transform.position);

        if (value > 5) PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0, 3)).ThrowItem(transform.position);
        if(value == 10) PoolManager.Instance.GetActiveItem((EActiveItems)Random.Range(0,3)).ThrowItem(transform.position);
        DropWeapon(3);
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Deimos);
        InGameManager.Instance.stage1Door.SetActive(false);
    }

    protected override IEnumerator IAttack()
    {
        //Debug.Log("���� ����");
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
            case 5:
                yield return StartCoroutine("BP5");
                break;
            /*
    case 6:
        BP6();
        break;

    case 7:
        BP7();
        break;
        */
            case 7:
                BP7();
                yield return new WaitForSeconds(1);
                break;
            case 9:
                yield return StartCoroutine("BP9");
                break;
        }
        //Debug.Log("���� ��");

        curAttackDelay = 0;
        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
        bossAttack = false;
    }

    protected override void SelectBP()
    {
        //int[] numbers = { 1 };
        int[] numbers = { 1, 2, 3, 5, 7, 9 }; // 7�ؾ���
        selectPivot = numbers[Random.Range(0, numbers.Length)];
        Debug.Log("���� ��ų ���");
        muzzle.localRotation = Quaternion.Euler(0, 0, -90);
    }
}