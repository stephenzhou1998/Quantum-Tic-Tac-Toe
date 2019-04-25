using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public int difficulty;
<<<<<<< HEAD
=======
    private Transform hidden;
    private GameManager gameManager;
>>>>>>> 08f2c1d76aa5817dacfb5207c4b3372170cf3ef2

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        
=======
        hidden = GameObject.Find("Hidden").transform;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
>>>>>>> 08f2c1d76aa5817dacfb5207c4b3372170cf3ef2
    }


    public Action getNextMove(Board board, int actionType, int difficulty)
    {   
        List<Action> legalmoves = getLegalActions(board, actionType, 1); // 1 means bot
        List<int> Scores = new List<int>();

        
        for(Action i:legalmoves){
            if(i.actionType == actionType){
<<<<<<< HEAD
                Board copy = Board.copy(board);
                Scores.add(evalMove(i,copy,1,difficulty));
=======
                Board copy = Instantiate(board, hidden);
                Scores.Add(evalMove(i,copy,1,difficulty));
>>>>>>> 08f2c1d76aa5817dacfb5207c4b3372170cf3ef2
            }
        }
        int max = max(Scores);
        int index = Scores.indexof(max);
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
        int min = min(Scores);
        int index = Scores.indexof(min);
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
            return ;
        }
        if(agent == 1){
            //Board copy = Board.copy(board);
            //getNextMoveopponent(copy,actionType,difficulty-1);
            getNextMoveopponent(Board board,int actionType,int difficulty)

        }

    }

    private List<Action> getLegalActions(Board board, int actionType, int agent)
    {
        // Generate all possible actions
<<<<<<< HEAD
        List<Action> result = new LinkedList<Action>();
=======
        List<Action> result = new List<Action>();
        int turnNum = gameManager.getTurnNum();
>>>>>>> 08f2c1d76aa5817dacfb5207c4b3372170cf3ef2

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
        List<Integer> validSquares = new LinkedList<Integer>();
        for (int i=0; i<9; i++)
        {
            Square sq = board.squares[i];
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
