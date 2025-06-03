using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDrug : Drug
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
        DrugManager.Instance.speed += 0.25f;
        InGameManager.Instance.player.speedApply = DrugManager.Instance.speed + InGameManager.Instance.Speed;
        if (!InGameManager.Instance.player.rollCnt)
            InGameManager.Instance.player.speed = InGameManager.Instance.player.speedApply;
    }
}
