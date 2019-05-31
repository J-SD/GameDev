using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenRandomPuzzles {
    int calculateNimSum(int[] oddGapSizes)
    {
        int nimSum = 0;

        for (int i = 0; i < oddGapSizes.Length; i++)
        {
            nimSum = nimSum ^ oddGapSizes[i];

        }

        return nimSum;
    }

    //gets odd gap sizes from board positions:
    int[] getOddGapSizes(int[] position)
    {
        int numCoins = position.Length;
        int numOddGaps = Mathf.CeilToInt(((float)numCoins / 2));

        int[] oddGapSizes = new int[numOddGaps];
        int j = 1;
        int n = 0;
        for (int i = numCoins; i > 0; i--)
        {
            //check if odd gap
            if (j % 2 != 0)
            {
                int gapSize;
                if (j == numCoins)
                {
                    //if this is the last gap, the size is the number of possible moves (the index of the first coin)
                    gapSize = position[0];
                }
                else
                {
                    int coin1Index = position[i - 1];
                    int coin0Index = position[i - 2];
                    gapSize = Mathf.Abs(coin1Index - coin0Index) - 1;
                }
                oddGapSizes[n] = gapSize;
                n++;
            }
            j++;
        }
        return oddGapSizes;
    }

    public bool checkDupes(int[] puzzle)
    {
        bool dupes = false;
        for (int i = 1; i < 7; i++)
        {
            int count = 0;
            foreach (int a in puzzle)
            {
                if (a == i)
                {
                    count++;
                }
            }
            if (count > 1) { dupes = true; }
        }
        return dupes;
    }

    List<int[]> getPuzzles()
    {
        //CREATE ONE LOOP THAT CAN ADD EVERYTHING 0-6 AND THEN REMOVE THE 0s
        List<int[]> puzzles = new List<int[]>();
        for (int u = 0; u < 7; u++)
        {
            for (int v = u; v < 7; v++)
            {
                for (int w = v; w < 7; w++)
                {
                    for (int x =0; x < 7; x++)
                    {
                        for (int y = x; y < 7; y++)
                        {
                            for (int z = y; z < 7; z++)
                            {
                                int[] puzzle = new int[6] { u, v, w, x, y, z };
                                
                                bool dupes = checkDupes(puzzle);

                                if (!checkInList(puzzle, puzzles) && !dupes)
                                {
                                    puzzles.Add(puzzle);
                                }
                            }
                        }
                    }
                }
            }
        }

        List<int[]> filteredPuzzles = new List<int[]>();
        foreach (int[] p in puzzles) {
            List<int> newPuzzle = new List<int>();
            foreach (int i in p) {
                if (i != 0) {
                    newPuzzle.Add(i);
                }
            }
            int[] newPuzzleArray = newPuzzle.ToArray();
            newPuzzleArray = (from element in newPuzzleArray orderby element ascending select element)
                   .ToArray();

            if (!checkInList(newPuzzleArray, filteredPuzzles) && !checkDupes(newPuzzleArray) && newPuzzle.Count > 1 ) {
                filteredPuzzles.Add(newPuzzleArray);
            }
        }

        return (filteredPuzzles);
    }

    bool checkInList(int[] puzzle, List<int[]> puzzles) {
        bool inList = false;

        foreach (int[] p in puzzles) {
            if (Enumerable.SequenceEqual(p, puzzle)) {
                inList = true;
            }
        }

        return inList;
    }
    public List<int[]> getWinnablePuzzles() {
        List<int[]> puzzles = new List<int[]>();
        List<int[]> allPuzzles = getPuzzles();

        foreach (int[] puzzle in allPuzzles) {
            int[] gapSizes = getOddGapSizes(puzzle);
            int nimSum = calculateNimSum(gapSizes);

            if (nimSum != 0 && !checkInList(puzzle, puzzles)) {
                puzzles.Add(puzzle);
            }

        }


        return puzzles;

    }


}



