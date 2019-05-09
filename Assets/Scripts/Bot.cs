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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        // currentTurn = 2;
    }

    private BoardBot generateSuccessor(BoardBot board, int agent, Action action)
    {
        GameManagerBot gmCopy = new GameManagerBot(board.gameManager);
        BoardBot copy = gmCopy.board;
        action.performAction(copy);
        return copy;
    }

    public void executeTurn(int actionType, int turnNum)
    {
        GameManagerBot gmCopy = new GameManagerBot(gameManager);
        BoardBot copy = gmCopy.board;
        Action act = getNextMove(copy, actionType, startDifficulty, turnNum);
        act.performAction(actualBoard);
    }

    public Action getNextMove(BoardBot board, int actionType, int difficulty, int turnNum)
    {
        List<Action> legalmoves = getLegalActions(board, actionType, 1, turnNum); // 1 means bot
        List<int> Scores = new List<int>();

        if (legalmoves == null)
        {
            return new Action();
        }

        Debug.Log("Number of legal moves: " + legalmoves.Count);
        foreach (Action i in legalmoves)
        {
            if (i.actionType == actionType)
            {
                BoardBot successor = generateSuccessor(board, 1, i);
                Debug.Log(i);
                int value = getValue(successor, 1, difficulty);
                Debug.Log("Score: " + value);
                Scores.Add(value);
            }
        }

        int min = Scores.Min();
        int index = Scores.IndexOf(min);
        return legalmoves[index];



        // Perform tree search for best strategy
    }

    private int getValue(BoardBot board, int agent, int depth)
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
        return minValue(board, agent, depth - 1);
    }

    private int maxValue(BoardBot board, int agent, int depth)
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
            int newVal = getValue(successor, (agent + 1) % 2, depth);
            v = v > newVal ? v : newVal;
        }
        return v;
    }

    private int minValue(BoardBot board, int agent, int depth)
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
            int newVal = getValue(successor, (agent + 1) % 2, depth);
            v = v < newVal ? v : newVal;
        }
        return v;
    }

    public Action getNextMoveopponent(BoardBot board, int actionType, int difficulty, int currentTurn)
    {
        List<Action> legalmoves = getLegalActions(board, actionType, 0, currentTurn); // 0 means opponent
        List<double> Scores = new List<double>();


        foreach (Action i in legalmoves)
        {
            if (i.actionType == actionType)
            {
                GameManagerBot gmCopy = new GameManagerBot(gameManager);
                BoardBot copy = gmCopy.board;
                Scores.Add(evalMove(i, copy, 0, difficulty, currentTurn));
            }
        }
        double min = Scores.Min();
        int index = Scores.IndexOf(min);
        return legalmoves[index];



        // Perform tree search for best strategy
    }

    private int evalState(BoardBot board)
    { // return [Xscore,Oscore]
        //Debug.Log("Evaluating board: ");
        //Debug.Log(board);
        //Debug.Log("Checking filled marks: ");
        //foreach (SquareBot sq in board.squares)
        //{
        //    Debug.Log(sq);
        //    Debug.Log("Filled marks: " + sq.filledMarks);
        //}
        int Xscore = evalplayer(board, 0);
        int Oscore = -evalplayer(board, 1);
        return Xscore + Oscore;
    }

    private int evalplayer(BoardBot board, int agent)
    { // return Score
        int score = 0;
        agent++;
        for (int i = 0; i < 9; i++)
        {
            //Debug.Log("Square " + i + ": ");
            SquareBot sq = board.squares[i];
            if (sq.classicallyMarked && sq.finalPlayer != agent)
            {
                continue;
            }

            List<int> neighbors = getNeighbor(i);

            foreach (int k in neighbors)
            {
                //Debug.Log("Neighbor " + k + ": ");
                int gain = 0;
                SquareBot neigh = board.squares[k];
                if (neigh.classicallyMarked && neigh.finalPlayer == agent)
                {
                    //Debug.Log("what?");
                    gain += 20;
                }
                else if (neigh.filledMarks == 0)
                {
                    //Debug.Log("neighbor is empty");
                    gain += 5;
                }
                else
                {
                    foreach (MarkBot m in neigh.presentMarks)
                    {
                        if (m.player == agent)
                        {
                            //Debug.Log("hello");
                            gain += 1;
                        }
                        if(m.player != agent){
                            //Debug.Log("hellooooo");
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


    public double evalMove(Action move, BoardBot board, int agent, int difficulty, int currentTurn)
    {
        double score;
        if (difficulty == 0)
        {
            GameManagerBot gmCopy = new GameManagerBot(gameManager);
            BoardBot copy = gmCopy.board;
            move.performAction(copy);
            score = evalState(copy);
            return score;
        }
        if (agent == 1)
        {
            GameManagerBot gmCopy = new GameManagerBot(gameManager);
            BoardBot copy = gmCopy.board;
            move.performAction(copy);
            Action act = getNextMoveopponent(copy, move.actionType, difficulty - 1, currentTurn);
            act.performAction(copy);
            return evalState(copy);

        }
        else if (agent == 0)
        {
            GameManagerBot gmCopy = new GameManagerBot(gameManager);
            BoardBot copy = gmCopy.board;
            move.performAction(copy);
            Action act = getNextMove(copy, move.actionType, difficulty - 1, currentTurn);
            act.performAction(copy);
            return evalState(copy);
        }
        return 0.0;
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

        //foreach (int[] c in combinations(2, validSquares.Count))
        //{
        //    int p = agent;
        //    result.Add(new Action(c[0], c[1]));
        //}
        
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
