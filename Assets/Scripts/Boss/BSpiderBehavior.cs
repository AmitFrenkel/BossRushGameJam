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
        walking
    }

    private NavMeshAgent agent;

    [Header("Behavior -")]
    [SerializeField] private bool isMovingWalls;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerLegs;
    [SerializeField] private Transform walkTarget;
    [SerializeField] private Animator anim;
    [SerializeField] private AgentLinkMover agentLinkMover;
    [SerializeField] private SpiderLegsAnimation spiderLegsAnim;
    [SerializeField] private SpiderState state;
    [SerializeField] private bool followPlayer;

    //attacks
    [Header("Attacks -")]
    [Header("IceCube Attack")]
    [SerializeField] private List<int> attacks;//1 - ice cubes
    [SerializeField] private bool isAttacking;
    private int iceCubesLeft;
    [SerializeField] private Transform throwLocation;
    [SerializeField] private int icePoolIndex;
    [SerializeField] private Transform[] iceCubes;
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

    [Header("Misc")]
    [Range(0, 10)]
    [SerializeField] private float stunTime;

    private void Start()
    {
        //followPlayer = true;
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        StartCoroutine(CheckForJumps());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AttackManager();
        }
        if (!isAttacking)
        {
            if (followPlayer)
                agent.SetDestination(player.position);

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
            default:
                break;
        }
    }
    public IEnumerator IceCubesAttack()
    {
        anim.Play("IceCubeThrow");
        agent.isStopped = true;
        iceCubesLeft = 5;
        while (iceCubesLeft > 0)
        {
            yield return null;
        }
        anim.SetTrigger("StopAttack");
        yield return new WaitForSeconds(stunTime);
        anim.SetTrigger("ExitStun");
        yield return new WaitForSeconds(2.5f);
        agent.isStopped = false;
        isAttacking = false;
        followPlayer = true;
    }
    public void CreateCubeProjectile()
    {
        iceCubesLeft--;
        Transform cube = iceCubes[icePoolIndex];
        cube.GetComponent<IceCubeProjectile>().warnHold = warnBehavior.CreateWarnAtAndReturn(playerLegs.position);
        cube.gameObject.SetActive(true);
        cube.position = throwLocation.position;
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
                print(true);
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
}