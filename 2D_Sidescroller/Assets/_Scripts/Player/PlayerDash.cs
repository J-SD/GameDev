using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(Player))]
public class PlayerDash : MonoBehaviour
{

    public float startDashTime = 0f;
    public float baseDashSpeed = 20;
    public float maxDashSpeed = 80;
    public float baseCameraImpulseAmt = 6f;


    public GameObject dashEffect;
    public GameObject chargeEffect;
    public float dashCooldown;
    public bool dashCharged = true;



    private CapsuleCollider2D dashCol;
    float dashTime;
    float lastDash;
    Rigidbody2D rb;
    Vector2 dashDirection;

    private float currentDashSpeed;
    private float cameraImpulseAmt;
    private float startSpeedModifier;



    Player player;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        rb = player.rb;

        dashCol = GetComponents<CapsuleCollider2D>()[1];
        dashCol.enabled = false;
        lastDash = -999f;
        if (player.canDash) dashTime = startDashTime;


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CheckForDash()
    {
        if (!player.dashing)
        {
            if (Input.GetButtonDown("Dash") && player.canDash)
            { // starting charge
                // charge
                //if (Time.time - lastDash >= dashCooldown) // after cooldown
                if (dashCharged)
                {
                    // freeze the player to allow for charge
                    rb.velocity = Vector2.zero;
                    player.chargingDash = true;
                    player.anim.SetBool("Charging", player.chargingDash);

                }
            }
            else if (player.chargingDash && Input.GetButtonUp("Dash")) //we were charging and we let go of dash button 
            {
                // dash!
                player.dashing = true;
                lastDash = Time.time;
                player.chargingDash = false;
                dashCharged = false;
                player.anim.SetBool("Charging", player.chargingDash);
                player.audioSource.PlayOneShot(player.audioSource.clip);

            }
            else if (player.chargingDash)
            { // we are holding the dash button
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;

                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                dashDirection = new Vector2(x * 100f, y * 100f).normalized;
                if (dashDirection == Vector2.zero) // no dash in place
                {

                    if (player.facingRight) dashDirection = Vector2.right;
                    else dashDirection = Vector2.left;
                }

                GameObject chargeParticles = Instantiate(chargeEffect, transform.position, Quaternion.identity);
                ParticleSystem sparkles = chargeParticles.GetComponent<ParticleSystem>();
                var main = sparkles.main;
                var emission = sparkles.emission;
                main.startSpeedMultiplier = 3*startSpeedModifier;
                main.startLifetimeMultiplier = startSpeedModifier*.1f;
                emission.rateOverTimeMultiplier = startSpeedModifier*100f;
                sparkles.Play();
                chargeParticles.transform.rotation = Quaternion.FromToRotation(transform.right, dashDirection);
            }

        }
        else // dash (player is dashing)
        {
            dashCol.enabled = true;

            if (dashTime <= 0)
            {
                rb.velocity = Vector2.zero;
                // this is hacky but it fixes a problem where the player stops dashing right before colliding with an enemy and dies
                // while technically not wrong, it feels like the player is cheated so we give them another bit of invunerability after they stop moving
                if (dashTime <= -.1f)
                //if (dashTime <= 0f)
                {
                    dashTime = startDashTime;

                    player.dashing = false;
                    dashCol.enabled = false;


                }
                else dashTime -= Time.deltaTime;
            }

            else// if (nonZeroDashDirection !=outOfBounds) // we are in the dash
            {

                // do dash
                CalculateDashValues();
                GetComponent<CinemachineImpulseSource>().GenerateImpulse(Vector3.right * cameraImpulseAmt);
                rb.gravityScale = 1f;
                rb.velocity = dashDirection * currentDashSpeed;

                // plus sparkles
                GameObject sparklesGO = Instantiate(dashEffect, new Vector2(transform.position.x, transform.position.y) + dashDirection /*nonZeroDashDirection*/ * .5f, Quaternion.identity);
                ParticleSystem sparkles = sparklesGO.GetComponent<ParticleSystem>();
                //max: Start speed 4-8
                var main = sparkles.main;
                var emission = sparkles.emission;
                main.startSpeedMultiplier = 4 * startSpeedModifier;
                main.startLifetimeMultiplier = startSpeedModifier * .2f;
                emission.rateOverTimeMultiplier = startSpeedModifier * 5f;
                sparkles.Play();
                dashTime -= Time.deltaTime; // count this down to 0
            }
        }

        if (!(Time.time - lastDash >= dashCooldown))
        {
            player.rechargingDash = true;

        }
        else { player.rechargingDash = false; }

    }

    public void CalculateDashValues() {
        int collectables = GameManager.Instance.collectables;
        float modifier = .3f + (collectables * .03f); // here should be roughly the total number of collectables

        //if (modifier > 1f) modifier = 1f;

        /****************/
        modifier = 2f; // TODO, JUST FOR TESTING, REMOVE
        /****************/

        currentDashSpeed = baseDashSpeed * modifier;
        cameraImpulseAmt = baseCameraImpulseAmt * modifier;
        startSpeedModifier = modifier;
        GameManager.Instance.powerMultiplier = modifier; // USE THIS ONE FOR PROGESSION
    }


}
