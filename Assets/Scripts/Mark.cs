﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public int player;
    public int turn;
    public int position;

    public Mark(int p, int t, int pos)
    {
        player = p;
        turn = t;
        position = pos;
    }
}
