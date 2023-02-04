using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Warn : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] sprites;
    public void Set(Transform settingTarget)
    {
        //transform.localScale = Vector3.zero;
        target = settingTarget;
        startPos = target.position;
    }
    public void SetOnGround(Transform settingTarget)
    {
        //transform.localScale = Vector3.zero;
        target = settingTarget;
        startPos = target.position;
        transform.position = new Vector3(target.position.x, 0.6043496f, target.position.z);
    }
    private void FixedUpdate()
    {
        float startDistance = Vector3.Distance(transform.position, startPos);
        float distance = Vector3.Distance(transform.position, target.position);
        float i = 100*((startDistance + (startDistance - distance)) / startDistance)-100;
        renderer.sprite = sprites[(int)i];
        //transform.localScale = new Vector3((startDistance + (startDistance - distance)) / startDistance, (startDistance + (startDistance - distance)) / startDistance, 1);
    }
}
