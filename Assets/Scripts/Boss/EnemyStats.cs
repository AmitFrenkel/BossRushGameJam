using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum StatusEffects
{
    nothing,
    elect,
    ice,
    fire,
    iced,
    onFire
}
public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool isBoss;
    [SerializeField] protected WarnBehavior warnBehavior;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthpackPrefabs;
    [SerializeField] protected Animator anim;
    private int lastHealthNum;
    public UnityEvent deathToDo;

    public StatusEffects currentStatus;
    private float iceEffectCap = 10;
    private float iceEffectAmount = 0;
    private float fireEffectCap = 100;
    private float fireEffectAmount = 0;
    private float fireAddAmount = 33;
    private void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        lastHealthNum = (int)health / 15;
        fireEffectAmount = 50;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            print(fireEffectAmount);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            iceEffectAmount = 9;
            AddEffect(StatusEffects.ice);
        }

        if (fireEffectAmount != 0)
        {
            fireEffectAmount -= 1;
            ChangeHealth(-0.2f);
        }
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

    public void AddEffect(StatusEffects status)
    {
        switch (currentStatus)
        {
            case StatusEffects.nothing:
                currentStatus = status;
                if (currentStatus == StatusEffects.ice)
                {
                    iceEffectAmount++;
                }
                break;
            case StatusEffects.elect:
                break;
            case StatusEffects.ice:
                if (currentStatus == StatusEffects.ice)
                {
                    iceEffectAmount += 1;
                    if (iceEffectAmount == iceEffectCap)
                    {
                        iceEffectAmount = 0;
                        anim.SetTrigger("Iced");
                        //Do stun
                    }
                }
                else if (currentStatus == StatusEffects.fire)
                {
                    iceEffectAmount = 0;
                    currentStatus = StatusEffects.nothing;
                }
                break;
            case StatusEffects.fire:
                if (currentStatus == StatusEffects.fire)
                {
                    fireEffectAmount = Mathf.Min(fireEffectAmount+fireAddAmount,fireEffectCap);
                }
                else if (currentStatus == StatusEffects.ice)
                {
                    fireEffectAmount = 0;
                    currentStatus = StatusEffects.nothing;
                }
                break;
            case StatusEffects.iced:
                break;
            case StatusEffects.onFire:
                break;
            default:
                break;
        }
    }
}
