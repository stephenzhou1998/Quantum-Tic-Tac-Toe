using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBot
{
    public int player;
    public int turn;
    public int position;
    // The square that this Mark is in
    // The SpookyMark that this mark is part of
    public SpookyMarkBot sm;

    public void init(int p, int t, int pos)
    {
        player = p;
        turn = t;
        position = pos;
        sm = null;
    }

    public MarkBot(int p, int t, int pos)
    {
        player = p;
        turn = t;
        position = pos;
        sm = null;
    }

    public MarkBot(Mark m)
    {
        this.init(m.player, m.turn, m.position);
    }

    public string ToString()
    {
        return string.Format("Player {0}, Turn {1}: Position {2}", player, turn, position);
    }
}
