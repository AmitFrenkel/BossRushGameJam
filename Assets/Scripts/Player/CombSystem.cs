using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombSystem : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody playerMovement;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    public static bool canMove = true;
 
    private void Start()
    {
        // anim = GetComponent<Animator>();
    }
    void Update()
    {
 
        print("canmove"+anim.GetBool("hit1"));
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
        print("Mr.Summer2");
        canMove = false;
        //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
        // lastClickedTime = Time.time;
        // noOfClicks++;
        // if (noOfClicks == 1)
        // {
        // }
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

    private void OnCollisionEnter(Collision other)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || 
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                // reduce enemy health
            }
        }
    }

    public static void StopWillSummer()
    {
        print("Mr.summer");
        canMove = true;
    }
}
