using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BSpiderBehavior : EnemyStats
{
    private NavMeshAgent agent;

    private Transform legs;
    [SerializeField] private Transform player;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        agent.SetDestination(player.position);
    }
}
