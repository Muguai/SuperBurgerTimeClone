using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingAStar : MonoBehaviour
{
    PathFGrid grid;
    public Transform seeker, target;

    void Awake()
    {
        grid = GetComponent<PathFGrid>();

    }

    void Start()
    {

        PathFNode playerNode = grid.NodeFromWorldPoint(seeker.position);
        Debug.Log(playerNode.ToString());
        playerNode = grid.NodeFromWorldPoint(target.position);
        Debug.Log(playerNode.ToString());

        FindPath(seeker.position, target.position);

    }

    void Update()
    {
    }

    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        int rounds = 0;
        int roundsInside = 0;
        PathFNode startNode = grid.NodeFromWorldPoint(startPos);
        PathFNode targetNode = grid.NodeFromWorldPoint(targetPos);
        if(startNode.Equals(targetNode))
        {
            Debug.Log("the same targetNode: " + targetNode.worldPosition + " startNode: " + startNode.worldPosition);
        }

        List<PathFNode> openSet = new List<PathFNode>();
        HashSet<PathFNode> closedSet = new HashSet<PathFNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Debug.Log("Rounds: " + rounds);

            PathFNode currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode.Equals(targetNode))
            {
                Debug.Log("Done ");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(PathFNode neighbour in grid.GetNeighbours(currentNode))
            {
               // Debug.Log("RoundsInside: " + roundsInside);
                if(rounds > 17)
                {
                    Debug.Log(currentNode.ToString());

                    foreach (PathFNode n in grid.GetNeighbours(currentNode)) 
                    {
                        Debug.Log(n.ToString());
                    }
                }
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    roundsInside++;

                  //  continue;
                }
                else
                {

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }

                roundsInside++;
            }
            rounds++;
            if(rounds > 200)
            {
                Debug.Log("Failed");
                break;
            }
            if(openSet.Count <= 0)
            {

                Debug.Log("Count 0");
            }
        }
    }

    void RetracePath(PathFNode startNode, PathFNode endNode)
    {
        List<PathFNode> path = new List<PathFNode>();
        PathFNode currentNode = endNode;

        while(currentNode.Equals(startNode) == false)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        Debug.Log("Found path " + path.Count);
        grid.path = path;
    }

    int GetDistance(PathFNode nodeA, PathFNode nodeB)
    {
        
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
        
       // return Mathf.RoundToInt(Mathf.Abs(nodeA.worldPosition.x - nodeB.worldPosition.x) + Mathf.Abs(nodeA.worldPosition.y - nodeB.worldPosition.y)); 

    }
}
