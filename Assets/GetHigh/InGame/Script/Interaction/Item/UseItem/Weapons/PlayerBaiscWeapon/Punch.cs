using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : BasicWeapon
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
        SoundManager.Instance.PlaySFX(SFX.Punch_Shot);

        InGameManager.Instance.BasicWeaponCheck(true);
        InGameManager.Instance.player.KnifeAttack(true);
        spriteRenderer.enabled = false;
        InGameManager.Instance.player.isAttack = true;
        InGameManager.Instance.player.hadnSprite.enabled = false;
        fireTime = 0;

        yield return new WaitForSeconds(0.15f);

        SoundManager.Instance.PlaySFX(SFX.Punch_Shot);

        yield return new WaitForSeconds(0.15f);

        InGameManager.Instance.BasicWeaponCheck(false);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
        InGameManager.Instance.player.hadnSprite.enabled = true;
    }

    public override void CancleAttack()
    {
        gameObject.SetActive(false);
        InGameManager.Instance.BasicWeaponCheck(false);
        InGameManager.Instance.player.hadnSprite.enabled = true;
        if (AttackCoroutine == null) return;

        StopCoroutine(AttackCoroutine);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
        AttackCoroutine = null;
    }
}
