using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BSpiderBehavior : EnemyStats
{
    private NavMeshAgent agent;

    private Transform legs;
    [SerializeField] private bool isMovingWalls;
    [SerializeField] private Transform player;
    [SerializeField] private Animator anim;
    [SerializeField] private AgentLinkMover agentLinkMover;
    private void Update()
    {
        agent.SetDestination(player.position);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        StartCoroutine(CheckForJumps());
    }
    public IEnumerator CheckForJumps()
    {
        while (true)
        {
            if (agent.isOnOffMeshLink && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (!isMovingWalls)
                {
                    isMovingWalls = true;
                    anim.SetBool("OnGround", false);
                    anim.Play("Jump");
                }
                while (isMovingWalls)
                {
                    agent.autoTraverseOffMeshLink = false;
                    yield return new WaitForSecondsRealtime(0.01f);
                }
            }
            else
            {
                isMovingWalls = false;
                anim.SetBool("OnGround", true);
            }

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    public void BeganJump()
    {
        print(true);
        isMovingWalls = false;
        agentLinkMover.allowedToMove = true;
    }
}
