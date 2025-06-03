using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Nerf : MonoBehaviour
{
    protected int nerfCount = 0; // �ش� �׸��� ���� ����
    protected bool isActive = false;

    public void NerfOn()
    {
        int a = Random.Range(0, nerfCount);
        ActiveLogic(a);
    }

    protected abstract void ActiveLogic(int value);
}
