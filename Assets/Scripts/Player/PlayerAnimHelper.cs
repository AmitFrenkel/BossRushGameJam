using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHelper : MonoBehaviour
{
    [SerializeField] private GameObject trails;
    [SerializeField] private CombSystem comb;
    public void EnableCollider()
    {
        comb.ToggleCollider(true);
    }
    public void DisableCollider()
    {
        comb.ToggleCollider(false);
    }

    public void CanMove()
    {
        CombSystem.EnableMoving();
    }
    public void ForceTowardsEnemy()
    {
        comb.ForceForward();
    }

    public void EnableTrails()
    {
        trails.SetActive(true);
    }
    public void DisableTrails()
    {
        trails.SetActive(false);
    }
}
