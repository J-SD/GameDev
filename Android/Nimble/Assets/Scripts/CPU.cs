using UnityEngine;
using System.Collections.Generic;

public class CPU : MonoBehaviour
{
    public Board board;
    int[] position;

    // Update is called once per frame
    void Update()
    {
        //CalculateMove();
    }

    int[] getCurrentPosition() {
        List<Coin> coins = board.coinList;
        coins.Sort(Board.SortCoinsByIndex);
        int i = 0;
        //the current position
        position = new int[coins.Count];
        i = 0;
        //set the current position in order
        foreach (Coin coin in coins)
        {
            position[i] = coin.box_index;
            i++;
        }
    return position;

    }

    //Looks at every coin and returns a list of arrays representing possible moves where the first int in the array is the coin and the second int is the possible next box
    List<int[]> getPossibleMoves() {
        List<Coin> coins = board.coinList;
        coins.Sort(Board.SortCoinsByIndex);
        position = getCurrentPosition();
       
        List<int[]> possibleMoves = new List<int[]>();

        //for ever coin, starting from the right
        for (int c = coins.Count - 1; c > -1; c--)
        {
            
            //if this is the leftmost coin
            if (c == 0)
            {
                //for every index left of the coin, add it to possible moves
                for (int lPI = coins[c].box_index - 1; lPI > -1; lPI--)
                {
                    int[] p_move = new int[2];
                    p_move[0] = c;
                    p_move[1] = lPI;
                    possibleMoves.Add(p_move);
                }
            }
            //if this is not the leftmost coin but this coin exists
            else if (c - 1 >= 0)
            {
                Coin thisCoin = coins[c];
                Coin nextCoinL = coins[c - 1];
                int thisCoinIndex = thisCoin.box_index;
                int nextCoinLIndex = nextCoinL.box_index;
                //if there is no coin to the left of this coin
                if (nextCoinLIndex + 1 != thisCoinIndex)
                {
                    //for each possible move for this coin, add it to possible moves
                    for (int pI = thisCoinIndex - 1; pI > nextCoinLIndex; pI--)
                    {
                        int[] p_move = new int[2];
                        p_move[0] = c;
                        p_move[1] = pI;
                        possibleMoves.Add(p_move);
                    }

                }
            }          
        }
        
        return possibleMoves;
        
    }
    

    public int[] CalculateMove() {
        bool foundWinningMove = false;
        int[] move = new int[2];
        int nimSum = 100;
        List<int[]> possibleMoves = getPossibleMoves();
        foreach (int[] p_move in possibleMoves)
        {
            //int[] newPosition = new int[position.Length];
            int[] newPosition = getNewPosition(p_move);

            int newNimSum = calculateNimSum(getOddGapSizes(newPosition));

            if (newNimSum == 0)
            {
                move = p_move;
                foundWinningMove = true;
            }

            //if (newNimSum < nimSum)
            //{
            //    nimSum = newNimSum;

            //    move = p_move;
            //}
        }
        //print(foundWinningMove);
        if (!foundWinningMove) {
            System.Random rnd = new System.Random();
            int r1 = rnd.Next(possibleMoves.Count);
            int r2 = rnd.Next(possibleMoves.Count);
            int r3 = rnd.Next(possibleMoves.Count);

            List<int[]> moves = new List<int[]> { possibleMoves[r1], possibleMoves[r2], possibleMoves[r3] };
            int[] sums = new int[3] { calculateNimSum(getNewPosition(moves[0])), calculateNimSum(getNewPosition(moves[1])), calculateNimSum(getNewPosition(moves[2])) };
            int min = sums[0];
            foreach (int sum in sums)
            {
                if (sum < min) min = sum;
            }

            move = possibleMoves[r1];
            for (int i = 0; i < sums.Length; i++){
                if (sums[i] == min) {
                    move = moves[i];
                }
            }
            
        }
     
        return move;        
    }

    int[] getNewPosition(int[] newMove) {
        //int[] newPosition = new int[position.Length];
        int[] currentPosition = getCurrentPosition();
        int[] newPosition = currentPosition;
        int coinToMove = newMove[0];
        int boxToMove = newMove[1];

        if (boxToMove != 0)
        {
            newPosition[coinToMove] = boxToMove;
        }
        else {
            newPosition = new int[currentPosition.Length - 1];
            int t = 0;
            for (int i = 1; i < currentPosition.Length; i++) {
                newPosition[t] = currentPosition[i];
                t++;
            }

        }

        //string currentPosString = "current pos: ";

        //foreach (int x in currentPosition) {
        //    currentPosString += x + ", ";
        //}

        //string printStr = currentPosString +  " new move: ";
        //foreach (int i in newMove) {
        //    printStr += i + ", ";
        //}
        //string newPosString = "";
        //foreach (int p in newPosition) {
        //    newPosString += p + ", ";
        //}
        //printStr += "new pos: " +  newPosString;

        //printStr += "new nim: " + calculateNimSum(getOddGapSizes(newPosition));


        //print(printStr);

        return newPosition;
    }

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
    int[] getOddGapSizes(int[] position) {
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

    bool checkLegalMove(int[] position) {
        foreach (int i in position) {
            if (countArray(position, i) > 1) {
                return false;
            }
        }

        return true;
    }

    int countArray(int[] array,int checkValue) {
        int count = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == checkValue) count++;
        }
        return count; 
    }

}

