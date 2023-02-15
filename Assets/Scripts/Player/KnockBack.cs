using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void GiveKnockBack(Transform item, float speed)
    {
        CombSystem.canMove = false;
        float distance = Mathf.Max(5, 5-Vector3.Distance(transform.position, item.position));
        rigidbody.AddForce(new Vector3(transform.position.x - item.position.x, 0, transform.position.z - item.position.z) * distance*speed);
        StartCoroutine(EnableMovement());
    } 
    public IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(0.5f);
        CombSystem.canMove = true;
    }
}
