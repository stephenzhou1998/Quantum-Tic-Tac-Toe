using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bot : MonoBehaviour
{
    public int difficulty;
    private Transform hidden;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        hidden = GameObject.Find("Hidden").transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }


    public Action getNextMove(Board board, int actionType, int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 1); // 1 means bot
        List<int> Scores = new List<int>();
        
        foreach (Action i in legalmoves) {
            if(i.actionType == actionType){
                Board copy = Instantiate(board, hidden);
                Scores.Add(evalMove(i,copy,1,difficulty));
            }
        }
        int max = Scores.Max();
        int index = Scores.IndexOf(max);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }

    public Action getNextMoveopponent(Board board,int actionType,int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 0); // 0 means opponent
        List<int> Scores = new List<int>();

        
        foreach (Action i in legalmoves){
            if(i.actionType == actionType)
            {
                Board copy = Instantiate(board, hidden);
                Scores.Add(evalMove(i,copy,0,difficulty));
            }
        }
        int min = Scores.Min();
        int index = Scores.IndexOf(min);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }

    private void evalState(Board board)
    {
        // Create leaf node of the search tree
    }
    

    public int evalMove(Action move,Board board,int agent,int difficulty){
        double score;
        if(difficulty == 0){
            return 0;
        }
        if(agent == 1){
            getNextMoveopponent(board, move.actionType, difficulty);
            
        }
        return 0;
    }

    private List<Action> getLegalActions(Board board, int actionType, int agent)
    {
        // Generate all possible actions
        List<Action> result = new List<Action>();
        int turnNum = gameManager.getTurnNum();

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
        List<Square> validSquares = new List<Square>();
        for (int i=0; i<9; i++)
        {
            Square sq = board.squares[i];
            if (!sq.classicallyMarked && sq.filledMarks < 8)
            {
                validSquares.Add(sq);
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
            Mark m1 = new Mark(p,t,validSquares[c[0]].position, validSquares[c[0]]);
            Mark m2 = new Mark(p,t,validSquares[c[1]].position, validSquares[c[1]]);
            SpookyMark sm = new SpookyMark(m1, m2);
            result.Add(new Action(sm));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
