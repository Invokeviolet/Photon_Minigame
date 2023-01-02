using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnermyAI : MonoBehaviour
{
    NavMeshAgent nav;
    Player target;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        target = GetComponent<Player>();
    }
    private void Update()
    {
        if (nav.destination != target.transform.position)
        {
            nav.SetDestination(target.transform.position);
        }
        else
        {
            nav.SetDestination(transform.position);
        }
    }
}
