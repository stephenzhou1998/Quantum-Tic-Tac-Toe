using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    Graph entGraph;
    List<Square> squares;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        entGraph = new Graph();
        squares = new ArrayList<Square>();
        for (int i=0; i < 9; i++)
        {
            squares.Add(new Square());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void addSpookyMark(int pos1, int pos2)
    {
        squares[pos1].addMark();
        squares[pos2].addMark();

        int player = gameManager.getCurrentPlayer();
        int turn = gameManager.getTurnNum();
        Mark newMark = new Mark(player, turn);
    }
}
