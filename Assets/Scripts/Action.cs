using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    // 0 means placing new spookyMarks, 1 means choosing square to collapse, 2 means do nothing
    public int actionType;
    int p1;
    int p2;
    SquareBot sqBot;

    public Action(int position1, int position2)
    {
        actionType = 0;
        this.p1 = position1;
        this.p2 = position2;
        sqBot = null;
    }

    public Action(SquareBot toCollapse)
    {
        actionType = 1;
        sqBot = toCollapse;
    }

    public Action()
    {
        actionType = 2;
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
            int positionToCollapse = sqBot.position;
            Debug.Log("collapsing position: " + positionToCollapse);
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
            int positionToCollapse = sqBot.position;
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

    public override string ToString()
    {
        string ret = "Action: ";
        if (actionType == 0)
        {
            ret += "Placing new SpookyMark: " + p1 + ", " + p2;
        } else if (actionType == 1)
        {
            ret += "Collapsing " + sqBot;
        } else if (actionType == 2)
        {
            ret += "Do nothing.";
        }
        return ret;
    }
}
