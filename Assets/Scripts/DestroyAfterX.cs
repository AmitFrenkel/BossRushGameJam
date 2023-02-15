using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{
    [SerializeField] private float destructionTimer;
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }
    public IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(destructionTimer);
        Destroy(gameObject);
    }
}
