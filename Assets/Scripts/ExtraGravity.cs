using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [SerializeField] private float gravity;
    private Rigidbody rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rg.AddForce(Vector3.down * gravity * Time.deltaTime);
    }
}
