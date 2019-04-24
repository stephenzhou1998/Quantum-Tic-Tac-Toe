using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public Action getNextMove(Board board,int actionType,int difficulty)
    {   
        List<Action> legalmoves = getallmoves(board,1); // 1 means bot
        List<int> Scores = new List<int>();

        
        for(Action i:legalmoves){
            if(i.actionType == actionType){
                Board copy = Board.copy(board);
                Scores.add(evalMove(i,copy,1,difficulty));
            }
        }
        int max = max(Scores);
        int index = Scores.indexof(max);
        return legalmoves[index];


        
        // Perform tree search for best strategy
    }

    public Action getNextMoveopponent(Board board,int actionType,int difficulty)
    {   
        List<Action> legalmoves = getallmoves(board,0); // 0 means opponent
        List<int> Scores = new List<int>();

        
        for(Action i:legalmoves){
            if(i.actionType == actionType){
                Board copy = Board.copy(board);
                Scores.add(evalMove(i,copy,0,difficulty));
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
            getNextMoveopponent(Board board,int actionType,int difficulty)

        }

    }

    public List<Action> getallmoves(Board board,int agent){



    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
