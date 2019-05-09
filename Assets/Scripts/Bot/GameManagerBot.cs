using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerBot
{
    private int currentPlayer;
    private int turnNum;
    public int numMarks;
    private bool won;
    private bool draw;
    public BoardBot board;
    // Start is called before the first frame update
    public void init(int currentPlayer, int turnNum, int numMarks,
        bool won, bool draw, BoardBot board)
    {
        this.currentPlayer = currentPlayer;
        this.turnNum = turnNum;
        this.numMarks = numMarks;
        this.won = won;
        this.draw = draw;
        this.board = board;
    }

    public GameManagerBot(GameManager gm)
    {
        this.init(gm.currentPlayer, gm.turnNum, gm.numMarks, 
            gm.won, gm.draw, new BoardBot(gm.board.GetComponent<Board>(), this));
    }

    public GameManagerBot(GameManagerBot gmb)
    {
        this.init(gmb.currentPlayer, gmb.turnNum, gmb.numMarks, gmb.won, gmb.draw,
            new BoardBot(gmb.board, this));
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public int getTurnNum()
    {
        return turnNum;
    }

    public int getNumMarks()
    {
        return numMarks;
    }

    public void nextTurn()
    {
        int lastPlayer = currentPlayer;
        int lastTurn = turnNum;
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else if (currentPlayer == 2)
        {
            currentPlayer = 1;
        }
        turnNum++;
        foreach (SquareBot square in board.squares)
        {
            square.reset();
        }
        SpookyMarkBot s = board.addSpookyMark();
        numMarks = 0;
    }

    public void win(int player)
    {
        won = true;
    }

    public void noWinner()
    {
        draw = true;
    }
}
