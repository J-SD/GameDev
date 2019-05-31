using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set;}

    public GameObject gameManagerPrefab;
    public AudioClip levelTrack;
    public float loopFadeTime;
    public bool loopFade;
    public bool cameraFollowsPath = false;
    public float cameraPathSpeed = 1f;
    public MainCamera cam;
    public Player player;
    public GameObject background;
    public Vector3 playerSpawn;
    public float camSize;


    public int level;
    public bool levelHasPlayer = true;

    public virtual void Awake() {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        
    }


    public virtual void Start()
    {
        if (!GameManager.Instance)
        {
            GameObject go = Instantiate(gameManagerPrefab, Vector3.zero, Quaternion.identity);
        }


        cam = MainCamera.Instance;
        player = Player.Instance;
        cameraFollowsPath = false;
        cam.ChangeSize(camSize);
        //if (player.canDash) player.dashing = true;

        


    }

    public virtual void Update()
    {
        if (!player) player = Player.Instance;
    }

    public virtual void NotifyOfPlayerRespawn() { }
}
