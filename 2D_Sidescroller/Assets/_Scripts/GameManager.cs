using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//test
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelManager levelManager;
    public RawImage fadeImage;
    public RawImage hurtImage;
    public GameObject playerPrefab;
    public int level;
    public Animator saveTextAnim;
    public Animator collectTextAnim;
    public int collectables = 0;
    public float powerMultiplier = 0f;
    public TextMeshProUGUI collectText;
    public Canvas pauseCanvas;
    public bool paused = false;
    public PauseScreen pauseScreen;
    public GameObject cameraPrefab;


    private AudioSource audioSource;
    private float maxVolume = .2f;
    private Player player;
    private MainCamera cam;
    private float timeLastOnScreen;
    private float timeAllowedOffScreen = 1.5f;
    private float loadTime = 2f;
    private bool skipUpdate = false;
    private int collectablesAtLevelStart;

    bool fading = false;
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    bool loaded = false;
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        fading = false;
        restarting = false;
        StopAllCoroutines();

        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        level = levelManager.level;
        cam = MainCamera.Instance;

        if (!player) SpawnPlayer();

        audioSource.clip = levelManager.levelTrack;

        hurtImage.color = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, 0f);


        StartCoroutine("FadeInScene");
        if (levelManager.loopFade)
        {
            audioSource.loop = false;
            StartCoroutine("LoopCurrentTrack");
        }
        else
        {
            audioSource.volume = maxVolume;
            audioSource.loop = true;
            audioSource.Play();
        }


        skipUpdate = false;
        timeLastOnScreen = -1f;
        audioSource.pitch = 1;
        loaded = true;
        if (cam && player)
        {
            cam.vcam.enabled = false;
            cam.transform.position = player.transform.position;
            cam.vcam.enabled = true;
        }

        if (level != 0 && level != -1) // not main menu or tutorial
        {
            SaveSystem.Save();
            saveTextAnim.SetTrigger("Saved");

        }

        collectablesAtLevelStart = collectables;
    }

    private void SpawnPlayer() {
        if (!levelManager.levelHasPlayer) return;
        GameObject playerObject = Instantiate(playerPrefab, levelManager.playerSpawn, Quaternion.identity);
        player = playerObject.GetComponent<Player>();
        cam.vcam.Follow = player.camTarget;
        levelManager.NotifyOfPlayerRespawn();
    }

    public int currentScene() {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
        if (!hurtImage) {
            hurtImage = GetComponentsInChildren<RawImage>()[0];
            fadeImage = GetComponentsInChildren<RawImage>()[1];
        };

        if (!loaded) {
            if (MainCamera.Instance)
            {
                cam = MainCamera.Instance;
                cam.mainCam = Camera.main;
                cam.transform.position += Vector3.back;
            }
            else {
                Instantiate(cameraPrefab, transform.position + Vector3.back * 5f, Quaternion.identity);
                cam = MainCamera.Instance;
                cam.mainCam = Camera.main;
                cam.transform.position += Vector3.back;
            }
        }

        //maxVolume = audioSource.volume;
        //SceneManager.sceneLoaded += OnSceneLoaded;

    }
    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance;
        if (!loaded) OnLevelFinishedLoading(SceneManager.GetActiveScene(), LoadSceneMode.Single);

    }

    // Update is called once per frame
    void Update()
    {

        if (skipUpdate || level == 0) return;
        // cheating for developers TODO REMOVE
        if (Input.GetKeyDown(KeyCode.Period)) {
            LoadLevelCaller(currentScene() + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Comma)) {
            LoadLevelCaller(currentScene() - 1);

        }
        if (player)
        {
            CheckPlayerCamPosition();
            fading = false;
        }

        if (Input.GetButtonDown("Pause")) {
            if (! (SceneManager.GetActiveScene().buildIndex == 0)) {
                if (!paused)
                {
                    Pause();
                } else {
                    UnPause();
                         }

            }

        }
    }

    public IEnumerator LoopCurrentTrack()
    {
        audioSource.volume = 0f;
        while (true) {
            audioSource.Play();
            yield return StartCoroutine(FadeInMusic());
            yield return new WaitForSeconds(audioSource.clip.length-3f);
            //while (audioSource.isPlaying) { yield return new WaitForSeconds(.01f); }
            yield return StartCoroutine(FadeOutMusic());
        }

    }

    private void CheckPlayerCamPosition() {
        Vector3 playerCamPosition = cam.mainCam.WorldToViewportPoint(
                new Vector3(player.transform.position.x - 1, player.transform.position.y, 0f)
                );

        if (( // if we are on screen
            0 < playerCamPosition.x &&
            playerCamPosition.x < 2 && // include this to disallow going off the right side of screen
            0 < playerCamPosition.y &&
            playerCamPosition.y < 1
            ))
        {
            timeLastOnScreen = Time.time; // restart the timer
        }
        if ((cam.isFollowingPath()) && Time.time >= timeLastOnScreen + timeAllowedOffScreen)
        {
            bool died = player.Die();
            if (died) skipUpdate = true;
        }

        if (cam.isFollowingPath())
        {

            if (playerCamPosition.x < .4)
                cam.CatchUp();
            else
                cam.StopCatchUp();
        }
    }

    public IEnumerator FadeInMusic() {
        audioSource.volume = 0;
        float step = .01f;
        float time = levelManager.loopFadeTime / (maxVolume / step);
        for (float i = 0; i <= maxVolume; i+=step){
            audioSource.volume = i;
            yield return new WaitForSeconds(time);
        }

    }


    public IEnumerator FadeOutMusic()
    {
        float step = .01f;
        float time = levelManager.loopFadeTime / (maxVolume / step);

        for (float i = maxVolume; i >= 0; i-=step)
        {
            audioSource.volume = i;
            yield return new WaitForSeconds(time);
        }
        audioSource.Stop();

    }

    IEnumerator FadeInScene()
    {
        fadeImage.raycastTarget = true;

        if (player && cam) cam.vcam.Follow = player.camTarget.transform;
        for (float i = 1f; i >= -.1f; i -= .05f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
            yield return new WaitForSeconds(.07f);
        }
        fadeImage.raycastTarget = false;


    }

    IEnumerator FadeOutScene()
    {
        fadeImage.raycastTarget = true;  
        for (float i = 0f; i <= 1.1f; i += .1f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
            yield return new WaitForSeconds(.07f);

        }
        fadeImage.raycastTarget = false;

    }

    public void FadeOutSceneCaller()
    {
     
        StartCoroutine("FadeOutScene");
    }
    bool loading = false;
    IEnumerator LoadLevel(int level){
        if (loading) yield break;
        loading = true;
        StartCoroutine("FadeOutMusic");
        yield return StartCoroutine("FadeOutScene");
        audioSource.Stop();
        audioSource.volume = 0f;
        if (level != currentScene())
        {
            
            SceneManager.LoadScene(level);

        }
        else SceneManager.LoadScene(level);
        loading = false;
    }

    public void LoadLevelCaller(int level = -1) {
        if (level == -1) level = currentScene() + 1;
        IEnumerator loader = LoadLevel(level);
        StartCoroutine(loader);

    }

    IEnumerator HurtFade()
    {

        for (float i = 0f; i <= .24; i += .1f)
        {
            hurtImage.color = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, i);
            yield return new WaitForSeconds(.07f);

        }
        for (float i = .24f; i >= -.1f; i -= .1f)
        {
            hurtImage.color = new Color(hurtImage.color.r, hurtImage.color.g, hurtImage.color.b, i);
            yield return new WaitForSeconds(.07f);
        }


    }

    public void PlayerDamage(float amt)
    {
        if (player.dashing) return;
        player.health -= amt;
        StartCoroutine("HurtFade");

        if (player.health <= 0) player.Die();

    }

    public void KillPlayer() {
        StartCoroutine("Restart");
    }

    bool restarting= false;
    IEnumerator Restart() {
        print(restarting);
        if (restarting) yield break;
        restarting = true;
        yield return FadeOutScene();
        Destroy(player.gameObject);
        player = null;
        yield return new WaitForSeconds(.2f);

        SpawnPlayer();
        yield return new WaitForSeconds(.2f);

        yield return FadeInScene();
        restarting = false;
    }

    public void LoadNextScene() {
        LoadLevelCaller(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AddCollectable()
    {
       collectables += 1;
       collectText.text = collectables.ToString();
       collectTextAnim.SetTrigger("Saved");
       player.playerController.playerDash.CalculateDashValues();
    }

    public void MainMenu() {
        LoadLevelCaller(0);
        UnPause();
    }
    public void RestartLevel() {
        collectables = collectablesAtLevelStart;
        LoadLevelCaller(SceneManager.GetActiveScene().buildIndex);
        UnPause();

    }

    public void Upgrades() { }

    public void ContinueFromPause() {
         UnPause();
    }

    public void Pause() {
        Time.timeScale = 0.1f;
        pauseScreen.Pause();
        paused = true;

    }
    public void UnPause() {
        Time.timeScale = 1;
        pauseScreen.UnPause();
        paused = false;
    }
}
