using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombSystem : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody playerMovement;
    [SerializeField] private BoxCollider attackCollider;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private Transform forwardDirection;
    public static bool canMove = true;
 
    private void Start()
    {
        canMove = true;
        // anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            anim.SetBool("hit1", false);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetBool("hit2", false);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            anim.SetBool("hit3", false);
            anim.SetBool("hit1", false);
            noOfClicks = 0;
        }

        //cooldown time
        if (Time.time > nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
 
            }
        }
    }
 
    void OnClick()
    {
        canMove = false;
        anim.SetBool("hit1", true);
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
 
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("Enemy"))
            {
                // reduce enemy health
                print("hit!");
                other.GetComponent<EnemyStats>().ChangeHealth(-1);
            }
        
    }
    public void ToggleCollider(bool toggle)
    {
        attackCollider.enabled = toggle;
    }
    public static void EnableMoving()
    {
        canMove = true;
    }
    public void ForceForward()
    {
        playerMovement.velocity = Vector3.zero;
        playerMovement.AddForce(forwardDirection.forward * forwardSpeed);
    }
    
    
}
