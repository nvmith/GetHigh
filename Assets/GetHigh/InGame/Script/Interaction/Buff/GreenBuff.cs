using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBuff : ColorBuff
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

    public override void FirstBuff()
    {
        DrugManager.Instance.green1 = true;
        DrugManager.Instance.RunGreenBuff1();
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.green2 = true;
        DrugManager.Instance.RunGreenBuff2();
    }

    public override void ThirdBuff()
    {
        DrugManager.Instance.green3 = true;
        DrugManager.Instance.RunGreenBuff3();
    }
}
