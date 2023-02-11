using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int knockbackRange;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.transform.parent.parent.GetComponent<Player>();
            p.ChangeHealth(-damage);
            p.GetComponent<KnockBack>().GiveKnockBack(transform,knockbackRange);
        }
    }
}
