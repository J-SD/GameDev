using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour {
    public GameObject puzzleController;
    public int levelToLoad;
	// Use this for initialization
	
    public void onClick() {
        puzzleController = GameObject.Find("PuzzleController");
        puzzleController.GetComponent<PuzzleController>().LoadPuzzle(levelToLoad);
    }
	
	
}
