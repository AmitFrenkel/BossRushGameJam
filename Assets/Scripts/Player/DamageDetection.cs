using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int knockbackRange;
    [SerializeField] private Transform originOverride;
    [SerializeField] private bool canDoDamage = true;

    private void OnEnable()
    {
        canDoDamage = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDoDamage)
        {
            canDoDamage = false;

            Player p = other.transform.parent.parent.GetComponent<Player>();
            p.ChangeHealth(-damage);
            if (originOverride)
                p.GetComponent<KnockBack>().GiveKnockBack(originOverride, knockbackRange);
            else
                p.GetComponent<KnockBack>().GiveKnockBack(transform, knockbackRange);
        }
    }
}
