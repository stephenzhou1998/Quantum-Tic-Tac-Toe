using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour

{
    private HashMap<Mark,LinkedList<Mark>> adjlist;
    int numVertices;
    int numEdegs;
    public Grahph(int numEdegs, int numVertices){
        this.numEdegs = numEdegs;
        this.numVertices = numVertices;
        this.adjlist = new HashMap<>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void addEdge(Mark u, Mark v){
        if(adjlist.get(u) == null){
            adjlist.get(u) = new LinkedList<Mark>();
        }
        adjlist.get(u).add(v);
        if(adjlist.get(v) == null){
            adjlist.get(v) = new LinkedList<Mark>();
        }
        adjlist.get(v).add(u);

    }
    
    public HashSet<Mark> getCycle(){
        for(Mark i:adjlist.keys()){
            HashSet<Mark> visited = new HashSet<>();
            if(dfs(i,visited) == true){
                for(Mark j: visited){
                    dfs(j,visited);
                }
                return visited;
            }
        }
        return null;
    }
    public boolean dfs(Mark i, HashSet visited){
        if(visited.contains(i)){
            return true;
        }else{
            visited.add(i);
            for(Mark j : adjlist.get(i)){
                dfs(j,visited);
            }
            return false;
        }
        
    }

    public boolean removeCycle(Mark v){
       
        if(getCycle(v) != null){
            for(Mark j: getCycle){
                deleteEdge(v,j);
            }
            return true;
        }
        return false;

    }
    public void deleteEdge(Mark u, Mark v){
        if(adjlist.get(u).contains(v)){
            adjlist.get(u).remove(v);
        }
        
        if(adjlist.get(v) l){
            adjlist.get(v).remove(u);
        }
        

    }

}
