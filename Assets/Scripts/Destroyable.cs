using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private int attackAmounts;

    private void OnEnable()
    {
        attackAmounts = 2;
    }
    public void ReduceHealth()
    {
        attackAmounts--;
        print(attackAmounts);
        if (attackAmounts <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
