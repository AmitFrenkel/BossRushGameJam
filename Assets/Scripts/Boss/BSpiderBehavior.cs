using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BSpiderBehavior : EnemyStats
{
    public enum SpiderState
    {
        intro,
        idle,
        jumping,
        walking,
        closeAttack
    }

    [SerializeField] private NavMeshAgent agent;

    [Header("Behavior -")]
    [SerializeField] private bool isMovingWalls;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerLegs;
    [SerializeField] private Transform walkTarget;
    [SerializeField] private AgentLinkMover agentLinkMover;
    [SerializeField] private SpiderLegsAnimation spiderLegsAnim;
    [SerializeField] private SpiderState state;
    [SerializeField] private bool followPlayer;
    [Header("Areas -")]
    [SerializeField] private Transform floor;
    [SerializeField] private Transform cieling;

    //attacks
    [Header("Attacks -")]
    [SerializeField] private List<int> attacks;//1 - ice cubes | 2 - ice spikes cieling | 3 - close attack
    [Header("IceCube Attack")]
    [SerializeField] private bool isAttacking;
    private int iceCubesLeft;
    [SerializeField] private Transform throwLocation;
    [SerializeField] private int icePoolIndex;
    [SerializeField] private Transform[] iceCubes;
    [SerializeField] private Transform[] throwingLocations;
    [Range(0, 90)]
    [SerializeField] private float throwAngle;
    [Range(0, 1)]
    [Tooltip("Using a values closer to 0 will make the agent throw with the lower force"
       + "down to the least possible force (highest angle) to reach the target.\n"
       + "Using a value of 1 the agent will always throw with the MaxThrowForce below.")]
    public float ForceRatio = 0;
    [SerializeField]
    [Tooltip("If the required force to throw the attack is greater than this value, "
        + "the agent will move closer until they come within range.")]
    private float MaxThrowForce = 25;

    [Header("IceSpikes Ceiling Attack")]
    private int iceSpikesLeft;
    [SerializeField] private int spikePoolIndex;
    [SerializeField] private Transform[] iceSpikes;
    [SerializeField] private BoxCollider triggerFallCheck;

    [Header("Misc")]
    [Range(0, 10)]
    [SerializeField] private float stunTime;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private bool onceCielingAttack;// CHECK IF WORKS????????? LIKE WHAT HTE HECK

    private void Start()
    {
        ToDoOnStart();
        agent.autoTraverseOffMeshLink = false;
        StartCoroutine(CooldownBetweenAttacks());
        StartCoroutine(CheckForJumps());
    }
    public void ExitStun()
    {
        anim.SetTrigger("ExitStun");
        StartCoroutine(CooldownBetweenAttacks());
        StartCoroutine(CheckForJumps());
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
    private void Update()
    {
        if (waitsForStun && attacks[0] != 2)
        {
            StopAllCoroutines();
            followPlayer = false;
            waitsForStun = false;
            anim.SetTrigger("Iced");
            Invoke("ExitStun", 10f);
            agent.isStopped = true;
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    attacks[0] = 1;
        //    AttackManager();
        //}
        // else if (Input.GetKeyDown(KeyCode.J))
        //{
        //    attacks[0] = 2;
        //    AttackManager();
        //}
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    attacks[0] = 3;
        //    AttackManager();
        //}
        if (followPlayer)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= 11f && state != SpiderState.closeAttack)
            {
                //Do Close attack
                state = SpiderState.closeAttack;
                attacks[0] = 3;
                AttackManager();
            }
            if (health <= 50 && onceCielingAttack)
            {
                followPlayer = false;
                onceCielingAttack = false;
                StopCoroutine(CooldownBetweenAttacks());
                attacks[0] = 2;
                AttackManager();
            }
        }

        if (agent.velocity == Vector3.zero)
            {
                state = SpiderState.idle;
                anim.SetInteger("State", 0);
            }
            else
            {
                state = SpiderState.walking;
                anim.SetInteger("State", 1);
            }
        
    }
    public void AttackManager()
    {
        followPlayer = false;
        isAttacking = true;
        switch (attacks[0])
        {
            case 1:
                StartCoroutine(IceCubesAttack());
                break;
            case 2:
                StartCoroutine(CielingSpikesAttack());
                break;
            case 3:
                StartCoroutine(CloseAttack());
                break;
            default:
                break;
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
    public IEnumerator IceCubesAttack()
    {
        int location = Random.Range(0, 4);
        agent.SetDestination(throwingLocations[location].position);
        while (Vector3.Distance(transform.position, throwingLocations[location].position) > agent.stoppingDistance)
        {
            yield return null;
        }
        agent.enabled = false;
        transform.parent.parent.LookAt(player);
        agent.enabled = true;
        anim.Play("IceCubeThrow");
        agent.isStopped = true;
        iceCubesLeft = 5;
        while (iceCubesLeft > 0)
        {
            //Vector3 direction = Point - transform.position;
            //Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);
            yield return null;
        }
        anim.SetTrigger("StopAttack");
        //yield return new WaitForSeconds(stunTime);
        anim.SetTrigger("ExitStun");
        yield return new WaitForSeconds(2.5f);
        agent.isStopped = false;
        isAttacking = false;
        followPlayer = true;
        StartCoroutine(CooldownBetweenAttacks());
    }
    public IEnumerator CielingSpikesAttack()
    {
        while (Vector3.Distance(transform.position,cieling.position) >= 11)
        {
            agent.SetDestination(cieling.position);
            yield return null;
        }
        anim.Play("CielingAttach");
        followPlayer = false;
        triggerFallCheck.enabled = false;
        iceSpikesLeft = 10;
        while (iceSpikesLeft != 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        anim.SetTrigger("StartFall");
        agent.enabled = false;
        rigidbody.isKinematic = false;
        while (transform.parent.parent.position.y > 3.6f)
        {
            yield return null;
        }
        agent.enabled = true;
        rigidbody.isKinematic = true;
        triggerFallCheck.enabled = true;
        anim.SetTrigger("StopAttack");
        yield return new WaitForSeconds(stunTime);
        anim.SetTrigger("ExitStun");
        yield return new WaitForSeconds(2.5f);
        agent.isStopped = false;
        isAttacking = false;
        followPlayer = true;
        StartCoroutine(CooldownBetweenAttacks());
    }
    public void EndedEntrance()
    {
        StartCoroutine(EntEnded());
    }
    private IEnumerator EntEnded()
    {
        yield return new WaitForSeconds(3);
        followPlayer = true;
    }
    public void StartSpikes()
    {
        StartCoroutine(CreateIceSpikes());
    }
    Vector3 RandomPointOnXZCircle(Vector3 center, float radius)
    {
        Vector3 offset = Random.insideUnitSphere * radius;
        Vector3 pos = center + offset;
        return pos;
    }
    public IEnumerator CreateIceSpikes()
    {
        iceSpikesLeft = 10;
        while (iceSpikesLeft != 0)
        {
            int toDrop = Mathf.Min(Random.Range(2, 6), iceSpikesLeft);
            for (int i = 0; i < toDrop; i++)
            {
                Transform spike = iceSpikes[spikePoolIndex];
                Vector3 temp = RandomPointOnXZCircle(transform.position, 20);
                spike.position = new Vector3(temp.x,transform.position.y, temp.z);
                spike.gameObject.SetActive(true);
                spike.GetComponent<Rigidbody>().isKinematic = false;
                spike.GetChild(0).GetComponent<IceCubeProjectile>().GetComponent<IceCubeProjectile>().warnHold = warnBehavior.CreateWarnAtAndReturn(spike.position, spike,true);

                iceSpikesLeft--;

                spikePoolIndex++;
                if (spikePoolIndex >= iceSpikes.Length)
                    spikePoolIndex = 0;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(2);
        }
    }
    public void StartFall()
    {
        //triggerFallCheck.enabled = true;
        //agent.enabled = false;
        //rigidbody.isKinematic = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            agent.enabled = true;
            rigidbody.isKinematic = true;
        }
        else if (other.CompareTag("Ice"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
    }
    public void CreateCubeProjectile()
    {
        iceCubesLeft--;
        Transform cube = iceCubes[icePoolIndex];
        cube.gameObject.SetActive(true);
        cube.position = throwLocation.position;
        cube.GetComponent<IceCubeProjectile>().warnHold = warnBehavior.CreateWarnAtAndReturn(playerLegs.position, cube,false);
        Rigidbody rigid = cube.GetComponent<Rigidbody>();
        ForceRatio = Mathf.Clamp01(Vector3.Distance(transform.position, player.position) / 50);

        ThrowData throwData = CalculateThrowData(player.position, cube.position);
        rigid.useGravity = true;
        rigid.isKinematic = false;
        rigid.velocity = throwData.ThrowVelocity;

        icePoolIndex++;
        if (icePoolIndex >= iceCubes.Length)
            icePoolIndex = 0;
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
                spiderLegsAnim.ResetLegs();
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
        isMovingWalls = false;
        agentLinkMover.allowedToMove = true;
    }

    private ThrowData CalculateThrowData(Vector3 TargetPosition, Vector3 StartPosition)
    {
        // v = initial velocity, assume max speed for now
        // x = distance to travel on X/Z plane only
        // y = difference in altitudes from thrown point to target hit point
        // g = gravity

        Vector3 displacement = new Vector3(
            TargetPosition.x,
            StartPosition.y,
            TargetPosition.z
        ) - StartPosition;
        float deltaY = TargetPosition.y - StartPosition.y;
        float deltaXZ = displacement.magnitude;

        // find lowest initial launch velocity with other magic formula from https://en.wikipedia.org/wiki/Projectile_motion
        // v^2 / g = y + sqrt(y^2 + x^2)
        // meaning.... v = sqrt(g * (y+ sqrt(y^2 + x^2)))
        float gravity = Mathf.Abs(Physics.gravity.y);
        float throwStrength = Mathf.Clamp(
            Mathf.Sqrt(
                gravity
                * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY, 2)
                + Mathf.Pow(deltaXZ, 2)))),
            0.01f,
            MaxThrowForce
        );
        throwStrength = Mathf.Lerp(throwStrength, MaxThrowForce, ForceRatio);

        float angle;
        if (ForceRatio == 0)
        {
            // optimal angle is chosen with a relatively simple formula
            angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));
        }
        else
        {
            // when we know the initial velocity, we have to calculate it with this formula
            // Angle to throw = arctan((v^2 +- sqrt(v^4 - g * (g * x^2 + 2 * y * v^2)) / g*x)
            angle = Mathf.Atan(
                (Mathf.Pow(throwStrength, 2) - Mathf.Sqrt(
                    Mathf.Pow(throwStrength, 4) - gravity
                    * (gravity * Mathf.Pow(deltaXZ, 2)
                    + 2 * deltaY * Mathf.Pow(throwStrength, 2)))
                ) / (gravity * deltaXZ)
            );
        }

        if (float.IsNaN(angle))
        {
            // you will need to handle this case when there
            // is no feasible angle to throw the object and reach the target.
            return new ThrowData();
        }

        Vector3 initialVelocity =
            Mathf.Cos(angle) * throwStrength * displacement.normalized
            + Mathf.Sin(angle) * throwStrength * Vector3.up;

        return new ThrowData
        {
            ThrowVelocity = initialVelocity,
            Angle = angle,
            DeltaXZ = deltaXZ,
            DeltaY = deltaY
        };
    }

    private struct ThrowData
    {
        public Vector3 ThrowVelocity;
        public float Angle;
        public float DeltaXZ;
        public float DeltaY;
    }

    public IEnumerator CooldownBetweenAttacks()
    {
        int timer = Random.Range(10, 30);
        yield return new WaitForSeconds(timer);
        while (!followPlayer)
        {
            yield return null;
        }
        switch (Random.Range(0,6))
        {
            case 0:
                attacks[0] = 2;
                AttackManager();
                break;
            default:
                attacks[0] = 1;
                AttackManager();
                break;
        }
    }
}