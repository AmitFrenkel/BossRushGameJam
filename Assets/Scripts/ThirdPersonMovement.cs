using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody rigidBody;
    private bool isGrounded;
    [SerializeField] private Animator anim;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (vertical == 0 && horizontal == 0)
        {
            anim.SetInteger("State", 0);
        }
        else
            anim.SetInteger("State", 1);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right = right.normalized;

        Vector3 direction = forward * vertical + right * horizontal;
        rigidBody.velocity = direction * speed + Vector3.up * rigidBody.velocity.y;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            print("MR.Will to to to");
            anim.SetInteger("State", 2);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
        }
        else if (!isGrounded && rigidBody.velocity.y > 0)
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("IsGrounded", true);
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        anim.SetBool("IsGrounded", false);
    }
}