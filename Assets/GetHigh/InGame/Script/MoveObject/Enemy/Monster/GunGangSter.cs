using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGangSter : Agent
{
    [SerializeField]
    private EnemyGun gun;

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

    protected override void AttackLogic()
    {
        /*
        Vector2 bulletDir = (target.position - muzzle.position).normalized;
        muzzle.up = bulletDir;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + Random.Range(-attackRecoil, attackRecoil + 1));

        GameObject bulletcopy = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletcopy.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;*/
        attackDistance = Random.Range(3.0f, 5.0f);
        gun.fireSpeed = Random.Range(6.0f, 7.0f);
        gun.ShotReady(isReverse);
    }
}
