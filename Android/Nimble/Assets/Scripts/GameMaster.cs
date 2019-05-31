using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameMaster : MonoBehaviour {
    public Game game;
    public int loadCount;
    string leaderboard = "CgkIxdGqgtsLEAIQAQ";

    // Use this for initialization
    void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Game.current = new Game();
    }
    void Start () {
        SaveLoad.Load();
        game = SaveLoad.game;
        Game.current = game;


        GetComponent<AudioSource>().mute = !Game.current.music;
        // Activate the Google Play Games platform
        //PlayGamesPlatform.Activate();
        LogIn();


        //******* FOR DEBUG ONLY:
        //Game.current.unlockedPuzz = 15;
        //*******

        this.tag = "game_master";
        if (GameObject.FindGameObjectsWithTag("game_master").Length > 1)
        {
            Destroy(GetComponent<Ads>());
            Destroy(this.gameObject);
        }
    }

    void OnDestroy() {
        SaveLoad.Save();
        //OnLogOut();
    }

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Sucess");
            }
            else
            {
                Debug.Log("Login failed");
            }
        });
    }

    public void OnShowLeaderBoard()
    {
        //        Social.ShowLeaderboardUI (); // Show all leaderboard
        //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard); // Show current (Active) leaderboard
    }

    public void OnAddScoreToLeaderBoard(int streak)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(streak, leaderboard, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");

                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }
    }

    public void OnLogOut()
    {
        //((PlayGamesPlatform)Social.Active).SignOut();
    }


}
