using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBuff : ColorBuff
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
        DrugManager.Instance.blue1 = true;
        DrugManager.Instance.RunBlueBuff1();
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.blue2 = true;
        DrugManager.Instance.RunBlueBuff2();
    }
    
    public override void ThirdBuff()
    {
        DrugManager.Instance.blue3 = true;
        DrugManager.Instance.RunBlueBuff3();
    }
}
