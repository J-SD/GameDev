using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Streak_Counter : MonoBehaviour {

	// Use this for initialization
	void Update () {
        //SaveLoad.Load();
        //int unlockedPuzz = SaveLoad.game.unlockedPuzz;
        int unlockedPuzz = Game.current.unlockedPuzz;
        if (unlockedPuzz > 14)
        {
            this.gameObject.SetActive(true);
            Text winStreakText = GameObject.Find("win_streak_text").GetComponent<Text>();
                winStreakText.text = Game.current.winStreak.ToString();
            if (winStreakText.gameObject.tag == "in_puzzle_streak") {
                if (GameObject.Find("PuzzleController").GetComponent<PuzzleController>().puzzle > 14) {
                    this.gameObject.SetActive(true);
                }
                else this.gameObject.SetActive(false);
            }
        }
        else {
            this.gameObject.SetActive(false);
        }
	}
}
