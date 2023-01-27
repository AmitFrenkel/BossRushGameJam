using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orienation : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Update()
    {
        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}
