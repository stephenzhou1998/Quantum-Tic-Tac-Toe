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
    public Text botThinking;
    public TextMeshProUGUI collapseText;
    public GameObject collapseButtons;
    public bool won;
    public bool draw;
    public bool pvb;
    private bool playerCollapsing;
    private Bot bot;
    // Start is called before the first frame update
    void Awake()
    {
        currentPlayer = 1;
        turnNum = 1;
        numMarks = 0;
        playerTurn.text = "Player X's turn";
        collapseText.text = "";
        botThinking.text = "";
        collapseButtons.SetActive(false);
        won = false;
        draw = false;
        playerCollapsing = false;
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
                botThinking.text = "Bot is thinking...";
            } else
            {
                str = "Player O's turn";
            }
            playerCollapsing = false;
        } else if (currentPlayer == 2)
        {
            if (pvb)
            {
                botSwitchedBack = true;
                botThinking.text = "";
            }
            currentPlayer = 1;
            str = "Player X's turn";
        }
        turnNum++;
        playerTurn.text = str;
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().reset();
        }
        SpookyMark s = board.GetComponent<Board>().addSpookyMark();
        if (s != null && !switchedToBot)
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
            StartCoroutine(botTurn(actionType, turnNum));
        }

        if (botSwitchedBack && !playerCollapsing)
        {
            foreach (Transform square in board.transform)
            {
                square.gameObject.GetComponent<Square>().enableButton();
            }
        }
    }

    IEnumerator botTurn(int actionType, int turnNum)
    {
        yield return new WaitForSeconds(0);
        bot.executeTurn(actionType, turnNum);
    }

    public void collapse(int player, int turn, SpookyMark sm)
    {
        string p = "";
        if (player == 1)
        {
            p = "X";
            playerCollapsing = false;
        } else if (player == 2)
        {
            p = "O";
            playerCollapsing = true;
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
        string winner = "";
        if (player == 1)
        {
            winner = "X";
        }
        if (player == 2)
        {
            winner = "O";
        }
        if (pvb && player == 2)
        {
            playerTurn.text = "Bot wins!";
        } else
        {
            playerTurn.text = "Player " + winner + " wins!";
        }
        botThinking.text = "";
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().disableButton();
        }
        won = true;
    }

    public void noWinner()
    {
        playerTurn.text = "Draw!";
        botThinking.text = "";
        foreach (Transform square in board.transform)
        {
            square.gameObject.GetComponent<Square>().disableButton();
        }
        draw = true;
    }

    public void resetGame()
    {
        if (pvb)
        {
            SceneManager.LoadScene("PVB");
        } else
        {
            SceneManager.LoadScene("PVP");
        }
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
