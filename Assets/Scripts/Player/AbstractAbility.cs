using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    // [SerializeField] protected Animator animator;
    [SerializeField] protected StatusEffects abilityName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected bool canUse;
    [SerializeField] protected float power;

    public bool CanUse
    {
        get => canUse;
        set => canUse = value;
    }
    

    public StatusEffects AbilityName
    {
        get => abilityName;
        set => abilityName = value;
    }

    public float Power
    {
        get => power;
        set => power = value;
    }

    // public virtual void ActivateAbility()
    // {
    //     // if (currentTimer == cooldownTimer)
    //     // {
    //     //     StartCoroutine(StartCooldown());
    //     //     // animator.Play(abilityName);
    //     // }
    // }
    
    
    
}
