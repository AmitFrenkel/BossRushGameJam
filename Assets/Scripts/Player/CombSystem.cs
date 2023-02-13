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
    [SerializeField] private ThirdPersonMovement tpm;
    [SerializeField] private GameObject electroPrefab;
    public static bool canMove = true;
    private float temps,longPressDuration = 1.0f,mouseDownTime;
    private bool click,isMouseDown,longPressActivated;

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

        }
        if (Input.GetMouseButtonDown(0))
        {
            // Store the time when the mouse button was first pressed
            mouseDownTime = Time.time;
            isMouseDown = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (isMouseDown)
            {
                // Check if the mouse button has been held down for the specified duration
                if (Time.time - mouseDownTime >= longPressDuration)
                {
                    if (!longPressActivated)
                    {
                        // Long press detected
                        Debug.Log("Long Press");
                        anim.SetTrigger("ElectAttack");
                        switch (tpm.CurrentAbility.AbilityName)
                            {
                                
                                case StatusEffects.elect:
                                    anim.SetTrigger("ElectAttack");
                                    break;
                                case StatusEffects.ice:
                                    anim.SetTrigger("IceAttack");
                                    break;
                                case StatusEffects.fire:
                                    anim.SetTrigger("FireAttack");
                                    break;
                            }
                        longPressActivated = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown)
            {
                // Check if the mouse button was only held down for a short period of time
                if (Time.time - mouseDownTime < longPressDuration)
                {
                    // Short press detected
                    OnClick();
                    Debug.Log("Short Press");
                }
                isMouseDown = false;
                longPressActivated = false;
            }
        }
        // if (Input.GetMouseButtonDown (0))
        // {
        //     temps = Time.time ;
        //     click = true ;
        // }
        //
        // if (click && (Time.time - temps) > 0.2 )
        // {
        //     // long click effect
        //     // anim.SetTrigger("ElectAttack");
        //
        //     switch (tpm.CurrentAbility.AbilityName)
        //     {
        //         
        //         case StatusEffects.elect:
        //             anim.SetTrigger("ElectAttack");
        //             break;
        //         case StatusEffects.ice:
        //             anim.SetTrigger("IceAttack");
        //             break;
        //         case StatusEffects.fire:
        //             anim.SetTrigger("FireAttack");
        //             break;
        //     }
        // }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     click = false ;
        //
        //     if ( (Time.time - temps) < 0.2 )
        //     {
        //         // short click effect
        //         OnClick();
        //     }
        // }
        //cooldown time
        // if (Time.time > nextFireTime)
        // {
        //     if (Input.GetMouseButtonDown (0))
        //     {
        //         temps = Time.time ;
        //         click = true ;
        //     }
        //
        //     if (click && (Time.time - temps) > 0.2 )
        //     {
        //         // long click effect
        //         // anim.SetTrigger("ElectAttack");
        //
        //         switch (tpm.CurrentAbility.AbilityName)
        //         {
        //         
        //             case StatusEffects.elect:
        //                 anim.SetTrigger("ElectAttack");
        //                 break;
        //             case StatusEffects.ice:
        //                 anim.SetTrigger("IceAttack");
        //                 break;
        //             case StatusEffects.fire:
        //                 anim.SetTrigger("FireAttack");
        //                 break;
        //         }
        //     }
        //
        //     if (Input.GetMouseButtonUp(0))
        //     {
        //         click = false ;
        //
        //         if ( (Time.time - temps) < 0.2 )
        //         {
        //             // short click effect
        //             OnClick();
        //         }
        //     }
        // }
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
            other.GetComponent<EnemyStats>().ChangeHealth(-tpm.Player.Power);
            Instantiate(electroPrefab, transform.position, Quaternion.identity, null);

            // reduce enemy health
            print("hit!");
            tpm.AddLaserCharge(tpm.Player.Power * 2f);
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
    public static void DisableMoving()
    {
        canMove = false;
    }
    public void ForceForward()
    {
        playerMovement.velocity = Vector3.zero;
        playerMovement.AddForce(forwardDirection.forward * forwardSpeed);
    }
    
    
}
