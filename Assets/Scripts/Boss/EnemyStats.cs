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
    fire
}
public class EnemyStats : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public bool isBoss;
    [SerializeField] protected WarnBehavior warnBehavior;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject healthpackPrefabs;
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] protected Animator anim;
    [SerializeField] private Slider fireStatus;
    [SerializeField] private Slider iceStatus;
    [SerializeField] protected bool waitsForStun;
    private bool once = true;
    private int lastHealthNum;
    public UnityEvent deathToDo;

    [SerializeField] private StatusEffects currentStatus;
    private int iceEffectCap = 10;
    private int iceEffectAmount = 0;
    private float fireEffectCap = 100;
    private float fireEffectAmount = 0;
    private float fireAddAmount = 33;
    protected void ToDoOnStart()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
        lastHealthNum = (int)health / 15;

        fireStatus.maxValue = fireEffectCap;
        iceStatus.maxValue = iceEffectCap;
    }
    public void ChangeHealth(float amount)
    {
        health += amount;
        if (health <= 0 && once)
        {
            once = false;
            health = 0;
            Rigidbody rb = Instantiate(chipPrefab, transform.position, Quaternion.identity, null).GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 500);
            rb.AddForce(Vector3.right * Random.Range(-500, 501));
            rb.AddForce(Vector3.forward * Random.Range(-500, 501));
            deathToDo.Invoke();
        }
        int t = (int)health / 20;
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
    public IEnumerator CheckOnFire()
    {
        while (fireEffectAmount > 0)
        {
            yield return new WaitForSeconds(0.1f);
            fireEffectAmount -= 1f;
            ChangeHealth(-0.1f);
            fireStatus.value = fireEffectAmount;
        }
        fireEffectAmount = 0;
        fireStatus.gameObject.SetActive(false);
        currentStatus = StatusEffects.nothing;
    }

    public StatusEffects GetCurrentStatus()
    {
        return currentStatus;
    }
    public float GetCurrentIceMultiplier()
    {
        int ices = iceEffectAmount;
        iceEffectAmount = 0;
        currentStatus = StatusEffects.nothing;
        iceStatus.gameObject.SetActive(false);
        return (float)(0.3*ices);
    }
    public void ReduceFire(float fireReduce)
    {
        fireEffectAmount -= fireReduce;
        if (fireAddAmount <= 0)
        {
            fireEffectAmount = 0;
            currentStatus = StatusEffects.nothing;
            fireStatus.gameObject.SetActive(false);
        }
        fireStatus.value = fireEffectAmount;
    }
    protected void ResetIce()
    {
        iceEffectAmount = 0;
        iceStatus.value = iceEffectAmount;
        iceStatus.gameObject.SetActive(false);
        currentStatus = StatusEffects.nothing;
    }
    public void AddEffect(StatusEffects status)
    {
        switch (currentStatus)
        {
            case StatusEffects.nothing:
                if (status == StatusEffects.elect)
                    return;

                currentStatus = status;
                if (currentStatus == StatusEffects.ice)
                {
                    iceEffectAmount++;
                    iceStatus.value = iceEffectAmount;
                    iceStatus.gameObject.SetActive(true);
                }
                if (currentStatus == StatusEffects.fire)
                {
                    fireEffectAmount += fireAddAmount;
                    fireStatus.gameObject.SetActive(true);
                    StartCoroutine(CheckOnFire());
                }
                break;
            case StatusEffects.elect:
                break;
            case StatusEffects.ice:
                if (status == StatusEffects.ice)
                {
                    iceEffectAmount++;
                    iceStatus.value = iceEffectAmount;
                    if (iceEffectAmount == iceEffectCap && !waitsForStun)
                    {
                        //iceEffectAmount = 0;
                        //iceStatus.value = iceEffectAmount;
                        //iceStatus.gameObject.SetActive(false);
                        //currentStatus = StatusEffects.nothing;
                        waitsForStun = true;
                        //Do stun
                    }
                }
                else if (status == StatusEffects.fire)
                {
                    iceEffectAmount = 0;
                    iceStatus.gameObject.SetActive(false);
                    currentStatus = StatusEffects.nothing;
                }
                break;
            case StatusEffects.fire:
                if (status == StatusEffects.fire)
                {
                    fireEffectAmount = Mathf.Min(fireEffectAmount+fireAddAmount,fireEffectCap);
                }
                else if (status == StatusEffects.ice)
                {
                    fireEffectAmount = 0;
                    fireStatus.gameObject.SetActive(false);
                    currentStatus = StatusEffects.nothing;
                }
                break;
            default:
                break;
        }
    }
}