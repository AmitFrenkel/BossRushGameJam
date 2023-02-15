using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BJennyBehavior : EnemyStats
{
    public enum JennyState
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
    [SerializeField] private JennyState state;
    [SerializeField] private Rigidbody rb;
    [Header("Attacks -")]
    [SerializeField] private List<int> attacks;//1 - laser attack | 2 - punch attack| 3 - smack attack
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool onceLaserAttack;
    [SerializeField] private float punchForce;
    [SerializeField] private Transform fistTarget;
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
        StartCoroutine(EntEnded());
        //StartCoroutine(CooldownBetweenAttacks());
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
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddEffect(StatusEffects.ice);
        }

        if (waitsForStun && attacks[0] != 2)
        {
            StopAllCoroutines();
            followPlayer = false;
            waitsForStun = false;
            anim.SetTrigger("EnterStun");
            Invoke("ExitStun", 10f);
            agent.isStopped = true;
        }
        if (followPlayer)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= 5f && state != JennyState.closeAttack)
            {
                //Do Close attack
                state = JennyState.closeAttack;
                attacks[0] = 3;
                AttackManager();
            }
            if (health <= 50 && onceLaserAttack)
            {
                followPlayer = false;
                onceLaserAttack = false;
                StopCoroutine(CooldownBetweenAttacks());
                attacks[0] = 2;
                AttackManager();
            }
        }

        if (agent.velocity == Vector3.zero)
        {
            state = JennyState.idle;
            anim.SetInteger("State", 0);
        }
        else
        {
            state = JennyState.walking;
            anim.SetInteger("State", 1);
        }
    }
    public IEnumerator CloseAttack()
    {
        followPlayer = true;
        while (Vector3.Distance(transform.position, player.position) >= 5f)
        {
            yield return null;
        }
        agent.enabled = false;
        transform.parent.LookAt(player);
        agent.enabled = true;
        //transform.eulerAngles = new Vector3(0, -90, 65.927f);
        followPlayer = false;
        if (Random.Range(0,2) == 0)
            anim.SetTrigger("Punch");
        else
            anim.SetTrigger("Smack");

        //anim.Play("Punch");
        yield return new WaitForSeconds(4);
        followPlayer = true;
    }
    public void ResetFistObject()
    {
        fistTarget.position = player.position;
    }
    public void StartForce()
    {
        StartCoroutine(ForceTowards());
    }
    public IEnumerator ForceTowards()
    {
        rb.isKinematic = false;
        agent.enabled = false;
        rb.AddForce(transform.right*punchForce);
        yield return new WaitForSeconds(0.1f);
        while (rb.velocity.magnitude > 0.1f)
        {
            yield return null;
        }
        rb.isKinematic = true;
        agent.enabled = true;
    }
    public void AttackManager()
    {
        followPlayer = false;
        isAttacking = true;
        switch (attacks[0])
        {
            case 1:
                //StartCoroutine(TowerAttack());//tower attack
                break;
            case 2:
                //StartCoroutine(SlashAttack());//slash attack
                break;
            case 3:
                StartCoroutine(CloseAttack());
                break;
            default:
                break;
        }
    }
    public IEnumerator LaserAttack()
    {
        yield return null;
    }
    public IEnumerator PunchAttack()
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
