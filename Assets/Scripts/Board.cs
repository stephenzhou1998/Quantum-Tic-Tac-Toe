using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    Graph entGraph;
    Square[] squares;
    List<SpookyMark> spookyMarks;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        entGraph = new Graph();
        squares = new Square[9];
        spookyMarks = new List<SpookyMark>();
        int i = 0;
        foreach (Transform square in transform)
        {
            squares[i] = square.gameObject.GetComponent<Square>();
        }
    }

    public SpookyMark addSpookyMark(Mark mark1, Mark mark2)
    {
        // Commented out code uses SpookyMarks to detect cycles, but that was buggy and 
        // too complicated, so now we are using Squares to detect cycles.
        SpookyMark s = new SpookyMark(mark1, mark2);
        //mark1.sm = s;
        //mark2.sm = s;
        //entGraph.addVertex(s);
        spookyMarks.Add(s);
        //Debug.Log("Current Spooky Marks: ");
        //foreach (SpookyMark sm in spookyMarks)
        //{
        //    Debug.Log(sm.ToString());
        //}
        //List<Mark> mark1Marks = mark1.square.getMarks();
        //List<Mark> mark2Marks = mark2.square.getMarks();

        //foreach (Mark m in mark1Marks)
        //{
        //    entGraph.addEdge(s, m.sm);
        //}
        //mark1.square.addPresentMark(mark1);

        //foreach (Mark m in mark2Marks)
        //{
        //    entGraph.addEdge(s, m.sm);
        //}
        //mark2.square.addPresentMark(mark2);

        entGraph.addEdgeSQ(mark1.square, mark2.square);

        // Finds cycles based on connections between Spooky Marks (buggy)
        //HashSet<SpookyMark> cycle = entGraph.getCycle(s);
        //if (cycle != null)
        //{
        //    Debug.Log("cycle detected: ");
        //    foreach (SpookyMark sm in cycle)
        //    {
        //        Debug.Log(sm.ToString());
        //    }
        //} else
        //{
        //    Debug.Log("cycle not detected");
        //}

        // Finds cycles based on connections between squares (probably the better way)
        HashSet<Square> sqCycle = entGraph.getCycleSQ(mark1.square);
        if (sqCycle != null)
        {
            Debug.Log("cycle detected: ");
            foreach (Square sq in sqCycle)
            {
                Debug.Log(sq.ToString());
            }
            return s;
        } else
        {
            Debug.Log("cycle not detected");
            return null;
        }
    }

    public void collapse(int position)
    {

    }
}
