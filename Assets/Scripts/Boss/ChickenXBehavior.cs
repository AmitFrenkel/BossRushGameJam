using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenXBehavior : EnemyStats
{
    public enum ChickenState
    {
        idle,
        walking,
        closeAttack
    }

    [SerializeField] private bool followPlayer;
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
    [SerializeField] private bool isFlying;
    [SerializeField] private Transform[] flightLocations;
    [SerializeField] private float flightSpeed;
    [SerializeField] private float flyingRotationSpeed;
    [SerializeField] private Transform[] toastLocations;
    [SerializeField] private Transform toastSource;
    [SerializeField] private int toastIndex;
    [SerializeField] private GameObject toastPrefab;
    private int amountOfToasts = 9;
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
        //StartCoroutine(CooldownBetweenAttacks());
        //StartCoroutine(EntEnded());
        attacks[0] = 1;
        AttackManager();
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            StopCoroutine(CooldownBetweenAttacks());
            attacks[0] = 1;
            AttackManager();
        }
        if (followPlayer)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= 3.5f && state != ChickenState.closeAttack)
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
        while (Vector3.Distance(transform.position, player.position) >= 3.5f)
        {
            yield return null;
        }
        followPlayer = false;
        agent.enabled = false;
        transform.parent.LookAt(player);
        agent.enabled = true;
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
    public void Flying()
    {
        isFlying = true;
    }
    public IEnumerator TowerAttack()
    {
        followPlayer = false;
        agent.isStopped = true;
        agent.enabled = false;
        anim.SetTrigger("Fly");
        while (!isFlying)
        {
            yield return null;
        }
        Vector3 flightTarget = Vector3.zero;
        for (int i = 0; i < flightLocations.Length; i++)
        {
            flightTarget = flightLocations[i].position; 
            while (Vector3.Distance(transform.parent.position, flightTarget) > 1f)
            {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, flightTarget, flightSpeed*Time.deltaTime);

                Vector3 relativePos = flightTarget - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rotation, Time.time * flyingRotationSpeed);

                //transform.parent.eulerAngles = Vector3.Slerp(transform.parent.position, flightTarget, flightSpeed*Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
        flightTarget = towers[Random.Range(0, towers.Length)].position;
        while (Vector3.Distance(transform.parent.position, flightTarget) > 0.1f)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, flightTarget, flightSpeed * Time.deltaTime);

            Vector3 relativePos = flightTarget - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rotation, Time.time * flyingRotationSpeed);

            //transform.parent.eulerAngles = Vector3.Slerp(transform.parent.position, flightTarget, flightSpeed*Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        anim.SetTrigger("Landed");
        transform.parent.rotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("ToastTime");
        amountOfToasts = 9;
        while (amountOfToasts != 0)
        {
            yield return null;
        }
        anim.SetTrigger("StopToast");
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Fly");
        while (Vector3.Distance(transform.parent.position, floor.position) > 0.1f)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, floor.position, flightSpeed * Time.deltaTime);

            Vector3 relativePos = floor.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rotation, Time.time * flyingRotationSpeed);
            yield return new WaitForSeconds(0.01f);
        }
        anim.SetTrigger("Landed");
        agent.enabled = true;
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("EnterStun");
        yield return new WaitForSeconds(7f);
        anim.SetTrigger("ExitStun");
        yield return new WaitForSeconds(3f);
        /*while (Vector3.Distance(transform.position, player.position) >= 3.5f)
        {
            yield return null;
        }
        followPlayer = false;
        agent.enabled = false;
        transform.parent.LookAt(player);
        agent.enabled = true;
        yield return new WaitForSeconds(4);
        followPlayer = true;*/
        isFlying = false;
        followPlayer = true;
        agent.isStopped = false;
    }
    public void ToastOut()
    {
        //create toast
        Transform toast = Instantiate(toastPrefab, toastSource.position, Quaternion.identity, null).transform;
        toast.LookAt(toastLocations[toastIndex]);
        toast.GetChild(0).GetComponent<IceCubeProjectile>().warnHold = 
           warnBehavior.CreateWarnAtAndReturn(toastLocations[toastIndex].position, toast, false);
        toastIndex++;
        amountOfToasts--;
        if (toastIndex > toastLocations.Length)
        {
            toastIndex = 0;
        }
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
