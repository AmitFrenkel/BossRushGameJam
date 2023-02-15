using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimHelper : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement tpm;
    [SerializeField] private GameObject trails;
    [SerializeField] private CombSystem comb;
    [SerializeField] private Transform icePosition, toastPosition,electricalPosition;
    [SerializeField] private GameObject ice, toast, electric;
    [SerializeField] private float power,toastPower;
    [SerializeField] private Animator anim;
    public void EnableCollider()
    {
        comb.ToggleCollider(true);
    }
    public void DisableCollider()
    {
        comb.ToggleCollider(false);
    }

    public void CanMove()
    {
        CombSystem.EnableMoving();
    }
    public void CantMove()
    {
        CombSystem.DisableMoving();
    }
    public void ForceTowardsEnemy()
    {
        comb.ForceForward();
    }

    public void EnableTrails()
    {
        trails.SetActive(true);
    }
    public void DisableTrails()
    {
        trails.SetActive(false);
    }

    public void ActivateIce()
    {
        var rb = Instantiate(ice,icePosition.position,quaternion.identity,null).GetComponent<Rigidbody>();
        // var startPos = rb.transform.localPosition.z;
        for (int i = 0; i < tpm.CurrentAbility.GetComponent<IceAttack>().NumberOfIce; i++)
        {
            rb.AddForce((transform.forward+Vector3.up*0.8f)*power);
            // StartCoroutine(DestroyRB(rb.gameObject));
        }
        anim.SetTrigger("ExitIceAttack");
        Invoke("CanMove",0.5f);
       
    }
    public void ActivateToast()
    {
        // var startPos = rb.transform.localPosition.z;
        // for (int i = 0; i < tpm.CurrentAbility.GetComponent<IceAttack>().NumberOfIce; i++)
        // {
        //     var rb = Instantiate(ice,icePosition.position,quaternion.identity,null).GetComponent<Rigidbody>();
        //     rb.AddForce((transform.forward+Vector3.up*0.4f)*power);
        //     // StartCoroutine(DestroyRB(rb.gameObject));
        // }
        var rb = Instantiate(toast,toastPosition.position,quaternion.identity,null).GetComponent<Rigidbody>();
        rb.AddForce((transform.forward+Vector3.up*0.4f)*toastPower);
        // anim.SetTrigger("ExitIceAttack");
        // CanMove();
       
    }

    // private IEnumerator DestroyRB(GameObject rb)
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     Destroy(rb);
    // }

    // public void ActivateAbility()
    // {
    //     var rb = Instantiate(ice,icePosition).GetComponent<Rigidbody>();
    //     var startPos = rb.transform.localPosition.z;
    //     while (startPos < startPos+2*Vector3.forward.z)
    //     {
    //         rb.AddForce(Vector3.forward,ForceMode.Impulse);
    //     }
    //
    //     rb.useGravity = true;
    //     // switch (tpm.CurrentAbility.AbilityName)
    //     // {
    //     //     case StatusEffects.nothing:
    //     //         break;
    //     //     case StatusEffects.elect:
    //     //         Instantiate(electric,electricalPosition);
    //     //         break;
    //     //     case StatusEffects.ice:
    //     //         var rb = Instantiate(ice,icePosition).GetComponent<Rigidbody>();
    //     //         var startPos = rb.transform.localPosition.z;
    //     //         while (startPos < startPos+2*Vector3.forward.z)
    //     //         {
    //     //             rb.AddForce(Vector3.forward,ForceMode.Impulse);
    //     //         }
    //     //
    //     //         rb.useGravity = true;
    //     //         break;
    //     //     case StatusEffects.fire:
    //     //         var rbT = Instantiate(toast,toastPosition).GetComponent<Rigidbody>();
    //     //         var startPosT = rbT.transform.localPosition.z;
    //     //         while (startPosT < startPosT+2*Vector3.forward.z)
    //     //         {
    //     //             rbT.AddForce(Vector3.forward,ForceMode.Impulse);
    //     //         }
    //     //         rbT.useGravity = true;
    //     //         break;
    //     //
    //     // }
    // }
    
}
