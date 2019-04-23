using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    Graph entGraph;
    List<Square> squares;
    List<SpookyMark> spookyMarks;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        entGraph = new Graph();
        squares = new List<Square>();
        spookyMarks = new List<SpookyMark>();
        foreach (Transform square in transform)
        {
            squares.Add(square.gameObject.GetComponent<Square>());
        }
    }

    public void addSpookyMark(Mark mark1, Mark mark2)
    {
        SpookyMark s = new SpookyMark(mark1, mark2);
        spookyMarks.Add(s);
        Debug.Log("Current Spooky Marks: ");
        foreach (SpookyMark sm in spookyMarks)
        {
            Debug.Log(sm.ToString());
        }
    }
}
