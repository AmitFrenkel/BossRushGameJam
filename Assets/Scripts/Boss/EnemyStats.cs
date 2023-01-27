using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health;

    public void ChangeHealth(int amount)
    {
        health += amount;

        if (health <= 0)
        {

        }
    }
}
