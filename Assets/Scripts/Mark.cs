using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public int player;
    public int turn;
    public int position;
    // The square that this Mark is in
    public Square square;
    // The SpookyMark that this mark is part of
    public SpookyMark sm;

    public Mark(int p, int t, int pos, Square sq)
    {
        player = p;
        turn = t;
        position = pos;
        square = sq;
        sm = null;
    }

    public string ToString()
    {
        return string.Format("Player {0}, Turn {1}: Position {2}", player, turn, position);
    }
}
