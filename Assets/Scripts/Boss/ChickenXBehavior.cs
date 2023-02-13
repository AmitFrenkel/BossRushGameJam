using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenXBehavior : EnemyStats
{
    public enum ChickenState
    {
        intro,
        idle,
        walking,
        closeAttack
    }

    private bool followPlayer;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerLegs;
    [SerializeField] private Transform floor;
    [SerializeField] private Transform[] towers;
    [SerializeField] private ChickenState state;
    [Header("Attacks -")]
    [SerializeField] private List<int> attacks;//1 - tower attack | 2 - slash attack| 3 - close attack
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool onceFlyingAttack;
    public void EndedEntrance()
    {
        StartCoroutine(EntEnded());
    }
    private IEnumerator EntEnded()
    {
        yield return new WaitForSeconds(3);
        followPlayer = true;
    }
    private void Start()
    {
        ToDoOnStart();
        StartCoroutine(CooldownBetweenAttacks());
    }
    public void ExitStun()
    {
        anim.SetTrigger("ExitStun");
        StartCoroutine(CooldownBetweenAttacks());
        Invoke("ExitStunDelay", 3);
    }
    public void ExitStunDelay()
    {
        followPlayer = true;
        agent.isStopped = false;
        ResetIce();
    }
    public void Death()
    {
        StopAllCoroutines();
        agent.isStopped = true;
        followPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (followPlayer)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= 11f && state != ChickenState.closeAttack)
            {
                //Do Close attack
                state = ChickenState.closeAttack;
                attacks[0] = 3;
                AttackManager();
            }
            if (health <= 50 && onceFlyingAttack)
            {
                followPlayer = false;
                onceFlyingAttack = false;
                StopCoroutine(CooldownBetweenAttacks());
                attacks[0] = 2;
                AttackManager();
            }
        }

        if (agent.velocity == Vector3.zero)
        {
            state = ChickenState.idle;
            anim.SetInteger("State", 0);
        }
        else
        {
            state = ChickenState.walking;
            anim.SetInteger("State", 1);
        }

    }
    public IEnumerator CloseAttack()
    {
        followPlayer = true;
        while (Vector3.Distance(transform.position, player.position) >= 12)
        {
            yield return null;
        }
        agent.enabled = false;
        transform.parent.parent.LookAt(player);
        agent.enabled = true;
        followPlayer = false;
        anim.Play("CloseAttack");
        yield return new WaitForSeconds(4);
        followPlayer = true;
    }
    public void AttackManager()
    {
        followPlayer = false;
        isAttacking = true;
        switch (attacks[0])
        {
            case 1:
                StartCoroutine(TowerAttack());//tower attack
                break;
            case 2:
                StartCoroutine(SlashAttack());//slash attack
                break;
            case 3:
                StartCoroutine(CloseAttack());
                break;
            default:
                break;
        }
    }
    public IEnumerator TowerAttack()
    {
        yield return null;
    }
    public IEnumerator SlashAttack()
    {
        yield return null;
    }
    public IEnumerator CooldownBetweenAttacks()
    {
        int timer = Random.Range(10, 30);
        yield return new WaitForSeconds(timer);
        while (!followPlayer)
        {
            yield return null;
        }
        switch (Random.Range(0, 6))
        {
            case 0:
                attacks[0] = 1;
                AttackManager();
                break;
            default:
                attacks[0] = 2;
                AttackManager();
                break;
        }
    }
}
