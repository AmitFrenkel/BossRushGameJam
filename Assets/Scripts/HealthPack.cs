using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.parent.GetComponent<Player>().ChangeHealth(5);
            Destroy(gameObject);
        }
    }
}
