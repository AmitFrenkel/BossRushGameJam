using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Player class
public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private string shortRangeAbility, longRangeAbility, specialAbility;

    private void Start()
    {
        health = PlayerPrefs.GetInt("health");
        shortRangeAbility = PlayerPrefs.GetString("shortRangeAbility");
        longRangeAbility = PlayerPrefs.GetString("longRangeAbility");
        specialAbility = PlayerPrefs.GetString("specialAbility");
    }

    public int Health
    {
        get => health;
        set => health = value;
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

    public string SpecialAbility
    {
        get => specialAbility;
        set => specialAbility = value;
    }

    private void Update()
    {
        PlayerPrefs.SetString("shortRangeAbility",shortRangeAbility);
        PlayerPrefs.SetString("longRangeAbility",longRangeAbility);
        PlayerPrefs.SetString("specialAbility",specialAbility);
        PlayerPrefs.SetInt("health",health);
    }
}
