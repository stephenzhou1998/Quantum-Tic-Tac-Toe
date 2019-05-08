using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    public Graph entGraph;
    public Square[] squares;
    public List<SpookyMark> spookyMarks;
    public List<int> collapsed;
    public List<Mark> currentSpookyMark;
    public SpookyMark toCollapse;
    public GameObject bigX;
    public GameObject bigO;
    public int nextAction; // 0: next player should place a spooky mark. 1: next player should pick a square to collapse.
    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        entGraph = new Graph();
        squares = new Square[9];
        spookyMarks = new List<SpookyMark>();
        currentSpookyMark = new List<Mark>();
        collapsed = new List<int>();
        int i = 0;
        foreach (Transform square in transform)
        {
            squares[i] = square.gameObject.GetComponent<Square>();
            i++;
        }
        toCollapse = null;
    }

    public void addMark(Mark mark)
    {
        gameManager.numMarks++;
        currentSpookyMark.Add(mark);
        if (gameManager.numMarks == 2)
        {
            gameManager.nextTurn();
        }
    }

    public SpookyMark addSpookyMark()
    {
        Mark mark1 = currentSpookyMark[0];
        Mark mark2 = currentSpookyMark[1];
        currentSpookyMark = new List<Mark>();
        SpookyMark s = new SpookyMark(mark1, mark2);
        mark1.sm = s;
        mark2.sm = s;
        spookyMarks.Add(s);

        if (entGraph.addEdgeSQ(mark1.square, mark2.square))
        {
            toCollapse = s;
            return s;
        }

        HashSet<Square> sqCycle = entGraph.getCycleSQ(mark1.square);
        if (sqCycle != null)
        {
            toCollapse = s;
            return s;
        } else
        {
            return null;
        }
    }

    public int checkWin()
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
            foreach (Square sq in squares)
            {
                if (!sq.classicallyMarked)
                {
                    return -1;
                }
            }
            gameManager.noWinner();
            return -1;
        } else if (winPlayers.Count == 1)
        {
            gameManager.win(winPlayers[0]);
            return winPlayers[0] - 1;
        } else
        {
            if (winPlayers[0] == winPlayers[1])
            {
                gameManager.win(winPlayers[0]);
                return winPlayers[0] - 1;
            } else {
                if (winMaxTurns[0] < winMaxTurns[1])
                {
                    gameManager.win(winPlayers[0]);
                    return winPlayers[0] - 1;
                } else if (winMaxTurns[0] > winMaxTurns[1]) {
                    gameManager.win(winPlayers[1]);
                    return winPlayers[0] - 1;
                }
                return -1;
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
