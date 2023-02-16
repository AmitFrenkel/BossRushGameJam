using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodgeRoll : MonoBehaviour
{
    public float dodgeSpeed = 5f;
    public float dodgeDuration = 0.5f;
    public float iframeDuration = 0.5f;

    private Rigidbody rigidbody;
    private float dodgeElapsedTime = 0f;
    private float iframeElapsedTime = 0f;
    public static bool canBeDamaged = true;
    private bool isRolling;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform dodgeHelper;
    [SerializeField] private Material reg;
    [SerializeField] private Material blue;
    [SerializeField] private SkinnedMeshRenderer renderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            animator.SetTrigger("Dodge");
            StartCoroutine(StartRoll());

            rigidbody.AddForce(dodgeHelper.forward * dodgeSpeed, ForceMode.Impulse);
        }
    }
    private IEnumerator StartRoll()
    {
        CombSystem.canMove = false;
        //renderer.material = blue;
        canBeDamaged = false;
        yield return new WaitForSeconds(0.1f);
        //renderer.material = reg;
        canBeDamaged = true;
        isRolling = true;
        while (rigidbody.velocity.magnitude > 0.2f)
        {
            yield return null;
        }
        isRolling = false;
        CombSystem.canMove = true;
    }
}