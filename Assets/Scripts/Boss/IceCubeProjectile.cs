using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IceCubeProjectile : MonoBehaviour
{
    private MeshRenderer renderer;
    public UnityEvent todo;
    public GameObject warnHold;
    public GameObject effect;
    public GameObject dangerArea;
    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            todo.Invoke();
        }
    }
    public void HideWarn()
    {
        warnHold.SetActive(false);
    }
    public void CreateAOE()
    {
        StartCoroutine(AOE());
    }
    public IEnumerator AOE()
    {
        effect.SetActive(true);
        dangerArea.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        dangerArea.SetActive(false);
        yield return new WaitForSeconds(0.65f);
        effect.SetActive(false);
        gameObject.SetActive(false);
        renderer.enabled = true;
    }
}
