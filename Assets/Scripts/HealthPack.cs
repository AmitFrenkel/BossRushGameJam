using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthPack : MonoBehaviour
{
    public enum TypeOfPickup
    {
        healthPack,
        Chip
    }
    [SerializeField] private TypeOfPickup type;
    [SerializeField] private UnityEvent todoAfterPickup;
    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case TypeOfPickup.healthPack:
                if (other.CompareTag("Player"))
                {
                    other.transform.parent.parent.GetComponent<Player>().ChangeHealth(5);
                    Destroy(gameObject);
                }
                break;
            case TypeOfPickup.Chip:
                if (other.CompareTag("Player"))
                {
                    todoAfterPickup.Invoke();
                }
                break;
            default:
                break;
        }
    }
}
