using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    public int startDifficulty;
    private GameManager gameManager;
    public Board actualBoard;
    // public int currentTurn;
    private Transform hidden;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // currentTurn = 2;
        hidden = GameObject.Find("Hidden").transform;
    }

    private Board generateSuccessor(Board board, int agent, Action action)
    {
        Board copy = Instantiate(actualBoard, hidden);
        GameManager gmCopy = Instantiate(gameManager, hidden);
        copy.gameManager = gmCopy;
        action.performAction(board);
        return board;
    }

    public void executeTurn(int actionType, int turnNum)
    {
        Board copy = Instantiate(actualBoard, hidden);
        GameManager gmCopy = Instantiate(gameManager, hidden);
        copy.gameManager = gmCopy;
        Action act = getNextMove(copy, actionType, startDifficulty, turnNum);
        Destroy(copy);
        Destroy(gmCopy);
        act.performAction(actualBoard);
    }

    public Action getNextMove(Board board, int actionType, int difficulty, int turnNum)
    {
        Debug.Log(actionType);
        List<Action> legalmoves = getLegalActions(board, actionType, 1, turnNum); // 1 means bot
        List<int> Scores = new List<int>();


        foreach (Action i in legalmoves)
        {
            if (i.actionType == actionType)
            {
                //Board successor = generateSuccessor(board, 1, i);
                //Scores.Add(getValue(successor, 1, difficulty));
                //Destroy(successor.gameManager);
                //Destroy(successor);
                Scores.Add(2);
            }
        }
        int max = Scores.Max();
        int index = Scores.IndexOf(max);
        return legalmoves[index];



        // Perform tree search for best strategy
    }

    private int getValue(Board board, int agent, int depth)
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
            return maxValue(board, agent, depth - 1);
        }
        return minValue(board, agent, depth);
    }

    private int maxValue(Board board, int agent, int depth)
    {
        int v = int.MinValue;
        foreach (Action action in getLegalActions(board, board.nextAction, agent,
            gameManager.getTurnNum() + startDifficulty - depth))
        {
            Board successor = generateSuccessor(board, agent, action);
            int newVal = getValue(successor, (agent + 1) % 2, depth);
            v = v > newVal ? v : newVal;
            Destroy(successor.gameManager);
            Destroy(successor);
        }
        return v;
    }

    private int minValue(Board board, int agent, int depth)
    {
        int v = int.MaxValue;
        foreach (Action action in getLegalActions(board, board.nextAction, agent,
            gameManager.getTurnNum() + startDifficulty - depth))
        {
            Board successor = generateSuccessor(board, agent, action);
            int newVal = getValue(successor, (agent + 1) % 2, depth);
            v = v < newVal ? v : newVal;
            Destroy(successor.gameManager);
            Destroy(successor);
        }
        return v;
    }

    public Action getNextMoveopponent(Board board, int actionType, int difficulty, int currentTurn)
    {
        List<Action> legalmoves = getLegalActions(board, actionType, 0, currentTurn); // 0 means opponent
        List<double> Scores = new List<double>();


        foreach (Action i in legalmoves)
        {
            if (i.actionType == actionType)
            {
                Board copy = Instantiate(board, hidden);
                GameManager gmCopy = Instantiate(gameManager, hidden);
                copy.gameManager = gmCopy;
                Scores.Add(evalMove(i, copy, 0, difficulty, currentTurn));
                Scores.Add(2.0);
            }
        }
        double min = Scores.Min();
        int index = Scores.IndexOf(min);
        return legalmoves[index];



        // Perform tree search for best strategy
    }

    private int evalState(Board board)
    { // return [Xscore,Oscore]
        int Xscore = evalplayer(board, 0);
        int Oscore = -evalplayer(board, 1);
        return Xscore + Oscore;
    }

    private int evalplayer(Board board, int agent)
    { // return Score

        int score = 0;
        for (int i = 0; i < 9; i++)
        {
            Square sq = board.squares[i];
            if (sq.classicallyMarked && sq.finalPlayer != agent)
            {
                continue;
            }

            List<int> neighbors = getNeighbor(i);
            foreach (int k in neighbors)
            {
                int gain = 0;
                Square neigh = board.squares[k];
                if (neigh.classicallyMarked && neigh.finalPlayer == agent)
                {
                    gain += 20;
                }
                else if (neigh.filledMarks == 0)
                {
                    gain += 5;
                }
                else
                {
                    foreach (Mark m in neigh.presentMarks)
                    {
                        if (m.player == agent)
                        {
                            gain += 1;
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


    public double evalMove(Action move, Board board, int agent, int difficulty, int currentTurn)
    {
        double score;
        if (difficulty == 0)
        {
            Board copy = Instantiate(board, hidden);
            GameManager gmCopy = Instantiate(gameManager, hidden);
            copy.gameManager = gmCopy;
            move.performAction(copy);
            score = evalState(copy);
            Destroy(copy);
            Destroy(gmCopy);
            return score;
        }
        if (agent == 1)
        {
            Board copy = Instantiate(board, hidden);
            GameManager gmCopy = Instantiate(gameManager, hidden);
            copy.gameManager = gmCopy;
            move.performAction(copy);
            Action act = getNextMoveopponent(copy, move.actionType, difficulty - 1, currentTurn);
            act.performAction(copy);
            Destroy(copy);
            Destroy(gmCopy);
            return evalState(copy);

        }
        else if (agent == 0)
        {
            Board copy = Instantiate(board, hidden);
            GameManager gmCopy = Instantiate(gameManager, hidden);
            copy.gameManager = gmCopy;
            move.performAction(copy);
            Action act = getNextMove(copy, move.actionType, difficulty - 1, currentTurn);
            act.performAction(copy);
            Destroy(copy);
            Destroy(gmCopy);
            return evalState(copy);
        }
        return 0.0;
    }

    private List<Action> getLegalActions(Board board, int actionType, int agent, int turnNum)
    {
        // Generate all possible actions

        List<Action> result = new List<Action>();

        if (actionType == 1)
        {
            // return the two possible squares we can collapse into.
            SpookyMark sm = board.toCollapse;
            if (sm == null)
            {
                //log error
                Debug.Log("ERROR: Illegal action requested from bot.");
                return null;
            }
            int pos1 = sm.position1;
            int pos2 = sm.position2;
            result.Add(new Action(board.squares[pos1]));
            result.Add(new Action(board.squares[pos2]));
            return result;
        }

        // Otherwise we return all the possible spookyMarks to add
        // First find all possible squares we can put marks on.
        List<int> validSquares = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            Square sq = board.squares[i];
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

        foreach (int[] c in combinations(2, validSquares.Count))
        {
            int p = agent;
            result.Add(new Action(c[0], c[1]));
        }
        return result;
    }

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
        int r = sq % 3;
        int c = sq - 3 * r;

        List<int> neighbors = new List<int>();
        for (int i = r - 1; i <= r + 1; i++)
        {
            for (int j = c - 1; j <= c + 1; j++)
            {
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
