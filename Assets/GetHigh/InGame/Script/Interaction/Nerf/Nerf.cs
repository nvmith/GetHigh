using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Nerf : MonoBehaviour
{
    protected int nerfCount = 0; // 해당 항목의 너프 개수
    protected bool isActive = false;

    public void NerfOn()
    {
        int a = Random.Range(0, nerfCount);
        ActiveLogic(a);
    }

    protected abstract void ActiveLogic(int value);
}
