using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        List<int> Scores = new List<int>();

        
        foreach (Action i in legalmoves){
            if(i.actionType == actionType){
                BoardBot copy = board.shallowCopy();
                Scores.Add(evalMove(i,copy,1,difficulty));
            }
        }
        int max = max(Scores);
        int index = Scores.indexof(max);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }

    public Action getNextMoveopponent(BoardBot board,int actionType,int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 0, currentTurn); // 0 means opponent
        List<int> Scores = new List<int>();

        
        foreach (Action i in legalmoves){
            if(i.actionType == actionType)
            {
                BoardBot copy = board.shallowCopy();
                Scores.Add(evalMove(i,copy,0,difficulty));
            }
        }
        int min = min(Scores);
        int index = Scores.indexof(min);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }
    
    private int evalState(Board board){ // return [Xscore,Oscore]
        int Xscore = evalplayer(Board,0);
        int Oscore = - evalplayer(Board,1);
        return Xscore + Oscore;
    }

    private int evalplayer(Board board, int agent){ // return Score
        int score;
        
        return score;
    }
    

    public int evalMove(Action move, BoardBot board,int agent,int difficulty) {
        double score;
        if(difficulty == 0){
            Board copy = Board.copy(board);
            copy.makemove(move);
            score = evalState(copy);
            return score;
        }
        if(agent == 1){
            Board copy = Board.copy(board);
            copy.makemove(move);
            Action act = getNextMoveopponent(copy,actionType,0,difficulty-1);
            copy.makemove(act);
            return evalState(copy);

        }else if(agent == 0){
            Board copy = Board.copy(board);
            copy.makemove(move);
            Action act = getNextMove(copy,actionType,1,difficulty-1);
            copy.makemove(act);
            return evalState(copy);
        }


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
        List<int> validSquares = new LinkedList<int>();
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

        List<int> neighbors = new LinkedList<int>();
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
