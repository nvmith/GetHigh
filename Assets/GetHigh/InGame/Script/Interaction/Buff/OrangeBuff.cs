using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBuff : ColorBuff
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
        DrugManager.Instance.orange1= true;
        DrugManager.Instance.RunOrangeBuff1();
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.orange2 = true;
        DrugManager.Instance.RunOrangeBuff2();
    }

    public override void ThirdBuff()
    {
        DrugManager.Instance.orange3 = true;
        DrugManager.Instance.RunOrangeBuff3();
    }
}
