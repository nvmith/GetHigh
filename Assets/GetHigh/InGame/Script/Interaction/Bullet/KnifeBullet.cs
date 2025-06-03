using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBullet : Bullet
{
    public float eraseTimer = 0;
    public BoxCollider2D box2DCol;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void TrrigerLogic()
    {
        target = null;
        PoolManager.Instance.ReturnKnifeBullet(this);
    }

    protected override IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed + eraseTimer);
        TrrigerLogic();
        box2DCol.enabled = true;
    }
    


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !InGameManager.Instance.player.AvoidCheck
            && !InGameManager.Instance.player.IsHit)
        {
            if (InGameManager.Instance.player.fogIn) return;
            TrrigerLogic();
        }
        else if(collision.tag == "Wall" || collision.tag == "Door")
        {
            rigid.velocity = Vector2.zero;
            moveDir = Vector2.zero;
            box2DCol.enabled = false;
        }
        else if(collision.tag == "Table")
        {
            TrrigerLogic();
        }
    }
}
