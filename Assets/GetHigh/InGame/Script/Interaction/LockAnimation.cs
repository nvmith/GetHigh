using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAnimation : MonoBehaviour
{
    Animator ani;

    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void AniPlay(int index)
    {
        ani.SetTrigger(index.ToString());
    }
}