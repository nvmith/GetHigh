using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDrug : Drug
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void DrugAbility()
    {
        DrugManager.Instance.playerAttackDelay += 0.3f;
        //DrugManager.Instance.playerAttackDelay += 0.5f;
    }

}
