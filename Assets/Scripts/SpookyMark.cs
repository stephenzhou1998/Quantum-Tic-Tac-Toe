﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMark : MonoBehaviour
{
	int player;
	int turn;
	int position1;
	int position2;
	int finalPosition;
    Mark mark1;
    Mark mark2;
	private bool finalized;

	public SpookyMark(Mark mark1, Mark mark2)
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

	public void setFinal(int pos)
	{
		this.position1 = -1;
		this.position2 = -1;
		this.finalPosition = pos;
		this.finalized = true;
	}

    public string ToString()
    {
        return string.Format("Player {0}, Turn {1}: ({2}, {3})", player, turn, position1, position2);
    }

	public bool isFinal()
	{
		return this.finalized;
	}
}
