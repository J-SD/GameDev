using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 10f;
    bool joystick = false;
    Player player;


    private float wallGravityScale = 1f;

    //sets up the grounded stuff
    bool grounded = false;
    bool touchingWall = false;
    bool noClimb = false;
    public Transform groundCheck;
    public Transform wallCheck;
    float wallTouchRadius = 0.2f;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public LayerMask climbMask;
    public LayerMask noClimbMask;
    public float jumpForce = 700f;
    public float jumpPushForce = 10f;


    //private BoxCollider2D col;
    private CapsuleCollider2D col;
    private SpriteRenderer spriteRenderer;
    private Vector2 groundCheckBoxSize = new Vector2(.2f, .3f);
    private Rigidbody2D rb;
    //private float climbCooldown = 1f;
    //private float lastClimbTime = 0f;

    public bool wallTop = false;
    private bool ledge = false;


    private bool climbingLedge = false;
    Vector2 startColSize;

    private float ledgeGrabTime = 0f;
    public float ledgeGrabInterval = .5f;
    private float wallGrabForce = 20f;
    private float wallMoveSpeedMultiplier = .8f;
    private float runSpeedMultiplier = 1.2f;
    private MainCamera cam;

    bool crouching = false;
    float crouchSpeedMod = .8f;

    public GameObject feetEffect;
    public GameObject landEffect;

    private float jumpVelocity = 7f;
    private float fallMulitplier = 2.5f;
    private float lowJumpMultiplier = 2.5f;

    public PlayerDash playerDash;

    Animator anim;
    private void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
        anim = player.anim;
        col = GetComponent<CapsuleCollider2D>();
        //col = GetComponent<BoxCollider2D>();
        startColSize = col.size;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = player.rb;
        cam = MainCamera.Instance;
        playerDash = GetComponent<PlayerDash>();

    }

    bool collisionGround;
    void OnCollisionStay2D(Collision2D c)
    {
        if ((groundMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            collisionGround = true;
    }
    private void OnCollisionExit2D(Collision2D c)
    {
        if ((groundMask.value & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
            collisionGround = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.dead || climbingLedge) return; // dont do input stuff

        player.canDie = !player.dashing; // maybe change


        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.OverlapBox(groundCheck.position, groundCheckBoxSize, 0f, groundMask);
        grounded = grounded && collisionGround;
        if (grounded & !anim.GetBool("Grounded")) Instantiate(landEffect, groundCheck.position, Quaternion.identity);


        anim.SetBool("Grounded", grounded);

        // Update the position of the parent object to be exactly where the sprite is 
        transform.position = spriteRenderer.transform.position;

        // we are touching the wall if the wallcheck object is over a wall and we are not in a no climb zone
        noClimb = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, noClimbMask);
        touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, wallMask) && !noClimb;

        if (!playerDash.dashCharged && (grounded || touchingWall)) playerDash.dashCharged = true; // if we just dashed and now we're on the ground again. The player can only dash after they have touched the ground since the last dash

        // TODO : come on man dont do this every frame
        anim.SetLayerWeight(1, touchingWall ? 1 : 0 * 1f);


        // the collider for when we reach the top of the wall 
        Collider2D climbCol = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, climbMask);

        // we are on a ledge if we are on a wall and have connected with a climb col, and we are not in a no climb zone 
        ledge = climbCol != null && touchingWall && !noClimb;


        // Check if there is more wall right above us, if not, we can ledge climb TODO NEW
        Collider2D hit = Physics2D.OverlapCircle(wallCheck.position + Vector3.up *( wallTouchRadius * 2), wallTouchRadius, wallMask);
        ledge = !hit && touchingWall;
        anim.SetBool("Ledge", ledge);


        float moveY = 0f; // initialize to 0 
        col.size = new Vector2(startColSize.x, startColSize.y);

       
        // if we are on a wall but not a ledge 
        if (touchingWall && !noClimb /*&& !ledge*/)
        {
            moveY = Input.GetAxis("Vertical"); // handle vertical input

            rb.gravityScale = wallGravityScale; // adjust gravity scale
            grounded = false;// we arent grounded

            // add force so player clings to wall 
            if (player.facingRight) rb.AddForce(new Vector2(wallGrabForce, 0f));
            else rb.AddForce(new Vector2(-wallGrabForce, 0f));

            // shrink the collider so that the player can get closer to the wall 
            col.size = new Vector2(startColSize.x * 1.3f, startColSize.y);
            //col.size = new Vector2(startColSize.x * .8f, startColSize.y);

            // remove cam jitter?
            cam.ToggleSmoothPlayerFollow(false);

        }
        else if (!ledge && !touchingWall) { // not touching a wall or a ledge
            // reset physics for ground/ air movement
            rb.gravityScale = 1f;
            col.size = startColSize;
            //cam.ToggleSmoothPlayerFollow(true);

        }

        // do movement

      

        if (ledge) // on a ledge, do ledge climbing animation and turn off physics
        {  
               
            //rb.gravityScale = 0f;
            //rb.velocity = Vector3.zero;
            if (Time.time > ledgeGrabTime + ledgeGrabInterval) { 
                if (Input.GetAxis("Vertical") > .1f)
                {
                    //up
                    //anim.SetTrigger("ClimbLedge");
                    //rb.AddForce(Vector2.up * 40f);
                    //if (facingRight) rb.AddForce(Vector2.right * 20f);
                    //else rb.AddForce(Vector2.left * 20f);
                    //climbingLedge = true;
                    //anim.applyRootMotion = true;
                    StartCoroutine("ClimbLedge");

                }
                else if (Input.GetAxis("Vertical") < -.1f) // the player wants to go off the ledge down
                {
                    //down
                    rb.gravityScale = wallGravityScale;
                    rb.AddForce(Vector2.down*10f);

                } } 

        }

        #region //move
        float move = Input.GetAxis("Horizontal");
        if (Input.GetButton("Run")) move *= runSpeedMultiplier;
        if (crouching) move *= crouchSpeedMod;

        anim.SetFloat("Speed", Mathf.Abs(move));


        if (!player.dashing && !player.chargingDash)
        {

            if (!touchingWall) // in air or on ground, move x
                rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
            else if (!ledge) // on wall but not on ledge, move y
                rb.velocity = new Vector2(rb.velocity.x, moveY * maxSpeed * wallMoveSpeedMultiplier);
        }
        else{
            anim.SetFloat("Speed",0f);


        }

        anim.SetFloat("VerticalSpeed", rb.velocity.y);
        // ground movement particles
        if (grounded && Mathf.Abs(rb.velocity.x) >9f && player.canDash && !player.chargingDash) {
            Instantiate(feetEffect, groundCheck.position, Quaternion.identity);
        }

       
        #endregion

       
        


        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !player.facingRight)
        {
            // ... flip the player.
            Flip();
        }// Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && player.facingRight)
        {
            // ... flip the player.
            Flip();
        }

        

    }

    IEnumerator ClimbLedge() {

        rb.AddForce(Vector2.up * 20f);
        col.size = new Vector2 (col.size.x, col.size.y / 2);
        yield return new WaitForSeconds(.3f);
        grounded = false;
        col.size = new Vector2(col.size.x, col.size.y * 2);

        //if (facingRight) rb.AddForce(Vector2.right * 100f);
        //else rb.AddForce(Vector2.left * 100f);
    }


    public void LedgeClimbComplete()
    {
        climbingLedge = false;
    }

    bool doubleJump = true;
    public bool doingDoubleJump = false;

    void Update()
    {
        joystick = Input.GetJoystickNames().Length > 0 && !Input.anyKey;


        if (player.dead || climbingLedge) return;

        anim.applyRootMotion = false;

        // jump
        if (joystick) {
            if ((Input.GetButtonDown("Jump")))
            {
                if (grounded)
                {
                    rb.velocity = Vector2.up * jumpVelocity;
                    Instantiate(landEffect, groundCheck.position, Quaternion.identity);
                    doubleJump = true;
                }
                else if (doubleJump) {
                    doubleJump = false;
                    rb.velocity = Vector2.up * jumpVelocity;
                    doingDoubleJump = true;


                }
            }
        }
        else if (Input.GetButtonDown("Jump") || Input.GetButtonDown("UpArrow"))
        {
            if (grounded)
            {
                rb.velocity = Vector2.up * jumpVelocity;
                Instantiate(landEffect, groundCheck.position, Quaternion.identity);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                doubleJump = false;
                rb.velocity = Vector2.up * jumpVelocity;
                doingDoubleJump = true;

            }
        }

        if (touchingWall || grounded)
        {
            doubleJump = true;
            doingDoubleJump = false;

        }


        if (rb.velocity.y < 0) // we are going down, increase gravity
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMulitplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !(Input.GetAxis("Vertical") > 0f)) //  we are going up and not holding the jump key
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        if (grounded && Input.GetButtonDown("Crouch")) {
            ToggleCrouch();
        }

        if (touchingWall && Input.GetButtonDown("Jump"))
        {
            doubleJump = true;
            WallJump();
        }

        anim.SetBool("DoubleJumping", doingDoubleJump);
        // do dash stuff:
        playerDash.CheckForDash();
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (player.dashing)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.defeatable)
                {
                    enemy.Defeat();
                }
            }

        }
    }


    void ToggleCrouch() {
        crouching = !crouching;
        anim.SetBool("Crouch", crouching);
    }

    void WallJump()
    {
        float thisJumpForceX = jumpPushForce;
        if (player.facingRight) {
            thisJumpForceX *= -1;
        }
        rb.AddForce(new Vector2(thisJumpForceX, jumpForce/2f));
        Instantiate(landEffect, wallCheck.position, Quaternion.identity);

    }

    void Flip()
    {
        player.playerTextObject.transform.parent = null;
        // Switch the way the player is labelled as facing
        player.facingRight = !player.facingRight;

        //Multiply the player's x local cale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        player.playerTextObject.transform.parent = transform;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckBoxSize.x, groundCheckBoxSize.y, 0f));
        Gizmos.DrawWireSphere(wallCheck.position, wallTouchRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector3(Physics2D.gravity.x, Physics2D.gravity.y, 0f));
        Gizmos.DrawWireSphere(wallCheck.position + Vector3.up * (wallTouchRadius *2), wallTouchRadius);
    }


}
