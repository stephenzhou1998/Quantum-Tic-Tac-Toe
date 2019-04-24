using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    Graph entGraph;
    public Square[] squares;
    List<SpookyMark> spookyMarks;
    List<int> collapsed;
    public SpookyMark toCollapse;
    public GameObject bigX;
    public GameObject bigO;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        entGraph = new Graph();
        squares = new Square[9];
        spookyMarks = new List<SpookyMark>();
        collapsed = new List<int>();
        int i = 0;
        foreach (Transform square in transform)
        {
            squares[i] = square.gameObject.GetComponent<Square>();
            i++;
        }
        toCollapse = null;
    }

    public SpookyMark addSpookyMark(Mark mark1, Mark mark2)
    {
        // Commented out code uses SpookyMarks to detect cycles, but that was buggy and 
        // too complicated, so now we are using Squares to detect cycles.
        SpookyMark s = new SpookyMark(mark1, mark2);
        mark1.sm = s;
        mark2.sm = s;
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

        if (entGraph.addEdgeSQ(mark1.square, mark2.square))
        {
            Debug.Log("cycle detected (two square cycle)");
            toCollapse = s;
            return s;
        }

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
            toCollapse = s;
            return s;
        } else
        {
            Debug.Log("cycle not detected");
            return null;
        }
    }

    public void checkWin()
    {
        List<int> winPlayers = new List<int>();
        List<int> winMaxTurns = new List<int>();
        // Top row
        checkWinHelper(0, 1, 2, winPlayers, winMaxTurns);

        // Middle row
        checkWinHelper(3, 4, 5, winPlayers, winMaxTurns);

        // Bottom row
        checkWinHelper(6, 7, 8, winPlayers, winMaxTurns);

        // Left column
        checkWinHelper(0, 3, 6, winPlayers, winMaxTurns);

        // Middle column
        checkWinHelper(1, 4, 7, winPlayers, winMaxTurns);

        // Right column
        checkWinHelper(2, 5, 8, winPlayers, winMaxTurns);

        // Left diagonal
        checkWinHelper(0, 4, 8, winPlayers, winMaxTurns);

        // Right diagonal
        checkWinHelper(2, 4, 6, winPlayers, winMaxTurns);

        if (winPlayers.Count == 0)
        {
            return;
        } else if (winPlayers.Count == 1)
        {
            gameManager.win(winPlayers[0]);
        } else
        {
            if (winPlayers[0] == winPlayers[1])
            {
                gameManager.win(winPlayers[0]);
            } else {
                if (winMaxTurns[0] < winMaxTurns[1])
                {
                    gameManager.win(winPlayers[0]);
                } else if (winMaxTurns[0] > winMaxTurns[1]) {
                    gameManager.win(winPlayers[1]);
                }
            }
        }
    }

    public void checkWinHelper(int a, int b, int c, List<int> winPlayers, List<int>winMaxTurns)
    {
        if (squares[a].classicallyMarked && squares[b].classicallyMarked && squares[c].classicallyMarked)
        {
            if (squares[a].finalPlayer == squares[b].finalPlayer && squares[b].finalPlayer == squares[c].finalPlayer)
            {
                winPlayers.Add(squares[a].finalPlayer);
                int maxTurn = System.Math.Max(squares[a].finalTurn,
                    System.Math.Max(squares[b].finalTurn, squares[c].finalTurn));
                winMaxTurns.Add(maxTurn);
            }
        }
    }

    public void collapse(int mark)
    {
        int position = 0;
        if (mark == 1)
        {
            position = toCollapse.position1;
        } else if (mark == 2)
        {
            position = toCollapse.position2;
        }
        int player = toCollapse.player;
        int turn = toCollapse.turn;
        toCollapse = null;
        collapseHelper(position, player, turn);
        gameManager.finishCollapse();
        // Might not be necessary
        // entGraph.removeCycleSQ(squares[position]);
        checkWin();
    }

    public void collapseHelper(int position, int player, int turn)
    {
        if (collapsed.Contains(position))
        {
            return;
        }
        Square square = squares[position];
        GameObject toAdd = null;
        string restore = "";
        if (player == 1)
        {
            toAdd = bigX;
            restore = "X";
        }
        else if (player == 2)
        {
            toAdd = bigO;
            restore = "O";
        }
        toAdd.GetComponent<TextMeshProUGUI>().text += "<sub>" + turn.ToString() + "</sub>";
        square.setBigMark(toAdd, restore, player, turn);
        collapsed.Add(position);
        
        // Check all marks in the square S collapsed into. Each corresponding
        // SpookyMark collapses into the square S' that is NOT S. Then, recursively
        // check each of those squares S'.
        foreach (Mark mark in square.getMarks())
        {
            SpookyMark sm = mark.sm;
            int spookyPosition;
            if (sm.position1 == position)
            {
                spookyPosition = sm.position2;
            } else
            {
                spookyPosition = sm.position1;
            }
            collapseHelper(spookyPosition, sm.player, sm.turn);
        }
    }
}
