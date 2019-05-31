using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using UnityEngine.SceneManagement;
public class Ads : MonoBehaviour
{
    Scene scene;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    bool bannerLoaded = false;
    bool interstitialLoaded = false;
    GameMaster master;

    private void RequestBanner()
    {
        #if UNITY_EDITOR
            string adUnitId = "ca-app-pub-6935891731532644/5501872811";
        #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-6935891731532644/5501872811" ;
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif
        
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
        bannerLoaded = true;
    }

    private void RequestInt() {
        #if UNITY_EDITOR
            string adUnitId = "ca-app-pub-6935891731532644/7643764817";
        #elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-6935891731532644/7643764817" ;
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        interstitial.LoadAd(request);
        interstitialLoaded = true;
        interstitial.OnAdClosed += HandleInterstitialClosed;


    }
    public void HandleInterstitialClosed(object sender, System.EventArgs args) {
        print("closed");
        RequestInt();
    }

    void Awake()
    {
        SceneManager.activeSceneChanged += SceneChanged; // subscribe
        SceneManager.sceneLoaded += SceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneChanged; // unsubscribe
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("game_master").Length == 1)
        {
            RequestInt();
            RequestBanner();
            bannerView.Hide();
        }
        master = this.GetComponentInParent<GameMaster>();
    }

    void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.name == "Game_Scene0" || scene.name == "Game_Scene1")
        {
            bannerView.Show();

        }
        else
        {
            if(bannerLoaded) bannerView.Hide();
        }

    }

    void SceneChanged(Scene previousScene, Scene newScene)
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        if (master.loadCount > 3 && interstitialLoaded) {
            print("int show");
            interstitial.Show(); 
            master.loadCount = 0;
        }
    }
}
