using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMarkBot
{
    public int player;
    public int turn;
    public int position1;
    public int position2;
    int finalPosition;
    MarkBot mark1;
    MarkBot mark2;
    private bool finalized;

    public void init(MarkBot mark1, MarkBot mark2)
    {
        this.mark1 = mark1;
        this.mark2 = mark2;
        this.player = mark1.player;
        this.turn = mark1.turn;
        this.position1 = mark1.position;
        this.position2 = mark2.position;
        this.finalPosition = -1;
        this.finalized = false;
    }
    
    public SpookyMarkBot(MarkBot mark1, MarkBot mark2)
    {
        this.mark1 = mark1;
        this.mark2 = mark2;
        this.player = mark1.player;
        this.turn = mark1.turn;
        this.position1 = mark1.position;
        this.position2 = mark2.position;
        this.finalPosition = -1;
        this.finalized = false;
    }

    public SpookyMarkBot(SpookyMark sm)
    {
        MarkBot mark1 = new MarkBot(sm.mark1);
        mark1.sm = this;
        MarkBot mark2 = new MarkBot(sm.mark2);
        mark2.sm = this;
        this.init(mark1, mark2);
    }

    public SpookyMarkBot(SpookyMarkBot sm)
    {
        MarkBot mark1 = new MarkBot(sm.mark1);
        mark1.sm = this;
        MarkBot mark2 = new MarkBot(sm.mark2);
        mark2.sm = this;
        this.init(mark1, mark2);
    }

    public void setFinal(int pos)
    {
        this.position1 = -1;
        this.position2 = -1;
        this.finalPosition = pos;
        this.finalized = true;
    }

    public override string ToString()
    {
        return string.Format("Player {0}, Turn {1}: ({2}, {3})", player, turn, position1, position2);
    }

    public bool isFinal()
    {
        return this.finalized;
    }

    public override bool Equals(object obj)
    {
        SpookyMarkBot sm = (SpookyMarkBot)obj;
        return player == sm.player && turn == sm.turn && position1 == sm.position1 && position2 == sm.position2;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
