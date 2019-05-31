using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour { 
    public static Player Instance { get; private set; }
    public RawImage hurtImage;
    public RawImage fadeImage;
    public float health = 100f;
    public bool dead = false;
    public Animator anim;
    public PlayerController playerController;
    public ProjectileModifiers projectileMods;

    public bool canDie = true;
    public bool dashing = true;
    public bool canDash = true;
    public Vector3 lastSpawnPoint = new Vector3(-999,-999,-999);
    public GameObject playerTextObject;
    public bool rechargingDash = false;
    public bool chargingDash = false;
    public int collectables;
    public AudioSource audioSource;
    public Rigidbody2D rb;
    public bool facingRight = true;


    private GameManager gameManager;
    private LevelManager levelManager;

    private  MainCamera cam;

    public Transform camTarget;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else {Destroy(gameObject); }
        //DontDestroyOnLoad(gameObject);
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        projectileMods = new ProjectileModifiers();

        gameManager = GameManager.Instance;
        levelManager = LevelManager.Instance;
        cam = MainCamera.Instance;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();



    }

    private void Start() {
        cam = MainCamera.Instance;

        cam.SetTarget(camTarget);


    }

    
    // Update is called once per frame
    void Update()
    {
    }

  

    public bool Die()
    {
        if (!canDie || dead) return false;
        dead = true;
        chargingDash = false;
        dashing = false;

        //StartCoroutine("DieAnim");
        anim.SetLayerWeight(0, 1);
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetBool("Grounded", true);
        anim.SetBool("Charging", false);
        anim.SetTrigger("Die");

        return true;
    }

    IEnumerator DieAnim()
    {
        anim.SetLayerWeight(0,1);
        anim.SetLayerWeight(1,0);
        anim.SetLayerWeight(2,0);
        anim.SetBool("Grounded", true);
        anim.SetTrigger("Die");
       
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(.5f);
    }

    public void Died() {
        anim.SetBool("Grounded", true);
        gameManager.KillPlayer();
    }

    public void Reset()
    {
        // called whenever the player spawns
        anim = GetComponent<Animator>();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Collider2D>().enabled = true;
        MainCamera.Instance.transform.position = transform.position;
        anim.SetLayerWeight(0, 1);
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetFloat("Speed", 0f);
        transform.rotation = Quaternion.identity;
        health = 100;
        anim.SetBool("Grounded", false);
        dashing = false;
        canDie = true;

    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public ProjectileModifiers GetProMods() {

        return projectileMods;
    }
}
