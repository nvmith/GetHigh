using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicWeapon : MonoBehaviour
{
    public float fireDelay = 2.0f;
    public float fireMaximumDelay = 0.15f;
    protected float fireTime = 0;
    public SpriteRenderer spriteRenderer;
    public Coroutine AttackCoroutine;

    protected virtual void Awake()
    {
    }


    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        fireTime += Time.deltaTime;

        if (InGameManager.Instance.player.AttackKey)
        {
            AttackDelay();
        }
    }

    protected virtual void AttackDelay()
    {
        if (fireTime >= fireMaximumDelay &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 10.0f + DrugManager.Instance.playerAttackDelay / 6.0f)
        {
            AttackCoroutine = StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack()
    {
        InGameManager.Instance.BasicWeaponCheck(true);
        InGameManager.Instance.player.KnifeAttack(true);
        spriteRenderer.enabled = false;
        InGameManager.Instance.player.isAttack = true;
        fireTime = 0;

        yield return new WaitForSeconds(0.3f);

        InGameManager.Instance.BasicWeaponCheck(false);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
    }

    public virtual void CancleAttack()
    {
        gameObject.SetActive(false);
        InGameManager.Instance.BasicWeaponCheck(false);
        if (AttackCoroutine == null) return;

        StopCoroutine(AttackCoroutine);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
        AttackCoroutine = null;
    }
}
