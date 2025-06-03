using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackRoomController : MonoBehaviour
{
    public AI[] agents;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            foreach (AI a in agents) a.PlayerRoom();
        }
    }
}
