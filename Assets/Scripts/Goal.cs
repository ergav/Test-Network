using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    [SerializeField] private bool isTeam1;

    private bool goalReached;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (goalReached)
            {
                return;
            }

            if (!manager.IsServer)
            {
                return;
            }
            
            if (isTeam1)
            {
                manager.ScoreTeam2RPC();
            }
            else
            {
                manager.ScoreTeam1RPC();
            }

            goalReached = true;
            
            StartCoroutine(ResetTimer());
        }
    }

    IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(1);
        manager.ResetPosition();
        goalReached = false;
    }
}