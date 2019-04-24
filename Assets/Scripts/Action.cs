using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    // 0 means placing new spookyMarks, 1 means choosing square to collapse
    int actionType;
    SpookyMark spMark;
    Square sq;

    public Action(SpookyMark markToAdd)
    {
        actionType = 0;
        spMark = markToAdd;
        sq = null;
    }

    public Action(Square toCollapse)
    {
        actionType = 1;
        sq = toCollapse;
        spMark = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
