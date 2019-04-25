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
    public GameObject board;
    public Text playerTurn;
    public TextMeshProUGUI collapseText;
    public GameObject collapseButtons;
    public bool won;
    public bool draw;
    public bool pvb;
    private Bot bot;
    // Start is called before the first frame update
    void Awake()
    {
        currentPlayer = 1;
        turnNum = 1;
        numMarks = 0;
        playerTurn.text = "Player " + currentPlayer.ToString() + "'s turn";
        collapseText.text = "";
        collapseButtons.SetActive(false);
        won = false;
        draw = false;
        bot = GameObject.Find("Bot").GetComponent<Bot>();
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
        bool switchedToBot = false;
        bool botSwitchedBack = false;
        string str = "";
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
            if (pvb)
            {
                str = "Bot's turn";
                switchedToBot = true;
            } else
            {
                str = "Player " + currentPlayer.ToString() + "'s turn";
            }
        } else if (currentPlayer == 2)
        {
            if (pvb)
            {
                botSwitchedBack = true;
            }
            currentPlayer = 1;
            str = "Player " + currentPlayer.ToString() + "'s turn";
        }
        turnNum++;
        playerTurn.text = str;
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().reset();
        }
        SpookyMark s = board.GetComponent<Board>().addSpookyMark();
        if (s != null)
        {
            collapse(lastPlayer, lastTurn, s);
        }
        numMarks = 0;
        if (switchedToBot)
        {
            foreach (Transform square in board.transform)
            {
                square.gameObject.GetComponent<Square>().disableButton();
            }
            int actionType = 0;
            if (s != null)
            {
                actionType = 1;
            }
            bot.executeTurn(actionType, turnNum);
        }

        if (botSwitchedBack)
        {
            foreach (Transform square in board.transform)
            {
                square.gameObject.GetComponent<Square>().enableButton();
            }
        }
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
        if (pvb && currentPlayer == 2)
        {
            bot.executeTurn(0, turnNum);
        }
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
