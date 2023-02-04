using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected string abilityName;
    protected float cooldownTimer = 1;
    protected Sprite icon;
    protected bool canUse;
    private float currentTimer;

    private void Start()
    {
        currentTimer = cooldownTimer;
    }

    public virtual void ActivateAbility()
    {
        if (currentTimer == cooldownTimer)
        {
            StartCoroutine(StartCooldown());
            animator.Play(abilityName);
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
