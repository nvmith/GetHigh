using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eunha : Player
{
    public GameObject fogPrefab;
    public bool fogCheck = false;
    public float fogDistance = 0;
    public float fogTimer = 0;
    public GameObject fog;

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

        if(fogCheck)
        {
            fogTimer += Time.deltaTime;

            if (fogDistance > 3) 
            { 
                fogIn = false;
            }
            else 
            { 
                fogIn = true;
            }

            if(fogTimer > 3)
            {
                fogCheck = false;
                fogTimer = 0;
                fogIn = false;

                Destroy(fog);
                fog = null;
            }
        }
    }
    protected override void Damage(int power)
    {
        if (fogIn) return;
        base.Damage(power);
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        /*if (collision.tag == "EnemyBullet") //수정
        {
            if (fogIn) return;

            if (avoidCheck || isHit) return;

            Damage(1); // 데미지 로직 나중에 수정
        }*/
    }

    protected override IEnumerator ESkill()
    {
        return base.ESkill();
    }

    protected override void PlayerSkill()
    {
        fogCheck = true;
        fogDistance = 0;
        fogIn = true;
        fogTimer = 0;

        fog = Instantiate(fogPrefab, transform.position, transform.rotation);
    }
}
