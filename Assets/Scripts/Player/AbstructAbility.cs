using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstructAbility : MonoBehaviour
{
    protected string abilityName;
    protected float cooldownTimer = 1;
    protected Sprite icon;
    protected bool canUse;
}
