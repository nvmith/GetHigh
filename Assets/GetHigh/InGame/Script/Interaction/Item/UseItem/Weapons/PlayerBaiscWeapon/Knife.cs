using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BasicWeapon
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

    protected override void AttackDelay()
    {
        base.AttackDelay();
    }

    protected override IEnumerator Attack()
    {
        SoundManager.Instance.PlaySFX(SFX.Knife_Shot);
        InGameManager.Instance.BasicWeaponCheck(true);
        InGameManager.Instance.player.KnifeAttack(true);
        spriteRenderer.enabled = false;
        InGameManager.Instance.player.isAttack = true;
        fireTime = 0;
        InGameManager.Instance.TrailCheck(true);

        yield return new WaitForSeconds(0.3f);

        InGameManager.Instance.BasicWeaponCheck(false);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
        InGameManager.Instance.TrailCheck(false);
    }

    public override void CancleAttack()
    {
        base.CancleAttack();
    }
}
