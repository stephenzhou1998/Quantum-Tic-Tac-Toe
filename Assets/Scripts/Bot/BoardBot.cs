using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoardBot
{
    GraphBot entGraph;
    public SquareBot[] squares;
    List<SpookyMarkBot> spookyMarks;
    public List<int> collapsed;
    public SpookyMarkBot toCollapse;
    public List<MarkBot> currentSpookyMark;
    public int nextAction; // 0: next player should place a spooky mark. 1: next player should pick a square to collapse.
    public GameManagerBot gameManager;

    // Start is called before the first frame update
    public void init(GraphBot entGraph, SquareBot[] squares, List<SpookyMarkBot> spookyMarks, 
        List<int> collapsed, SpookyMarkBot toCollapse, List<MarkBot> currentSpookyMark, int nextAction)
    {
        this.entGraph = entGraph;
        this.squares = squares;
        this.spookyMarks = spookyMarks;
        this.collapsed = collapsed;
        this.toCollapse = toCollapse;
        this.currentSpookyMark = currentSpookyMark;
        this.nextAction = nextAction;
    }

    public BoardBot(BoardBot b, GameManagerBot gameManager)
    {
        this.gameManager = gameManager;
        SquareBot[] s = b.squares;
        SquareBot[] sb = new SquareBot[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            sb[i] = new SquareBot(gameManager, s[i]);
        }
        List<SpookyMarkBot> smb = new List<SpookyMarkBot>();
        foreach (SpookyMarkBot sm in b.spookyMarks)
        {
            smb.Add(new SpookyMarkBot(sm));
        }
        List<MarkBot> mb = new List<MarkBot>();
        foreach (MarkBot m in b.currentSpookyMark)
        {
            mb.Add(new MarkBot(m));
        }
        List<int> collapsed = new List<int>(b.collapsed);

        if (b.toCollapse == null)
        {
            this.init(new GraphBot(b.entGraph, gameManager), sb, smb, collapsed,
            null, mb, b.nextAction);
        }
        else
        {
            this.init(new GraphBot(b.entGraph, gameManager), sb, smb, collapsed,
                new SpookyMarkBot(b.toCollapse), mb, b.nextAction);
        }
    }

    public BoardBot(Board b, GameManagerBot gameManager)
    {
        this.gameManager = gameManager;
        Square[] s = b.squares;
        SquareBot[] sb = new SquareBot[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            sb[i] = new SquareBot(gameManager, s[i]);
        }
        List<SpookyMarkBot> smb = new List<SpookyMarkBot>();
        foreach (SpookyMark sm in b.spookyMarks)
        {
            smb.Add(new SpookyMarkBot(sm));
        }
        List<MarkBot> mb = new List<MarkBot>();
        foreach (Mark m in b.currentSpookyMark)
        {
            mb.Add(new MarkBot(m));
        }
        List<int> collapsed = new List<int>(b.collapsed);

        if (b.toCollapse == null)
        {
            this.init(new GraphBot(b.entGraph, gameManager), sb, smb, collapsed,
            null, mb, b.nextAction);
        } else
        {
            this.init(new GraphBot(b.entGraph, gameManager), sb, smb, collapsed,
                new SpookyMarkBot(b.toCollapse), mb, b.nextAction);
        }
    }

    public void addMark(MarkBot mark)
    {
        gameManager.numMarks++;
        currentSpookyMark.Add(mark);
        if (gameManager.numMarks == 2)
        {
            gameManager.nextTurn();
        }
    }

    public SpookyMarkBot addSpookyMark()
    {
        MarkBot mark1 = currentSpookyMark[0];
        MarkBot mark2 = currentSpookyMark[1];
        currentSpookyMark = new List<MarkBot>();
        SpookyMarkBot s = new SpookyMarkBot(mark1, mark2);
        mark1.sm = s;
        mark2.sm = s;
        spookyMarks.Add(s);

        if (entGraph.addEdgeSQ(squares[mark1.position], squares[mark2.position]))
        {
            toCollapse = s;
            return s;
        }

        HashSet<SquareBot> sqCycle = entGraph.getCycleSQ(squares[mark1.position]);
        if (sqCycle != null)
        {
            toCollapse = s;
            return s;
        }
        else
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
            foreach (SquareBot sq in squares)
            {
                if (!sq.classicallyMarked)
                {
                    return -1;
                }
            }
            gameManager.noWinner();
            return -1;
        }
        else if (winPlayers.Count == 1)
        {
            gameManager.win(winPlayers[0]);
            return winPlayers[0] - 1;
        }
        else
        {
            if (winPlayers[0] == winPlayers[1])
            {
                gameManager.win(winPlayers[0]);
                return winPlayers[0] - 1;
            }
            else
            {
                if (winMaxTurns[0] < winMaxTurns[1])
                {
                    gameManager.win(winPlayers[0]);
                    return winPlayers[0] - 1;
                }
                else if (winMaxTurns[0] > winMaxTurns[1])
                {
                    gameManager.win(winPlayers[1]);
                    return winPlayers[1] - 1;
                }
                return -1;
            }
        }
    }

    public void checkWinHelper(int a, int b, int c, List<int> winPlayers, List<int> winMaxTurns)
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
        }
        else if (mark == 2)
        {
            position = toCollapse.position2;
        }
        int player = toCollapse.player;
        int turn = toCollapse.turn;
        toCollapse = null;
        collapseHelper(position, player, turn);
        checkWin();
    }

    public void collapseHelper(int position, int player, int turn)
    {
        if (collapsed.Contains(position))
        {
            return;
        }
        SquareBot square = squares[position];
        square.setBigMark(player, turn);
        collapsed.Add(position);

        // Check all marks in the square S collapsed into. Each corresponding
        // SpookyMark collapses into the square S' that is NOT S. Then, recursively
        // check each of those squares S'.
        foreach (MarkBot mark in square.getMarks())
        {
            SpookyMarkBot sm = mark.sm;
            int spookyPosition;
            if (sm.position1 == position)
            {
                spookyPosition = sm.position2;
            }
            else
            {
                spookyPosition = sm.position1;
            }
            collapseHelper(spookyPosition, sm.player, sm.turn);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("Board: \n");
        foreach (SquareBot sq in squares)
        {
            sb.Append(sq);
        }
        return sb.ToString();
    }
}
