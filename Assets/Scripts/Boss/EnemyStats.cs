using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool isBoss;
    [SerializeField] protected WarnBehavior warnBehavior;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthpackPrefabs;
    private int lastHealthNum;
    public UnityEvent deathToDo;
    private void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        lastHealthNum = (int)health / 15;
    }
    public void ChangeHealth(float amount)
    {
        health += amount;
        if (health <= 0)
        {
            health = 0;
            deathToDo.Invoke();
        }
        int t = (int)health / 15;
        if (t != lastHealthNum)
        {
            lastHealthNum = t;
            Rigidbody rb = Instantiate(healthpackPrefabs, transform.position, Quaternion.identity, null).GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 500);
            rb.AddForce(Vector3.right * Random.Range(-500,501));
            rb.AddForce(Vector3.forward * Random.Range(-500,501));
        }

        if (isBoss)
            healthBar.value = health;

    }
}
