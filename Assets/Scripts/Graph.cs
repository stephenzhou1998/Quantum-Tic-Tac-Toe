using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour

{
    private Dictionary<Mark,HashSet<Mark>> adjlist;
    int numVertices;
    int numEdges;

    public Graph()
    {
        this.adjlist = new Dictionary<Mark, HashSet<Mark>>();
    }

    public Graph(int numEdges, int numVertices){
        this.numEdges = numEdges;
        this.numVertices = numVertices;
        this.adjlist = new Dictionary<Mark, HashSet<Mark>>();
    }

    public void addEdge(Mark u, Mark v){
        if(adjlist[u] == null){
            adjlist[u] = new HashSet<Mark>();
        }
        adjlist[u].Add(v);
        if(adjlist[v] == null){
            adjlist[v] = new HashSet<Mark>();
        }
        adjlist[v].Add(u);

    }
    
    public HashSet<Mark> getCycle(Mark i){
        HashSet<Mark> visited = new HashSet<Mark>();
        if(dfs(i,visited) == true){
            foreach (Mark j in visited){
                dfs(j,visited);
            }
            return visited;
        }
        return null;
    }
    public bool dfs(Mark i, HashSet<Mark> visited){
        if(visited.Contains(i)){
            return true;
        }else{
            visited.Add(i);
            foreach (Mark j in adjlist[i]){
                dfs(j,visited);
            }
            return false;
        }
        
    }

    public bool removeCycle(Mark v){
       
        if(getCycle(v) != null){
            foreach (Mark j in getCycle(v)){
                deleteEdge(v,j);
            }
            return true;
        }
        return false;

    }
    public void deleteEdge(Mark u, Mark v){
        if(adjlist[u].Contains(v)){
            adjlist[u].Remove(v);
        }
        
        if(adjlist[v].Contains(u)){
            adjlist[v].Remove(u);
        }
        

    }

}
