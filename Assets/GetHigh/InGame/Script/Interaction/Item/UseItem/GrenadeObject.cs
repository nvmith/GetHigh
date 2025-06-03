using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;

    public float distance = 2.5f;
    public int damage = 100;

    public bool boomCheck = false; // 폭발 수정용

    //public Vector2 moveVec;
    private Vector2 lastVec;

    public CircleCollider2D cirCol;

    //public ParticleSystem particle;
    public ParticleSystem particleObject;

    public SpriteRenderer render;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        cirCol = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        render.enabled = true;
        cirCol.isTrigger = false;
        rigid.constraints = RigidbodyConstraints2D.None;

        transform.position = InGameManager.Instance.player.transform.position;
        transform.rotation = InGameManager.Instance.player.transform.rotation;

        if (DrugManager.Instance.green3)
            rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f * 1.25f, ForceMode2D.Impulse);
        else
            rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f, ForceMode2D.Impulse);

        StartCoroutine(Explode());
    }

    // Update is called once per frame
    private void Update()
    {
        lastVec = rigid.velocity;
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        render.enabled = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        //particle.Play();

        if (DrugManager.Instance.bombMissCheck)
        {
            int bombMissCheck = Random.Range(1, 11);
            Debug.Log("값 : " + bombMissCheck);
            if (bombMissCheck == 1)
            {
                Debug.Log("수류탄 불발");
                boomCheck = true;
                PoolManager.Instance.ReturnGrenadeObject(this);
                yield return null;
            }
        }

        if (!boomCheck)
        {
            particleObject = PoolManager.Instance.GetFireEffect(transform.rotation);
            particleObject.transform.position = transform.position;
            cirCol.isTrigger = true;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, distance);

            foreach (Collider2D c in colliders)
            {
                c.GetComponent<AI>()?.Damage(damage, WeaponValue.Knife);
            }
            SoundManager.Instance.PlaySFX(SFX.UseGrenade);
            yield return new WaitForSeconds(2f);
            //particle.Stop();
            PoolManager.Instance.ReturnFireEffect(particleObject);

            PoolManager.Instance.ReturnGrenadeObject(this);
        }

        boomCheck = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") // 벽에 부딪히면 반대로 가면서 힘을 반으로 감소
        {
            Debug.Log("벽에 닿은지");

            var speed = lastVec.magnitude * 0.5f;
            if (DrugManager.Instance.green3) speed *= 1.25f;
            var dir = Vector2.Reflect(lastVec.normalized, collision.contacts[0].normal);

            rigid.velocity = dir * Mathf.Max(speed, 0f);
        }
    }
}
