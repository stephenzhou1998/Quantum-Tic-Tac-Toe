using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    public int startDifficulty;
    private GameManager gameManager;
    public Board actualBoard;
    private int numCopies;
    private static Random rnd;
    // public int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // currentTurn = 2;
        numCopies = 0;
        rnd = new Random();
    }

    private BoardBot generateSuccessor(BoardBot board, int agent, Action action)
    {
        numCopies++;
        GameManagerBot gmCopy = new GameManagerBot(board.gameManager);
        BoardBot copy = gmCopy.board;
        action.performAction(copy);
        return copy;
    }

    public void executeTurn(int actionType, int turnNum)
    {
        numCopies = 0;
        GameManagerBot gmCopy = new GameManagerBot(gameManager);
        BoardBot copy = gmCopy.board;
        Action act = getNextMove(copy, actionType, startDifficulty, turnNum);
        act.performAction(actualBoard);
        Debug.Log("Made " + numCopies + " copies.");
    }

    public Action getNextMove(BoardBot board, int actionType, int difficulty, int turnNum)
    {
        List<Action> legalmoves = getLegalActions(board, actionType, 1, turnNum); // 1 means bot
        List<int> Scores = new List<int>();

        if (legalmoves == null)
        {
            return new Action();
        }

        int alpha = int.MinValue;
        int beta = int.MaxValue;

        Debug.Log("Number of legal moves: " + legalmoves.Count);
        foreach (Action i in legalmoves)
        {
            if (i.actionType == actionType)
            {
                BoardBot successor = generateSuccessor(board, 1, i);
                Debug.Log(i);
                int val = getValue(successor, 1, difficulty, alpha, beta);
                if (val < alpha)
                {
                    break;
                }
                beta = beta < val ? beta : val;
                Debug.Log("Score: " + val);
                Scores.Add(val);
            }
        }

        int min = Scores.Min();
        int index = Scores.IndexOf(min);

        // Build a list of indices of all min scores
        List<int> minScoreIndices = new List<int>();
        for (int i = 0; i < Scores.Count; i++)
        {
            int s = Scores[i];
            if (s == min)
            {
                minScoreIndices.Add(i);
            }
        }

        // Now randomly choose one from the min scores
        return legalmoves[minScoreIndices[Random.Range(0, minScoreIndices.Count)]];
    }

    private int getValue(BoardBot board, int agent, int depth, int alph, int beta)
    {
        int winner = board.checkWin();
        if (winner == 0)
        {
            return int.MaxValue;
        }
        else if (winner == 1)
        {
            return int.MinValue;
        }
        else if (depth == 0)
        {
            return evalState(board);
        }
        if (agent == 0)
        {
            return maxValue(board, agent, depth - 1, alph, beta);
        }
        return minValue(board, agent, depth - 1, alph, beta);
    }

    private int maxValue(BoardBot board, int agent, int depth, int alph, int beta)
    {
        List<Action> legalmoves = getLegalActions(board, board.nextAction, agent,
            gameManager.getTurnNum() + startDifficulty - depth);

        int v = int.MinValue;
        if (legalmoves == null)
        {
            return v;
        }
        foreach (Action action in legalmoves)
        {
            BoardBot successor = generateSuccessor(board, 1, action);
            int newVal = getValue(successor, (agent + 1) % 2, depth, alph, beta);
            v = v > newVal ? v : newVal;
            if (v > beta)
            {
                return v;
            }
            alph = alph > v ? alph : v;
        }
        return v;
    }

    private int minValue(BoardBot board, int agent, int depth, int alph, int beta)
    {
        List<Action> legalmoves = getLegalActions(board, board.nextAction, agent,
            gameManager.getTurnNum() + startDifficulty - depth);
        int v = int.MaxValue;
        if (legalmoves == null)
        {
            return v;
        }
        foreach (Action action in legalmoves)
        {
            BoardBot successor = generateSuccessor(board, 1, action);
            int newVal = getValue(successor, (agent + 1) % 2, depth, alph, beta);
            v = v < newVal ? v : newVal;
            if (v < alph)
            {
                return v;
            }
            beta = beta < v ? beta : v;
        }
        return v;
    }

    private int evalState(BoardBot board)
    {
        int Xscore = evalplayer(board, 0);
        int Oscore = -evalplayer(board, 1);
        return Xscore + Oscore;
    }

    private int evalplayer(BoardBot board, int agent)
    { // return Score
        int score = 0;
        agent++;
        // Look through every square
        for (int i = 0; i < 9; i++)
        {
            //Debug.Log("Square " + i + ": ");
            SquareBot sq = board.squares[i];
            if (sq.classicallyMarked && sq.finalPlayer != agent)
            {
                continue;
            }

            List<int> neighbors = getNeighbor(i);

            // Look into every neighbor
            foreach (int k in neighbors)
            {
                int gain = 0;
                SquareBot neigh = board.squares[k];

                // Always good to be next to your own classical mark
                if (neigh.classicallyMarked && neigh.finalPlayer == agent)
                {
                    gain += 100;
                }
                // Always good to have empty neighbor. More chance for expansion.
                else if (neigh.filledMarks == 0)
                {
                    gain += 5;
                }

                // Last case is when neighbor has spooky marks
                else
                {
                    // more marks means more competition. Gotta win!
                    gain += 10 * neigh.presentMarks.Count;

                    // Penalize if spooky mark is the opponent
                    foreach (MarkBot m in neigh.presentMarks)
                    {
                        if (m.player == agent)
                        {
                            gain += 2;
                        }
                        if(m.player != agent) {
                            gain -= 5;
                        }
                    }
                }
                if (sq.classicallyMarked)
                {
                    gain *= 2;
                }
                score += gain;
            }
        }
        return score;
    }

    private List<Action> getLegalActions(BoardBot board, int actionType, int agent, int turnNum)
    {
        // Generate all possible actions

        List<Action> result = new List<Action>();

        if (actionType == 1)
        {
            // return the two possible squares we can collapse into.
            SpookyMarkBot sm = board.toCollapse;
            if (sm == null)
            {
                //log error
                Debug.Log("ERROR: Illegal action requested from bot.");
                return null;
            }
            int pos1 = sm.position1;
            int pos2 = sm.position2;
            Debug.Log("Will collapse position: " + board.squares[pos1].position);
            result.Add(new Action(board.squares[pos1]));
            result.Add(new Action(board.squares[pos2]));
            return result;
        }

        // Otherwise we return all the possible spookyMarks to add
        // First find all possible squares we can put marks on.
        List<int> validSquares = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            SquareBot sq = board.squares[i];
            if (!sq.classicallyMarked && sq.filledMarks < 8)
            {
                validSquares.Add(i);
            }
        }
        // If no valid squares, return null
        if (validSquares.Count == 0)
        {
            return null;
        }
        // If only one valid square, duplicate it so combinations(2, count) doesn't freak out
        if (validSquares.Count == 1)
        {
            validSquares.Add(validSquares[0]);
        }
        
        foreach (int first in validSquares)
        {
            foreach (int second in validSquares)
            {
                if (second <= first)
                {
                    continue;
                }
                result.Add(new Action(first, second));
            }
        }
        return result;
    }

// Citation:
// www.technical-recipes.com/2017/obtaining-combinations-of-k-elements-from-n-in-c/
    private IEnumerable<int[]> combinations(int m, int n)
    {
        int[] result = new int[m];
        Stack<int> stack = new Stack<int>();
        stack.Push(0);

        while (stack.Count > 0)
        {
            int index = stack.Count - 1;
            int value = stack.Pop();

            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);

                if (index == m)
                {
                    yield return result;
                    break;
                }
            }
        }
    }

    private List<int> getNeighbor(int sq)
    {
        // return a list of indices of squares that are neighbors of sq
        int r = sq / 3;
        int c = sq % 3;

        List<int> neighbors = new List<int>();
        for (int i = r - 1; i <= r + 1; i++)
        {
            for (int j = c - 1; j <= c + 1; j++)
            {
                if (i < 0 || j < 0 || i > 2 || j > 2)
                {
                    continue;
                }
                int index = 3 * i + j;
                if (index != sq && index >= 0 && index < 9)
                {
                    neighbors.Add(index);
                }
            }
        }
        return neighbors;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
