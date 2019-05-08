﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // 0 means placing new spookyMarks, 1 means choosing square to collapse
    public int actionType;
    int p1;
    int p2;
    Square sq;
    SquareBot sqBot;

    public Action(int position1, int position2)
    {
        actionType = 0;
        this.p1 = position1;
        this.p2 = position2;
        sq = null;
    }

    public Action(Square toCollapse)
    {
        actionType = 1;
        sq = toCollapse;
    }

    public Action(SquareBot toCollapse)
    {
        actionType = 1;
        sqBot = toCollapse;
    }

    public void performAction(Board board)
    {
        if (actionType == 0)
        {
            board.squares[p1].addMark();
            board.squares[p2].addMark();
        }
        else if (actionType == 1)
        {
            SpookyMark toCollapse = board.toCollapse;
            int positionToCollapse = sq.position;
            if (positionToCollapse == toCollapse.position1)
            {
                board.collapse(1);
            }
            else if (positionToCollapse == toCollapse.position2)
            {
                board.collapse(2);
            }
        }
    }

    public void performAction(BoardBot board)
    {
        if (actionType == 0)
        {
            board.squares[p1].addMark();
            board.squares[p2].addMark();
        }
        else if (actionType == 1)
        {
            SpookyMarkBot toCollapse = board.toCollapse;
            int positionToCollapse = sq.position;
            if (positionToCollapse == toCollapse.position1)
            {
                board.collapse(1);
            }
            else if (positionToCollapse == toCollapse.position2)
            {
                board.collapse(2);
            }
        }
    }
}
