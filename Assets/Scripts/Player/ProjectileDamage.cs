using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (player.CurrentAbility.AbilityName)
                {
                    case StatusEffects.elect:
                        other.transform.GetComponent<EnemyStats>().AddEffect(player.CurrentAbility.AbilityName);
                        switch (other.transform.GetComponent<EnemyStats>().GetCurrentStatus())
                        {
                            case StatusEffects.ice:
                                other.transform.GetComponent<EnemyStats>().ChangeHealth(other.transform.GetComponent<EnemyStats>().GetCurrentIceMultiplier()*-player.CurrentAbility.Power);
                                break;
                            case StatusEffects.fire:
                                other.transform.GetComponent<EnemyStats>().ReduceFire(10);
                                other.transform.GetComponent<EnemyStats>().ChangeHealth(1.5f*-player.CurrentAbility.Power);
                                break;
                            case StatusEffects.nothing:
                                other.transform.GetComponent<EnemyStats>().ChangeHealth(-player.CurrentAbility.Power);
                                break;
                            case StatusEffects.elect:
                                other.transform.GetComponent<EnemyStats>().ChangeHealth(-player.CurrentAbility.Power);
                                break;
                        }
                        break;
                    case StatusEffects.ice:
                        other.transform.GetComponent<EnemyStats>().AddEffect(player.CurrentAbility.AbilityName);
                        other.transform.GetComponent<EnemyStats>().ChangeHealth(-player.CurrentAbility.Power);
                        break;
                    case StatusEffects.fire:
                        other.transform.GetComponent<EnemyStats>().AddEffect(player.CurrentAbility.AbilityName);
                        other.transform.GetComponent<EnemyStats>().ChangeHealth(-player.CurrentAbility.Power);
                        break;
                }
    }
    
}
