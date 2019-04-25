using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int currentPlayer;
    public int turnNum;
    public int numMarks;
    public List<Mark> currentSpookyMark;
    public GameObject board;
    public Text playerTurn;
    public TextMeshProUGUI collapseText;
    public GameObject collapseButtons;
    public bool won;
    public bool draw;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = 1;
        turnNum = 1;
        numMarks = 0;
        playerTurn.text = "Player " + currentPlayer.ToString() + "'s turn";
        collapseText.text = "";
        collapseButtons.SetActive(false);
        currentSpookyMark = new List<Mark>();
        won = false;
        draw = false;
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
        int lastPlayer = currentPlayer;
        int lastTurn = turnNum;
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
        SpookyMark s = board.GetComponent<Board>().addSpookyMark(currentSpookyMark[0], currentSpookyMark[1]);
        if (s != null)
        {
            collapse(lastPlayer, lastTurn, s);
        }
        currentSpookyMark = new List<Mark>();
        numMarks = 0;
    }

    public void collapse(int player, int turn, SpookyMark sm)
    {
        string p = "";
        if (player == 1)
        {
            p = "X";
        } else if (player == 2)
        {
            p = "O";
        }
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().disableButton();
        }
        collapseText.text = "Select which square to collapse " + p + "<sub>" + turn.ToString() + "</sub>" + " into:";
        collapseButtons.SetActive(true);
        collapseButtons.transform.Find("Button1").Find("Text").GetComponent<Text>().text = "Square " + sm.position1;
        collapseButtons.transform.Find("Button2").Find("Text").GetComponent<Text>().text = "Square " + sm.position2;
    }

    public void finishCollapse()
    {
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().enableButton();
        }
        collapseText.text = "";
        collapseButtons.SetActive(false);
    }

    public void win(int player)
    {
        playerTurn.text = "Player " + player.ToString() + " wins!";
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().disableButton();
        }
        won = true;
    }

    public void noWinner()
    {
        playerTurn.text = "Draw!";
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().disableButton();
        }
        draw = true;
    }

    public void resetGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
