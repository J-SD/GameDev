using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public GameObject coinPrefab;
    public float[] box_positions;
    public bool playerTurn = false;
    public List<Box> boxes;
    public int NUM_COINS;
    public Coin[] coins;
    public List<Coin> coinList;
    public Button restartButton;
    public Button quitButton;
    public Button nextButton;
    public GameObject inkArrow;
    public GameObject streakNumber;

    bool moved;
    PuzzleController pc;
    public int[] position;
    int frame;
    Text move_text1;
    Text move_text2;
    CPU cpu;
    int puzzle;
    bool secondScene;
    int[] randomPuzzle;
    int lossCount = 0;

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex > 3) { secondScene = true; } else secondScene = false;
        frame = 0;
        pc = GameObject.FindGameObjectWithTag("PuzzleController").GetComponent<PuzzleController>();
        puzzle = pc.puzzle;
        cpu = GetComponent<CPU>();

        GameObject[] bs = GameObject.FindGameObjectsWithTag("box");
        move_text1 = GameObject.FindGameObjectWithTag("move_text1").GetComponent<Text>();
        if(!secondScene) move_text2 = GameObject.FindGameObjectWithTag("move_text2").GetComponent<Text>();

        box_positions = new float[bs.Length];

        foreach (GameObject b in bs)
        {
            b.SendMessage("Initialize");
        }

        boxes.Sort(SortBoxesByIndex);
        
        Restart(true);
       
    }


    public void Restart(bool next = false)
    {
        lossCount++;
        if (puzzle != 0)
        {
            move_text1.text = "Your Turn";
            StartCoroutine(ChangeText(move_text1, "Your Turn"));
            if (!secondScene) move_text2.enabled = false;
        }
        else
        {
            inkArrow.SetActive(true);
            move_text2.enabled = true;
            move_text1.text = "Slide coins to the left\n(including off the board)";
            if (!secondScene) move_text2.text = "only one coin is allowed\nper box";
        }

        if (puzzle == 2 && lossCount > 2) {
            move_text2.enabled = true;
            move_text2.text = "Move coins all the way\nto the left to remove them";
        }
      

        if (puzzle < 15)
        {
            Text puzzNum = GameObject.Find("Puzzle_Number").GetComponent<Text>();
            puzzNum.text = "Puzzle: " + (puzzle + 1).ToString() + "/15";
        }
        else
        {
            try
            {
                Text puzzNum = GameObject.Find("Puzzle_Number").GetComponent<Text>();
                puzzNum.gameObject.SetActive(false);
            }
            catch (Exception e) {
                print(e.Data);
            }
            streakNumber.gameObject.SetActive(true);
        }

        foreach (Coin c in coinList)
        {
            Destroy(c.gameObject);
        }
        coinList = new List<Coin>();

        foreach (Box b in boxes) {
            b.has_coin = false;
        }

        coinList = new List<Coin>();
        puzzle = pc.puzzle;
        if (next)
        {
            position = pc.getPuzzlePosition(puzzle);
            if (puzzle > 14) {
                randomPuzzle = position;
            }
        }
        else
        {
            if (puzzle > 14)
            {
                position = randomPuzzle;
                Game.current.winStreak = 0;
            }
        }
        NUM_COINS = position.Length;

        coins = new Coin[NUM_COINS];

        for (int i = 0; i < NUM_COINS; i++)
        {
            GameObject c = (Instantiate(coinPrefab) as GameObject);
            c.transform.SetParent(transform, false);
            c.name = "Coin_" + i.ToString();
            coins[i] = c.GetComponent<Coin>();
            coins[i].box_index = position[i];
            coins[i].index = i;
            coinList.Add(coins[i]);

            c.SendMessage("Initialize");
        }

        coinList.Sort(SortCoinsByIndex);

        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        playerTurn = true;
        moved = false;
       

    }



    public static int SortCoinsByIndex(Coin c1, Coin c2)
    {
        return c1.box_index.CompareTo(c2.box_index);
    }

    public static int SortBoxesByIndex(Box b1, Box b2)
    {
        return b1.index.CompareTo(b2.index);
    }

    void Update()
    {
        frame++;
        if (frame % 10 == 0) {
            int last_index = 100;
            Coin coin;
            foreach (Coin c in coins) {
                try { coin = c.GetComponent<Coin>();}
                catch (Exception e) { return; }
                
                if (coin.box_index==last_index)
                {
                    c.transform.position = new Vector2(box_positions[coin.box_index+1], 0);
                }

                last_index = coin.box_index;
            }
            coinList.Sort(SortCoinsByIndex);

        }
        

    }

    bool checkValidMove()
    {
        foreach (Coin c in coinList)
        {
            if (countCoinIndexes(coinList, c.box_index) > 1) return false;
           
        }
        return true;
    }
    int countCoinIndexes(List<Coin> list, int i) {
        int count = 0;
        foreach (Coin c in list) {
            if (c.box_index == i) count++;
        }
        return count;
    }


    void NextTurn() {
        foreach (Coin c in coins)
        {
            try
            {
                c.SendMessage("reset", SendMessageOptions.DontRequireReceiver);
            }
            catch (Exception e) { }
        }

        if (!checkValidMove()) return;

        if (playerTurn) playerTurn = false;
        else if (!playerTurn) playerTurn = true;

        if (playerTurn) PlayerTurn();
        else if (!playerTurn) StartCoroutine(WaitBeforeComputerTurn());

    }

    IEnumerator WaitBeforeComputerTurn()
    {
        if (coinList.Count == 0)
        {
            StartCoroutine(ChangeText(move_text1,"You Won!"));
            if(puzzle == 0 &&!secondScene) move_text2.enabled = false;
            PlayerWin();
        }
        else
        {
            if (puzzle != 0)
            {
                StartCoroutine(ChangeText(move_text1,"Thinking"));
                yield return new WaitForSeconds(.2f);
                move_text1.text = "Thinking.";
                yield return new WaitForSeconds(.2f);
                move_text1.text = "Thinking..";
                yield return new WaitForSeconds(.2f);
                move_text1.text = "Thinking...";
                yield return new WaitForSeconds(.3f);
            }
            else {
                if (!moved)
                {
                    inkArrow.SetActive(false);
                    StartCoroutine(ChangeText(move_text1, "Be the last player to remove a coin"));
                    if(!secondScene)StartCoroutine(ChangeText(move_text2, "Good Luck!"));
                    moved = true;
                }
            }
            ComputerTurn();
        }
    }

    void ComputerTurn() {     
        int[] move = cpu.CalculateMove();
        coinList[move[0]].SmoothSnapToBox(move[1]);
    }

    void PlayerTurn() {
        if (coinList.Count == 0)
        {
            if(!secondScene)move_text2.text = "";
            StartCoroutine(ChangeText(move_text1,"Computer Won"));
            ComputerWin();
        }
        else {
            if (puzzle != 0) StartCoroutine(ChangeText(move_text1, "Your Turn"));
           }
       
    }

    void PlayerWin() {
        nextButton.gameObject.SetActive(true);
        if (puzzle > 14)
        {
            GameMaster master = GameObject.FindObjectOfType<GameMaster>();
            Game.current.winStreak++;
            if (Game.current.winStreak >= Game.current.highestWinStreak)
            {
                Game.current.highestWinStreak = Game.current.winStreak;
                master.OnAddScoreToLeaderBoard(Game.current.highestWinStreak);
            }

            
        }
        if (puzzle+1 >= Game.current.unlockedPuzz)
        {
            Game.current.unlockedPuzz = puzzle + 1;
            if (Game.current.unlockedPuzz > 15) Game.current.unlockedPuzz = 15;
            //SaveLoad.game.unlockedPuzz = puzzle+1;            
        }
        SaveLoad.Save();

        if (puzzle == 2) {
            move_text2.enabled = false;
        }
    }
    void ComputerWin() {
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        if (puzzle > 14) {
            //SaveLoad.game.winStreak = 0;
            Game.current.winStreak = 0; 
            SaveLoad.Save();
        }
        if (puzzle == 2)
        {
            move_text2.enabled = false;
        }
    }

    IEnumerator ChangeText(Text text, string s) {
        yield return StartCoroutine(FadeTextToZeroAlpha(.2f, text));
        text.text = s;
        yield return StartCoroutine(FadeTextToFullAlpha(.2f, text));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    void NextPuzzle() {
        StartCoroutine(puzzleAdvance());
    }

    IEnumerator puzzleAdvance() {
        puzzle = pc.NextPuzzle();
        lossCount = 0;
        yield return new WaitForSeconds(1);

    }

}

