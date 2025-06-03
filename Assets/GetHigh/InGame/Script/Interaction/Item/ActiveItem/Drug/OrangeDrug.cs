using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDrug : Drug
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
        base.DrugAbility();
        DrugManager.Instance.aim += 1;
    }
}
