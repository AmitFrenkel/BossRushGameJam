using UnityEngine;

public class DodgeRoll : MonoBehaviour
{
    public float dodgeSpeed = 5f;
    public float dodgeDuration = 0.5f;
    public float iframeDuration = 0.5f;

    private Rigidbody rigidbody;
    private float dodgeElapsedTime = 0f;
    private float iframeElapsedTime = 0f;
    public static bool canBeDamaged = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeElapsedTime + iframeDuration > iframeElapsedTime)
        {
            print("DODGE");
            dodgeElapsedTime = 0f;
            iframeElapsedTime = 0f;

            Vector3 dodgeDirection = transform.forward * Input.GetAxisRaw("Horizontal") + transform.right * Input.GetAxisRaw("Vertical");
            rigidbody.AddForce(dodgeDirection * dodgeSpeed, ForceMode.Impulse);
        }

        dodgeElapsedTime += Time.deltaTime;
        iframeElapsedTime += Time.deltaTime;

        if (iframeElapsedTime >= iframeDuration)
        {
            canBeDamaged = false;
        }
        else
        {
            canBeDamaged = true;
        }
    }
}