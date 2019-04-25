using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    public int difficulty;
    private Transform hidden;
    private GameManager gameManager;
    public Board actualBoard;
    public int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        hidden = GameObject.Find("Hidden").transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentTurn = 2;
    }

    private BoardBot generateSuccessor(Board board, int agent, Action action)
    {
        BoardBot copy = new BoardBot(board);
        GameManagerBot gmCopy = new GameManagerBot(board.gameManager);
        copy.gameManager = gmCopy;
        action.performAction(copy);
        return copy;
    }

    public Action getNextMove(BoardBot board, int actionType, int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 1, currentTurn); // 1 means bot
        List<double> Scores = new List<double>();

        
        foreach (Action i in legalmoves){
            if(i.actionType == actionType){
                BoardBot copy = board.shallowCopy();
                Scores.Add(evalMove(i,copy,1,difficulty));
            }
        }
        double max = Scores.Max();
        int index = Scores.IndexOf(max);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }

    public Action getNextMoveopponent(BoardBot board,int actionType,int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 0, currentTurn); // 0 means opponent
        List<double> Scores = new List<double>();

        
        foreach (Action i in legalmoves){
            if(i.actionType == actionType)
            {
                BoardBot copy = board.shallowCopy();
                Scores.Add(evalMove(i,copy,0,difficulty));
            }
        }
        double min = Scores.Min();
        int index = Scores.IndexOf(min);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }
    
    private int evalState(BoardBot board){ // return [Xscore,Oscore]
        int Xscore = evalplayer(board,0);
        int Oscore = - evalplayer(board,1);
        return Xscore + Oscore;
    }
  
    private int evalplayer(BoardBot board, int agent){ // return Score

        int score = 0;
        for (int i=0; i<9; i++)
        {
            SquareBot sq = board.squares[i];
            if (sq.classicallyMarked && sq.finalPlayer != agent)
            {
                continue;
            }

            List<int> neighbors = getNeighbor(i);
            foreach (int k in neighbors)
            {
                int gain = 0;
                SquareBot neigh = board.squares[k];
                if (neigh.classicallyMarked && neigh.finalPlayer == agent)
                {
                    gain += 20;
                } else if (neigh.filledMarks == 0)
                {
                    gain += 5;
                } else
                {
                    foreach (MarkBot m in neigh.presentMarks) {
                        if (m.player == agent) {
                            gain += 1;
                        }
                    }
                }
                if (sq.classicallyMarked) {
                    gain *= 2;
                }
                score += gain;
            }
        }
        return score;
    }
    

    public double evalMove(Action move, BoardBot board,int agent,int difficulty) {
        double score;
        if(difficulty == 0){
            BoardBot copy = board.shallowCopy();
            move.performAction(copy);
            score = evalState(copy);
            return score;
        }
        if(agent == 1){
            BoardBot copy = board.shallowCopy();
            move.performAction(copy);
            Action act = getNextMoveopponent(copy, move.actionType,difficulty-1);
            act.performAction(copy);
            return evalState(copy);

        }else if(agent == 0){
            BoardBot copy = board.shallowCopy();
            move.performAction(copy);
            Action act = getNextMove(copy,move.actionType,difficulty-1);
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
            result.Add(new Action(board.squares[pos1]));
            result.Add(new Action(board.squares[pos2]));
            return result;
        }

        // Otherwise we return all the possible spookyMarks to add
        // First find all possible squares we can put marks on.
        List<int> validSquares = new List<int>();
        for (int i=0; i<9; i++)
        {
            SquareBot sq = board.squares[i];
            if (!sq.classicallyMarked && sq.filledMarks < 8)
            {
                validSquares.Add(i);
            }
        }
        // If no valid squares, return null
        if (validSquares.Count == 0) {
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
            int t = turnNum;
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
        int c = sq - 3*r;

        List<int> neighbors = new List<int>();
        for (int i=r-1; i<=r+1; i++)
        {
            for (int j=c-1; j<=c+1; j++)
            {
                int index = 3*i+j;
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
