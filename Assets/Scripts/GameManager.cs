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
    private List<Mark> currentSpookyMark;
    public GameObject board;
    public Text playerTurn;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = 1;
        turnNum = 1;
        numMarks = 0;
        playerTurn.text = "Player " + currentPlayer.ToString() + "'s turn";
        currentSpookyMark = new List<Mark>();
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public int getTurnNum()
    {
        return turnNum;
    }

    public void addMark(Mark mark)
    {
        numMarks++;
        currentSpookyMark.Add(mark);
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
        board.GetComponent<Board>().addSpookyMark(currentSpookyMark[0], currentSpookyMark[1]);
        currentSpookyMark = new List<Mark>();
        numMarks = 0;
    }
}
