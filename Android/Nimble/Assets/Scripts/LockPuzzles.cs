using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPuzzles : MonoBehaviour {
    private GameObject[] button_objects;
    private Button[] buttons;
    public Button firstPuzzButton;

    // Use this for initialization
    void Start()
    {
        //SaveLoad.Load();
        //int unlockedPuzz = SaveLoad.game.unlockedPuzz;
        int unlockedPuzz = Game.current.unlockedPuzz;
        button_objects = GameObject.FindGameObjectsWithTag("puzz_button");

        //if (unlockedPuzz == 0)
        //{
        //    foreach (GameObject button in button_objects)
        //    {
        //        button.GetComponent<Button>().interactable = false;
        //    }
        //    firstPuzzButton.interactable = true;
        //}
        //else
        //{

            foreach (GameObject button in button_objects)
            {
                if (button.GetComponent<PuzzleButton>().levelToLoad > unlockedPuzz)
                {
                    button.GetComponent<Button>().interactable = false;
                }
                else
                {
                    button.GetComponent<Button>().interactable = true;
                }
            //}
        }
    }
	
}
