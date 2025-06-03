using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBuff : ColorBuff
{
    
    // Start is called before the first frame update
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
        DrugManager.Instance.yellow1 = true;
        DrugManager.Instance.RunYellowBuff1();
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.yellow2 = true;
        DrugManager.Instance.RunYellowBuff2();
    }

    public override void ThirdBuff()
    {
        DrugManager.Instance.yellow3 = true;
        DrugManager.Instance.RunYellowBuff3();
    }
}
