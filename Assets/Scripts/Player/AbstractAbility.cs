using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected StatusEffects abilityName;
    protected float cooldownTimer = 1;
    protected Sprite icon;
    protected bool canUse;
    protected int power;
    private float currentTimer;

    public bool CanUse
    {
        get => canUse;
        set => canUse = value;
    }

    private void Start()
    {
        currentTimer = cooldownTimer;
    }

    public StatusEffects AbilityName
    {
        get => abilityName;
        set => abilityName = value;
    }

    public int Power
    {
        get => power;
        set => power = value;
    }

    public virtual void ActivateAbility()
    {
        if (currentTimer == cooldownTimer)
        {
            StartCoroutine(StartCooldown());
            // animator.Play(abilityName);
        }
    }

    private IEnumerator StartCooldown()
    {
        canUse = false;
        while (currentTimer < 0)
        {
            yield return new WaitForSeconds(1f);
        }

        canUse = true;
        currentTimer = cooldownTimer;
    }
    
    
}
