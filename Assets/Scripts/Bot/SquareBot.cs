﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SquareBot
{
    private GameManagerBot gameManager;
    public int position;
    private bool alreadyMarked;
    public bool classicallyMarked;
    public int finalPlayer;
    public int finalTurn;
    public int filledMarks;
    public List<MarkBot> presentMarks;

    public void init(GameManagerBot gmb, List<MarkBot> pms, int position, bool alreadyMarked, bool classicallyMarked, 
        int finalPlayer, int finalTurn, int filledMarks)
    {
        gameManager = gmb;
        presentMarks = pms;
        this.position = position;
        this.alreadyMarked = alreadyMarked;
        this.classicallyMarked = classicallyMarked;
        this.finalPlayer = finalPlayer;
        this.finalTurn = finalTurn;
        this.filledMarks = filledMarks;
    }

    public SquareBot(GameManagerBot gmb, Square sq)
    {
        List<MarkBot> pms = new List<MarkBot>();
        foreach (Mark m in sq.presentMarks)
        {
            if (m.sm != null)
            {
                pms.Add(new MarkBot(m.player, m.turn, m.position, new SpookyMarkBot(m.sm)));
            } else
            {
                pms.Add(new MarkBot(m.player, m.turn, m.position, null));
            }
        }
        this.init(gmb, pms, sq.position, sq.alreadyMarked, sq.classicallyMarked, 
            sq.finalPlayer, sq.finalTurn, sq.filledMarks);
    }

    public SquareBot(Square sq)
    {
        List<MarkBot> pms = new List<MarkBot>();
        foreach (Mark m in sq.presentMarks)
        {
            if (m.sm != null)
            {
                pms.Add(new MarkBot(m.player, m.turn, m.position, new SpookyMarkBot(m.sm)));
            }
            else
            {
                pms.Add(new MarkBot(m.player, m.turn, m.position, null));
            }
        }
        this.init(new GameManagerBot(sq.gameManager), pms, sq.position, sq.alreadyMarked, sq.classicallyMarked,
            sq.finalPlayer, sq.finalTurn, sq.filledMarks);
    }

    public override string ToString()
    {
        return "Square " + position.ToString();
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
        
        MarkBot mark = new MarkBot(player, turn, position, null);
        presentMarks.Add(mark);
        alreadyMarked = true;
        gameManager.board.addMark(mark);
    }

    public List<MarkBot> getMarks()
    {
        return presentMarks;
    }

    public void addPresentMark(MarkBot mark)
    {
        presentMarks.Add(mark);
    }

    public void reset()
    {
        alreadyMarked = false;
    }
}
