using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Game
{
    public static Game current;

    public int unlockedPuzz = 0;
    public bool music = true;
    public int winStreak = 0;
    public int highestWinStreak = 0;
    public List<int[]> winnablePuzzles = new List<int[]>();

    public void GetWinPuzzles()
    {
        current.winnablePuzzles.Add(new int[2] { 1, 3 });
        current.winnablePuzzles.Add(new int[2] { 1, 4 });//
        current.winnablePuzzles.Add(new int[2] { 1, 5 });
        current.winnablePuzzles.Add(new int[2] { 1, 6 });
        current.winnablePuzzles.Add(new int[2] { 2, 4 });//
        current.winnablePuzzles.Add(new int[2] { 2, 5 });//
        current.winnablePuzzles.Add(new int[2] { 2, 6 });
        current.winnablePuzzles.Add(new int[2] { 3, 5 });
        current.winnablePuzzles.Add(new int[2] { 3, 6 });
        current.winnablePuzzles.Add(new int[2] { 4, 6 });

        current.winnablePuzzles.Add(new int[3] { 1, 2, 3 });//
        current.winnablePuzzles.Add(new int[3] { 1, 2, 5 });
        current.winnablePuzzles.Add(new int[3] { 1, 2, 6 });//
        current.winnablePuzzles.Add(new int[3] { 1, 3, 4 });
        current.winnablePuzzles.Add(new int[3] { 1, 3, 6 });
        current.winnablePuzzles.Add(new int[3] { 1, 4, 5 });//
        current.winnablePuzzles.Add(new int[3] { 1, 5, 6 });
        current.winnablePuzzles.Add(new int[3] { 2, 3, 4 });//
        current.winnablePuzzles.Add(new int[3] { 2, 3, 5 });
        current.winnablePuzzles.Add(new int[3] { 2, 4, 5 });
        current.winnablePuzzles.Add(new int[3] { 2, 4, 6 });//
        current.winnablePuzzles.Add(new int[3] { 2, 5, 6 });
        current.winnablePuzzles.Add(new int[3] { 3, 4, 5 });
        current.winnablePuzzles.Add(new int[3] { 3, 4, 6 });
        current.winnablePuzzles.Add(new int[3] { 4, 5, 6 });

        current.winnablePuzzles.Add(new int[4] { 1, 2, 3, 5 });
        current.winnablePuzzles.Add(new int[4] { 1, 2, 3, 6 });//
        current.winnablePuzzles.Add(new int[4] { 1, 2, 4, 6 });//
        current.winnablePuzzles.Add(new int[4] { 1, 3, 4, 5 });//
        current.winnablePuzzles.Add(new int[4] { 1, 3, 5, 6 });
        current.winnablePuzzles.Add(new int[4] { 1, 4, 5, 6 });
        current.winnablePuzzles.Add(new int[4] { 2, 3, 4, 6 });
        current.winnablePuzzles.Add(new int[4] { 2, 4, 5, 6 });//

        current.winnablePuzzles.Add(new int[5] { 1, 2, 3, 4, 5 });
        current.winnablePuzzles.Add(new int[5] { 1, 2, 3, 5, 6 });//
        current.winnablePuzzles.Add(new int[5] { 1, 3, 4, 5, 6 });
        current.winnablePuzzles.Add(new int[5] { 2, 3, 4, 5, 6 });//


    }
}