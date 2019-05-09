using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBot

{
    private Dictionary<SquareBot, HashSet<SquareBot>> adjlistSQ;

    public GraphBot(Dictionary<SquareBot, HashSet<SquareBot>> adjlistSQ)
    {
        this.adjlistSQ = adjlistSQ;
    }

    public void init(Dictionary<SquareBot, HashSet<SquareBot>> adjlistSQ)
    {
        this.adjlistSQ = adjlistSQ;
    }

    public GraphBot(Graph g, GameManagerBot gameManager)
    {
        Dictionary<SquareBot, HashSet<SquareBot>> adjlistSQ = new Dictionary<SquareBot, HashSet<SquareBot>>();
        Dictionary<Square, HashSet<Square>> gList = g.adjlistSQ;
        foreach (KeyValuePair<Square, HashSet<Square>> kvp in gList)
        {
            SquareBot sb = new SquareBot(gameManager, kvp.Key);
            HashSet<SquareBot> h = new HashSet<SquareBot>();
            foreach (Square sq in kvp.Value)
            {
                h.Add(new SquareBot(gameManager, sq));
            }
            adjlistSQ.Add(sb, h);
        }
        this.init(adjlistSQ);
    }

    public GraphBot(GraphBot g, GameManagerBot gameManager)
    {
        Dictionary<SquareBot, HashSet<SquareBot>> adjlistSQ = new Dictionary<SquareBot, HashSet<SquareBot>>();
        Dictionary<SquareBot, HashSet<SquareBot>> gList = g.adjlistSQ;
        foreach (KeyValuePair<SquareBot, HashSet<SquareBot>> kvp in gList)
        {
            SquareBot sb = new SquareBot(gameManager, kvp.Key);
            HashSet<SquareBot> h = new HashSet<SquareBot>();
            foreach (SquareBot sq in kvp.Value)
            {
                h.Add(new SquareBot(gameManager, sq));
            }
            adjlistSQ.Add(sb, h);
        }
        this.init(adjlistSQ);
    }

    // Returns true if there is a cycle
    public bool addEdgeSQ(SquareBot u, SquareBot v)
    {
        HashSet<SquareBot> m = null;
        if (!adjlistSQ.TryGetValue(u, out m))
        {
            adjlistSQ[u] = new HashSet<SquareBot>();
        }
        if (!adjlistSQ.TryGetValue(v, out m))
        {
            adjlistSQ[v] = new HashSet<SquareBot>();
        }

        // Two Square cycle case
        if (adjlistSQ[u].Contains(v) && adjlistSQ[v].Contains(u))
        {
            return true;
        }
        adjlistSQ[u].Add(v);
        adjlistSQ[v].Add(u);
        return false;
    }

    public void printSetSQ(HashSet<SquareBot> visited)
    {
        foreach (SquareBot sq in visited)
        {
            Debug.Log(sq.ToString());
        }
    }

    public HashSet<SquareBot> getCycleSQ(SquareBot i)
    {
        HashSet<SquareBot> visited = new HashSet<SquareBot>();
        if (dfsSQ(i, visited, null))
        {
            HashSet<SquareBot> newVisited = new HashSet<SquareBot>(visited);
            foreach (SquareBot j in visited)
            {
                dfsComplete(j, newVisited, null);
            }
            return newVisited;
        }
        return null;
    }

    public bool dfsSQ(SquareBot i, HashSet<SquareBot> visited, SquareBot parent)
    {
        visited.Add(i);
        foreach (SquareBot j in adjlistSQ[i])
        {
            if (visited.Contains(j))
            {
                if (!j.Equals(parent))
                {
                    return true;
                }
                continue;
            }
            else if (dfsSQ(j, visited, i))
            {
                return true;
            }
        }
        return false;
    }

    public bool dfsComplete(SquareBot i, HashSet<SquareBot> visited, SquareBot parent)
    {
        visited.Add(i);
        foreach (SquareBot j in adjlistSQ[i])
        {
            if (visited.Contains(j))
            {
                continue;
            }
            else if (dfsSQ(j, visited, i))
            {
                return true;
            }
        }
        return false;
    }

    public bool removeCycleSQ(SquareBot v)
    {
        if (getCycleSQ(v) != null)
        {
            foreach (SquareBot j in getCycleSQ(v))
            {
                deleteEdgeSQ(v, j);
            }
            return true;
        }
        return false;
    }

    public void deleteEdgeSQ(SquareBot u, SquareBot v)
    {
        if (adjlistSQ[u].Contains(v))
        {
            adjlistSQ[u].Remove(v);
        }

        if (adjlistSQ[v].Contains(u))
        {
            adjlistSQ[v].Remove(u);
        }
    }
}
