using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBot
{
    public int player;
    public int turn;
    public int position;
    // The SpookyMark that this mark is part of
    public SpookyMarkBot sm;

    public void init(int p, int t, int pos, SpookyMarkBot sm)
    {
        player = p;
        turn = t;
        position = pos;
        this.sm = sm;
    }

    public MarkBot(int p, int t, int pos, SpookyMarkBot sm)
    {
        player = p;
        turn = t;
        position = pos;
        this.sm = sm;
    }

    public MarkBot(MarkBot m)
    {
        this.init(m.player, m.turn, m.position, m.sm);
    }

    public MarkBot(Mark m)
    {
        this.init(m.player, m.turn, m.position, null);
    }

    public override string ToString()
    {
        return string.Format("Player {0}, Turn {1}: Position {2}", player, turn, position);
    }
}
