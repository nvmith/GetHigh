using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuiper : Player
{

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

    protected override void Damage(int power)
    {
        base.Damage(power);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override IEnumerator ESkill()
    {
        yield return null;
        isSkill = false;
    }

    protected override void PlayerSkill()
    {
        //ebug.Log("기본 플레이어 스킬");
       
    }
}
