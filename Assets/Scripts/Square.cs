using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    private Transform innerSquares;
    private GameManager gameManager;
    public GameObject X;
    public GameObject O;
    private int filledMarks;
    private int chosenMarks;
    public List<Tuple<int, int>> presentMarks;

    // Start is called before the first frame update
    void Start()
    {
        innerSquares = transform.Find("InnerSquares");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        presentMarks = new List<Tuple<int, int>>();
    }

    public void addMark()
    {
        int player = gameManager.getCurrentPlayer();
        int turn = gameManager.getTurnNum();
        GameObject toAdd;
        if (player == 1)
        {
            toAdd = X;
        } else if (player == 2) {
            toAdd = O;
        }
        foreach (Transform square in innerSquares)
        {

        }
        Tuple<int, int> mark = new Tuple<int, int>(player, turn);
        presentMarks.add(mark);
    }

    public void test()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
