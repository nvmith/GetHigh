using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorBuff : MonoBehaviour
{
    public EDrugColor colorValue;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void ExcuteBuff(int i)
    {
        if (i == 0) FirstBuff();
        else if (i == 1) SecondBuff();
        else if (i == 2) ThirdBuff();

        int index = ((int)colorValue) * 3 + i;
        Debug.Log("버프 실행 인덱스 값 : " + index);
        //    (i + 1) - 1;
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Extra_Health + index);
        Debug.Log(i+1 + " 버프 실행");
    }

    public abstract void FirstBuff();

    public abstract void SecondBuff();

    public abstract void ThirdBuff();

}
