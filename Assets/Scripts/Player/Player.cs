using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


// Player class
public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth,currentHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private string shortRangeAbility, longRangeAbility;
    private AbstractAbility[] specialAbility;
    [SerializeField] private UnityEvent deathTodo;
    [SerializeField] private int power;

    private void Start()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        shortRangeAbility = PlayerPrefs.GetString("shortRangeAbility");
        longRangeAbility = PlayerPrefs.GetString("longRangeAbility");
        specialAbility = JsonUtility.FromJson<AbstractAbility[]>(PlayerPrefs.GetString("specialAbility"));

        PlayerPrefs.SetString("shortRangeAbility", shortRangeAbility);
        PlayerPrefs.SetString("longRangeAbility", longRangeAbility);
        PlayerPrefs.SetString("specialAbility", JsonUtility.ToJson(specialAbility));
        PlayerPrefs.SetInt("maxHealth", maxHealth);
    }

    public void IncreaseMacHealth(int value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        if (DodgeRoll.canBeDamaged && value < 0)
        {
            currentHealth += value;
            healthBar.value = currentHealth;
            if (currentHealth <= 0)
            {
                deathTodo.Invoke();
            }
        }
        else if (value > 0)
        {
            currentHealth += value;
            healthBar.value = currentHealth;
        }
    }
    public AbstractAbility[] SpecialAbility
    {
        get => specialAbility;
        set => specialAbility = value;
    }

    public int Power
    {
        get => power;
        set => power = value;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public string ShortRangeAbility
    {
        get => shortRangeAbility;
        set => shortRangeAbility = value;
    }

    public string LongRangeAbility
    {
        get => longRangeAbility;
        set => longRangeAbility = value;
    }

    // public string SpecialAbility
    // {
    //     get => specialAbility;
    //     set => specialAbility = value;
    // }

}
