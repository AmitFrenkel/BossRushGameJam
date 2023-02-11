using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHelper : MonoBehaviour
{
    [SerializeField] private GameObject trails;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanMove()
    {
        CombSystem.EnableMoving();
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
