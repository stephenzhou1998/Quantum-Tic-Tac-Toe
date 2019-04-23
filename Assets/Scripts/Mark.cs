using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour
{
    public int player;
    public int turn;

    public Mark(int p, int t)
    {
        player = p;
        turn = t;
    }
}
