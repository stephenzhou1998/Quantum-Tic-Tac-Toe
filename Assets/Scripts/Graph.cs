﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour

{
    private Dictionary<SpookyMark, HashSet<SpookyMark>> adjlist;
    private Dictionary<Square, HashSet<Square>> adjlistSQ;
    int numVertices;
    int numEdges;

    public Graph()
    {
        this.adjlist = new Dictionary<SpookyMark, HashSet<SpookyMark>>();
        this.adjlistSQ = new Dictionary<Square, HashSet<Square>>();
    }

    public Graph(int numEdges, int numVertices){
        this.numEdges = numEdges;
        this.numVertices = numVertices;
        this.adjlist = new Dictionary<SpookyMark, HashSet<SpookyMark>>();
        this.adjlistSQ = new Dictionary<Square, HashSet<Square>>();
    }

    public void addVertex(SpookyMark v)
    {
        adjlist[v] = new HashSet<SpookyMark>();
    }
    
    public void addEdge(SpookyMark u, SpookyMark v){
        //HashSet<Mark> m = null;
        //if(!adjlist.TryGetValue(u, out m)){
        //    adjlist[u] = new HashSet<Mark>();
        //}
        //Debug.Log("Making edge between " + u.ToString() + " and " + v.ToString());
        adjlist[u].Add(v);
        //if(!adjlist.TryGetValue(v, out m)){
        //    adjlist[v] = new HashSet<Mark>();
        //}
        adjlist[v].Add(u);
        //Debug.Log("adjacency list of " + u.ToString() + " contains:");
        //printSet(adjlist[u]);
    }

    // Returns true if there is a cycle
    public bool addEdgeSQ(Square u, Square v)
    {
        HashSet<Square> m = null;
        if(!adjlistSQ.TryGetValue(u, out m)){
            adjlistSQ[u] = new HashSet<Square>();
        }
        if (!adjlistSQ.TryGetValue(v, out m))
        {
            adjlistSQ[v] = new HashSet<Square>();
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

    public void printSet(HashSet<SpookyMark> visited)
    {
        foreach (SpookyMark sm in visited)
        {
            Debug.Log(sm.ToString());
        }
    }

    public void printSetSQ(HashSet<Square> visited)
    {
        foreach (Square sq in visited)
        {
            Debug.Log(sq.ToString());
        }
    }

    public HashSet<SpookyMark> getCycle(SpookyMark i){
        Debug.Log("detecting if there's a cycle starting from SpookyMark: " + i.ToString());
        HashSet<SpookyMark> visited = new HashSet<SpookyMark>();
        if(dfs(i,visited, null)) {
            //foreach (SpookyMark j in visited){
            //    dfs(j,visited, null);
            //}
            return visited;
        }
        return null;
    }

    public HashSet<Square> getCycleSQ(Square i)
    {
        Debug.Log("detecting if there's a cycle starting from " + i.ToString());
        HashSet<Square> visited = new HashSet<Square>();
        if (dfsSQ(i, visited, null))
        {
            HashSet<Square> newVisited = new HashSet<Square>(visited);
            foreach (Square j in visited){
                dfsComplete(j, newVisited, null);
            }
            return newVisited;
        }
        return null;
    }

    public bool dfs(SpookyMark i, HashSet<SpookyMark> visited, SpookyMark parent)
    {
        //Debug.Log("Processing " + i.ToString() + ", visited contains: ");
        //printSet(visited);
        //if (visited.Contains(i)){
        //    return true;
        //} else {}
        visited.Add(i);
        //Debug.Log("Adjacency list of " + i.ToString() + ": ");
        //printSet(adjlist[i]);
        foreach (SpookyMark j in adjlist[i]) {
            if (visited.Contains(j))
            {
                if (!j.Equals(parent))
                {
                    return true;
                }
                continue;
            } else if (dfs(j, visited, i))
            {
                return true;
            }
        }
        return false;
    }

    public bool dfsSQ(Square i, HashSet<Square> visited, Square parent)
    {
        //Debug.Log("Processing " + i.ToString() + ", visited contains: ");
        //printSetSQ(visited);
        //if (parent != null)
        //{
        //    Debug.Log("Parent is " + parent.ToString());
        //} else
        //{
        //    Debug.Log("No parent");
        //}
        visited.Add(i);
        //Debug.Log("Adjacency list of " + i.ToString() + ": ");
        //printSetSQ(adjlistSQ[i]);
        foreach (Square j in adjlistSQ[i])
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

    public bool dfsComplete(Square i, HashSet<Square> visited, Square parent)
    {
        visited.Add(i);
        foreach (Square j in adjlistSQ[i])
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

    public bool removeCycle(SpookyMark v){
       
        if(getCycle(v) != null){
            foreach (SpookyMark j in getCycle(v)){
                deleteEdge(v,j);
            }
            return true;
        }
        return false;

    }

    public bool removeCycleSQ(Square v)
    {
        if (getCycleSQ(v) != null)
        {
            foreach (Square j in getCycleSQ(v))
            {
                deleteEdgeSQ(v, j);
            }
            return true;
        }
        return false;
    }

    public void deleteEdge(SpookyMark u, SpookyMark v){
        if(adjlist[u].Contains(v)){
            adjlist[u].Remove(v);
        }
        
        if(adjlist[v].Contains(u)){
            adjlist[v].Remove(u);
        }
    }

    public void deleteEdgeSQ(Square u, Square v)
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
