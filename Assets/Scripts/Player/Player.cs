using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Player class
public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth,currentHealth;
    [SerializeField] private string shortRangeAbility, longRangeAbility;
    private AbstractAbility[] specialAbility;

    private void Start()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        shortRangeAbility = PlayerPrefs.GetString("shortRangeAbility");
        longRangeAbility = PlayerPrefs.GetString("longRangeAbility");
        specialAbility = JsonUtility.FromJson<AbstractAbility[]>(PlayerPrefs.GetString("specialAbility"));
    }

    public void IncreaseMacHealth(int value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        if (DodgeRoll.canBeDamaged)
        {
            currentHealth += value;

        }
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

    private void Update()
    {
        PlayerPrefs.SetString("shortRangeAbility",shortRangeAbility);
        PlayerPrefs.SetString("longRangeAbility",longRangeAbility);
        PlayerPrefs.SetString("specialAbility",JsonUtility.ToJson(specialAbility));
        PlayerPrefs.SetInt("maxHealth",maxHealth);
    }

}
