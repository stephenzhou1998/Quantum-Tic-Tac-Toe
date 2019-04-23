using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentPlayer;
    private int turnNum;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = 1;
        turnNum = 1;
    }

    public int getCurrentPlayer()
    {
        return currentPlayer;
    }

    public int getTurnNum()
    {
        return turnNum;
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
    }
}
