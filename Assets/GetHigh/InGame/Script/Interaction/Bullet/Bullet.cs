using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EUsers
{
    Player, Enemy
}
public enum EBullets
{
    Rifle, Shotgun, Sniper, Revolver
}

// 분류하기
public abstract class Bullet : MonoBehaviour
{
    public float eraseSpeed;
    protected Rigidbody2D rigid;
    [SerializeField]
    protected EUsers eUsers;
    [SerializeField]
    protected EBullets eBullets;
    [SerializeField] // 유도탄 확인용
    protected GameObject target;

    [SerializeField]
    protected float[] bulletSize;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletSize = new float[] { transform.localScale.x, transform.localScale.y, 1 };
    }
    protected virtual void OnEnable()
    {
        if (eraseSpeed > 0)
        {
            rigid.velocity = Vector2.zero;

            StartCoroutine(Erase());
            transform.localScale = new Vector3(bulletSize[0], bulletSize[1], bulletSize[2]);
        }
        
    }

    // 추가 1
    protected Vector2 moveDir;//여기 해야함

    protected virtual void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDir * Time.fixedDeltaTime);
    }

    public void MoveBullet(Vector2 dir)
    {
        moveDir = dir;
        //rigid.AddForce(dir, ForceMode2D.Impulse);
    }

    protected virtual IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed + DrugManager.Instance.playerAttackRange);
        TrrigerLogic();
    }

    protected virtual void TrrigerLogic()
    {
        target = null;
        PoolManager.Instance.ReturnBullet(this, eUsers, eBullets);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.tag == "Wall" || collision.tag == "Table")
        {
            TrrigerLogic();
        }

    }
}
