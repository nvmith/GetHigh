using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public int triggerCount = 0;
    protected Vector2 startPos;
    protected int distanceDamage = 0;
    [SerializeField]
    protected int bulletPower;

    protected float rotateSpeed = 3.0f;
    //protected Vector2 moveDir;

    private bool sizeCheck = false;

    [SerializeField] // 외부에서 볼 수 있도록
    private ParticleSystem bombEffect;
    
    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        if (!sizeCheck && DrugManager.Instance.isBulletSizeUp) {
            bulletSize[0] *= 1.5f;
            bulletSize[1] *= 1.5f;
            gameObject.transform.localScale = new Vector3(bulletSize[0], bulletSize[1], bulletSize[2]);
            sizeCheck = true;
        }

        if (InGameManager.Instance.player.IsReverse)
        {
            transform.localScale = new Vector3(-1f * bulletSize[0], bulletSize[1], bulletSize[2]);
        }

        if (DrugManager.Instance.isExecution) DetectAgent();
        startPos = transform.localPosition;
    } // 만약 유도탄일때는 맞추기 전까지 안사라진다고 하면, 로직 수정(유도탄에서 적 찾을 시 추격 값 무한으로 올리기)

    protected override void FixedUpdate()
    {
        // 유도탄 구현중 제대로 적용이 안됨
       /* if (target != null && DrugManager.Instance.isBulletChase)
        {
            // 목표의 방향 계산
            Vector2 direction = (Vector2)target.transform.position - rigid.position;
            direction.Normalize();

            // 회전 방향 계산
            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            // 유도탄 회전
            rigid.angularVelocity = -rotateAmount * rotateSpeed;

            // 유도탄 이동
            rigid.velocity = transform.up * 3.0f;
        }*/
        if(DrugManager.Instance.green3)
        {
            rigid.MovePosition(rigid.position + moveDir * Time.fixedUnscaledDeltaTime);
        }
        else rigid.MovePosition(rigid.position + moveDir * Time.fixedDeltaTime);
    }

    private void DetectAgent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, 7.0f);
        float nearTarget = 999;

        //Debug.Log("실행은 하니1?");
        foreach (Collider2D c in colliders)
        {
                //Debug.Log("실행은 하니2?");
            if (c.gameObject.CompareTag("Agent"))
            {
                //Debug.Log("실행은 하니3?");
                float value = Vector2.Distance(c.gameObject.transform.position, transform.position);
                if (value < nearTarget)
                {
                    nearTarget = value;
                    target = c.gameObject;
                }
            }
        }
    }

    private void DistancePower()
    {
        float distance = Vector3.Distance((Vector2)transform.localPosition, startPos);

        if (distance < 5) distanceDamage = 0;
        else if (distance < 10) distanceDamage = 5;
        else if (distance < 15) distanceDamage = 10;
        else distanceDamage = 20;

        Debug.Log("start : " + startPos + " / cur : " + transform.position);
        Debug.Log("거리 : " + distance + " / 데미지 : " + distanceDamage);
    }

    private void BombBullet() // 처음 맞은 적은 데미지 안들어 오게 할거면 따로 변수로 체크
    {
        bombEffect = PoolManager.Instance.GetFireEffect(transform.rotation);
        bombEffect.transform.position = transform.position;
        bombEffect.transform.localScale *= 0.8f;

        RoomController.Instance.BombLogic(transform.position, bombEffect);
    }

    protected override void TrrigerLogic()
    {
        base.TrrigerLogic();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Agent" || collision.tag == "Boss")
        {
            if (DrugManager.Instance.isDistanceDamage) DistancePower();
            if (DrugManager.Instance.isBomb) BombBullet();
            InGameManager.Instance.bulletPower = this.bulletPower + distanceDamage;

            triggerCount++;
            if (DrugManager.Instance.isBulletPass && triggerCount < 5) return;

            TrrigerLogic();
        }
        else if (collision.tag =="Door") TrrigerLogic();
    }
}
