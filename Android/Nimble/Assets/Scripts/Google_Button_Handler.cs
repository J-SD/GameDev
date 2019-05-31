using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Google_Button_Handler : MonoBehaviour {
    GameMaster master;
    Text logText;
    public void Start() {
        master = GameObject.Find("Game_Master").GetComponent<GameMaster>();
        logText = GameObject.Find("LO_Text").GetComponent<Text>();
        if (Social.localUser.authenticated)
        {
            logText.text = "Log out";
        }
        else
        {
            logText.text = "Log in";
        }
    }
    public void ShowLeaderboard() {
        master.OnShowLeaderBoard();
    }

    public void ChangeLog() {
        
        if (Social.localUser.authenticated)
        {
            LogOut();
            logText.text = "Log in";
        }
        else {
            LogIn();
            logText.text = "Log out";
        }

    }
    public void LogOut() {
        master.OnLogOut();
    }

    public void LogIn() {
        master.LogIn();
    }

}
