using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentPlayer;
    private int turnNum;
    private int numMarks;
    public GameObject board;
    public Text playerTurn;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = 1;
        turnNum = 1;
        numMarks = 0;
        playerTurn.text = "Player " + currentPlayer.ToString() + "'s turn";
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public int getTurnNum()
    {
        return turnNum;
    }

    public void addMark()
    {
        numMarks++;
        if (numMarks == 2)
        {
            nextTurn();
        }
    }

    public int getNumMarks()
    {
        return numMarks;
    }

    public void nextTurn()
    {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        } else if (currentPlayer == 2)
        {
            currentPlayer = 1;
        }
        turnNum++;
        playerTurn.text = "Player " + currentPlayer.ToString() + "'s turn";
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().reset();
        }
        numMarks = 0;
    }
}
