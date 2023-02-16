using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingForward : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    public void Straigthen()
    {
        transform.rotation = Quaternion.identity;
    }
    public void DestroyPlease()
    {
        Destroy(gameObject);
    }
}
