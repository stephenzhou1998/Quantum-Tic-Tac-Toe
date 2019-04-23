using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMark : MonoBehaviour
{
	int player;
	int turn;
	int position1;
	int position2;
	int finalPosition;
	private bool finalized;

	public SpookyMark(int player, int turn, int p1, int p2)
	{
		this.player = player;
		this.turn = turn;
		this.position1 = p1;
		this.position2 = p2;
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

	public bool isFinal()
	{
		return this.finalized;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
