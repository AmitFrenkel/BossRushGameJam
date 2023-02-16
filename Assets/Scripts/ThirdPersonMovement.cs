using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    private AbstractAbility[] abilities;
    [SerializeField] private Player player;
    [SerializeField] private GameObject beam;
    [SerializeField] private Material beamMaterial;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody rigidBody;
    private bool isGrounded;
    [SerializeField] private Animator anim;
    [SerializeField] private MultiAimConstraint headConstraint;
    [SerializeField] private Transform enemyNeck;
    [SerializeField] private Transform cameraFront;
    [SerializeField] private Image[] abilitiesImages;
    [SerializeField] private Sprite[] abilitiesSprites;
    [SerializeField] private Transform laserPos;
    [SerializeField] private Slider energyBar;

    private AbstractAbility currentAbility;
    private int currentAbilityIndex;


    public AbstractAbility CurrentAbility
    {
        get => currentAbility;
        set => currentAbility = value;
    }

    public Player Player
    {
        get => player;
        set => player = value;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        abilities = player.SpecialAbility;
        currentAbility = abilities[0];
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].CanUse)
            {
                abilitiesImages[i].sprite = abilities[i].Icon;
                abilitiesImages[i].color = new Color(abilitiesImages[i].color.r,
                    abilitiesImages[i].color.g, abilitiesImages[i].color.b, 0.1294118f);
            }
            else
            {
                abilitiesImages[i].sprite = abilitiesSprites[i];

            }
            
        }

        abilitiesImages[currentAbilityIndex].color = new Color(abilitiesImages[currentAbilityIndex].color.r,
            abilitiesImages[currentAbilityIndex].color.g, abilitiesImages[currentAbilityIndex].color.b, 1);
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        
        if (vertical == 0 && horizontal == 0)
        {
            anim.SetInteger("State", 0);
        }
        else
            anim.SetInteger("State", 1);

        if (!CombSystem.canMove)
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right = right.normalized;

        Vector3 direction = forward * vertical + right * horizontal;
        if (CombSystem.canMove)
        {
            rigidBody.velocity = direction * speed + Vector3.up * rigidBody.velocity.y;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetInteger("State", 2);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z);
        }
        else if (!isGrounded && rigidBody.velocity.y > 0)
        {
            rigidBody.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        // Change ability
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            abilitiesImages[currentAbilityIndex].color = new Color(abilitiesImages[currentAbilityIndex].color.r,
                abilitiesImages[currentAbilityIndex].color.g,abilitiesImages[currentAbilityIndex].color.b,0.1294118f);
            currentAbilityIndex++;
            if (currentAbilityIndex == abilities.Length)
            {
                currentAbilityIndex = 0;
            }

            while (!abilities[currentAbilityIndex].CanUse)
            {
                currentAbilityIndex++;
                if (currentAbilityIndex == abilities.Length)
                {
                    currentAbilityIndex = 0;
                    break;
                }
            }
            abilitiesImages[currentAbilityIndex].color = new Color(abilitiesImages[currentAbilityIndex].color.r,
                abilitiesImages[currentAbilityIndex].color.g,abilitiesImages[currentAbilityIndex].color.b,1);
            currentAbility = abilities[currentAbilityIndex];

        }
        
        // short Range ability in comboSystem script
        // dodge ability in DodgeRoll script

        //Long range ability
        if (Input.GetKeyDown(KeyCode.Mouse1) && energyBar.value > 0)
        {
            beam.SetActive(true);
            CombSystem.canMove = false;
            CameraController.goForward = true;
            // StartCoroutine(StartOrStopBeam(true));
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // StartCoroutine(StartOrStopBeam(false));
            beam.SetActive(false);
            CameraController.goForward = false;
            CombSystem.canMove = true;
            StartCoroutine(LaserCharge());
        }
        else if (beam.activeSelf && energyBar.value == 0)
        {
            beam.SetActive(false);
        }
        RaycastHit hit;
        if (Input.GetKey(KeyCode.Mouse1) && energyBar.value > 0)
        {
            energyBar.value -= 0.4f;
            if (Physics.Raycast(laserPos.position,laserPos.forward, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(laserPos.position, laserPos.forward);
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<EnemyStats>().ChangeHealth(-0.05f);
                }
                //print(hit.collider.gameObject.name);
            }
        }
    }
    public void AddLaserCharge(float charge)
    {
        energyBar.value += charge;
    }
    public IEnumerator LaserCharge()
    {
        yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Mouse1))
        {
            energyBar.value += 2f*Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("IsGrounded", true);
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        anim.SetBool("IsGrounded", false);
    }

    // private IEnumerator StartOrStopBeam(bool start)
    // {
    //     if (start)
    //     {
    //         while (beamMaterial.GetFloat("_DissolveAmmount") > 0.05f)
    //         {
    //             beamMaterial.SetFloat("_DissolveAmmount", -0.001f);
    //             yield return new WaitForSeconds(0.02f);
    //         }
    //     }
    //     else
    //     {
    //         print("111"+beamMaterial.GetFloat("_DissolveAmmount"));
    //         while (beamMaterial.GetFloat("_DissolveAmmount") < 1)
    //         {
    //             beamMaterial.SetFloat("_DissolveAmmount", +0.001f);
    //             yield return new WaitForSeconds(0.02f);
    //         }  
    //         beam.SetActive(false);
    //     }
    // }
}