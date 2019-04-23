﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Square : MonoBehaviour
{
    private Transform innerSquares;
    private GameManager gameManager;
    public GameObject X;
    public GameObject O;
    private bool alreadyMarked;
    private int filledMarks;
    public List<Mark> presentMarks;

    // Start is called before the first frame update
    void Start()
    {
        innerSquares = transform.Find("InnerSquares");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        presentMarks = new List<Mark>();
        alreadyMarked = false;
    }

    public Square()
    {
        Start();
    }

    public void addMark()
    {
        int numMarks = gameManager.getNumMarks();
        if (numMarks == 2 || alreadyMarked || filledMarks == 8)
        {
            Debug.Log("1");
            return;
        }
        int player = gameManager.getCurrentPlayer();
        int turn = gameManager.getTurnNum();
        GameObject toAdd = null;
        string restore = "";
        if (player == 1)
        {
            toAdd = X;
            restore = "X";
        } else if (player == 2) {
            toAdd = O;
            restore = "O";
        } else
        {
            toAdd = O;
            restore = "O";
        }
        toAdd.GetComponent<TextMeshProUGUI>().text += "<sub>" + turn.ToString() + "</sub>";
        foreach (Transform square in innerSquares)
        {
            if (square.childCount == 0)
            {
                Instantiate(toAdd, square);
                filledMarks++;
                break;
            }
        }
        toAdd.GetComponent<TextMeshProUGUI>().text = restore;
        Mark mark = new Mark(player, turn);
        presentMarks.Add(mark);
        alreadyMarked = true;
        gameManager.addMark();
    }

    public void reset()
    {
        alreadyMarked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
