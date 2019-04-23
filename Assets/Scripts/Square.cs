using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
	public List<Tuple<int, int>>presentMarks;
    // Start is called before the first frame update
    void Start()
    {
        presentMarks = new List<Tuple<int, int>>()
    }

    bool addMark(int player, int turn)
    {
    	Tuple<int, int> mark = new Tuple<int, int>(player, turn);
    	presentMarks.add(mark);
    	return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
