using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    protected override void ShotDelay()
    {
        if (fireTime >= fireMaximumDelay &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 10.0f + DrugManager.Instance.playerAttackDelay/ 2.4f)
        {
            Debug.Log("DrugManager.Instance.playerAttackDelay / 2.4f : " + DrugManager.Instance.playerAttackDelay / 2.4f);
            StartCoroutine(Shot());
        }
    }
}
