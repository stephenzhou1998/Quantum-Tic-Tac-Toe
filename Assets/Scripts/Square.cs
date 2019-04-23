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

    // Start is called before the first frame update
    void Start()
    {
        innerSquares = transform.Find("InnerSquares");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void addMark()
    {
        GameObject toAdd;
        if (gameManager.getCurrentPlayer() == 1)
        {
            toAdd = X;
        } else if (gameManager.getCurrentPlayer() == 2) {
            toAdd = O;
        }
        foreach (Transform square in innerSquares)
        {

        }
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
