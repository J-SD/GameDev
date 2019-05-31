using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PuzzleController : MonoBehaviour {
    public int puzzle;
    public int lastPuzzleCleared;

    List<int[]> puzzles;

    // Use this for initialization


	void Start () {
        if (GameObject.Find("PuzzleController") != this.gameObject) {
            this.gameObject.tag = "coin";
            this.name = "NotAPuzzleController";
            return;
        }
        Game.current.GetWinPuzzles();

        puzzles = new List<int[]>();
        DontDestroyOnLoad(this.gameObject);

        //puzzle 0: [2,4]
        puzzles.Add(new int[] { 2, 4 });
        //puzzle 1: 
        puzzles.Add(new int[] { 1, 4 });
        //2
        puzzles.Add(new int[] { 2, 3, 4 });
        //3
        puzzles.Add(new int[] { 1, 2, 3 });
        //4
        puzzles.Add(new int[] {2,5 });
        //5
        puzzles.Add(new int[] { 1,4,5});
        //6
        puzzles.Add(new int[] { 2, 4, 6 });
        //7
        puzzles.Add(new int[] { 1, 2, 6 });
        //8
        puzzles.Add(new int[] { 1, 2, 3, 6 });
        //9
        puzzles.Add(new int[] { 2, 4, 5, 6 });
        //10
        puzzles.Add(new int[] { 1, 2, 4, 6 });
        //11
        puzzles.Add(new int[] { 1, 4, 5, 6});
        //12
        puzzles.Add(new int[] { 1, 3, 4, 5 });
        //13
        puzzles.Add(new int[] { 1, 2, 3, 5, 6 });
        //14
        puzzles.Add(new int[] { 2, 3, 4, 5, 6 });

    }
    
    public int[] getPuzzlePosition(int p) {
        if (puzzle < 15) return puzzles[p]; //if this is one of the selectable puzzles, use it's index
        else {                              //if this is a random puzzle:
            List<int[]> winnablePuzzles; //the list to be filled with winnable puzzles, get it from the current game
            if (Game.current.winnablePuzzles == null) //if the current game's list is empty, then we need to get the random puzzles
            {
                Game.current.GetWinPuzzles();
                winnablePuzzles = Game.current.winnablePuzzles;
            }
            winnablePuzzles = Game.current.winnablePuzzles;
            System.Random rnd = new System.Random(); 
            int r = rnd.Next(winnablePuzzles.Count);
            return winnablePuzzles[r]; //return a random puzzle
        }
    }

    public void LoadPuzzle(int p) {
       
        LoadOnClick loc = GameObject.Find("Canvas").GetComponent<LoadOnClick>();
        puzzle = p;
       
        if (p<4) loc.LoadScene(3);
        else if (p < 100) loc.LoadScene(4);
    }

    public int NextPuzzle() {
        GameObject.Find("Game_Master").GetComponent<GameMaster>().loadCount++;
        puzzle = puzzle + 1;
       
        if (puzzle == 4) LoadPuzzle(4);
        else
        {
            StartCoroutine(fadePuzzles());
        }
        return puzzle;
    }


    IEnumerator fadePuzzles() { 
        Fading fading = GameObject.Find("Game_Master").GetComponent<Fading>();
        float fadeTime = fading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        GameObject.Find("Board").GetComponent<Board>().Restart(true);
        fading.BeginFade(-1);
    }
}
