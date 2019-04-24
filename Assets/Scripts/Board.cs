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
        mark1.sm = s;
        mark2.sm = s;
        entGraph.addVertex(s);
        spookyMarks.Add(s);
        //Debug.Log("Current Spooky Marks: ");
        //foreach (SpookyMark sm in spookyMarks)
        //{
        //    Debug.Log(sm.ToString());
        //}
        List<Mark> mark1Marks = mark1.square.getMarks();
        List<Mark> mark2Marks = mark2.square.getMarks();

        foreach (Mark m in mark1Marks)
        {
            entGraph.addEdge(s, m.sm);
        }
        mark1.square.addPresentMark(mark1);

        foreach (Mark m in mark2Marks)
        {
            entGraph.addEdge(s, m.sm);
        }
        mark2.square.addPresentMark(mark2);

        HashSet<SpookyMark> cycle = entGraph.getCycle(s);
        if (cycle != null)
        {
            Debug.Log("cycle detected: ");
            foreach (SpookyMark sm in cycle)
            {
                Debug.Log(sm.ToString());
            }
        } else
        {
            Debug.Log("cycle not detected");
        }
    }
}
