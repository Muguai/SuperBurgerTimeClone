using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFGrid : MonoBehaviour
{

    public Transform player;
    public LayerMask layerMask;
    public Vector2 gridWorldSize;
    public float spaceBetweenNodes;
    public float nodeRadius;
    PathFNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public PathFNode NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = ((worldPosition.x - 10.75f) + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x,y];
    }

    public List<PathFNode> GetNeighbours(PathFNode node)
    {
        List<PathFNode> neighbours = new List<PathFNode>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX,checkY]);
                }
            }
        }

        return neighbours;
    }

    void CreateGrid()
    {
        grid = new PathFNode[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y/2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, layerMask));
                grid[x, y] = new PathFNode(walkable, worldPoint, x, y);
            }
        }
    }


    public List<PathFNode> path;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));

        if(grid != null)
        {
            PathFNode playerNode = NodeFromWorldPoint((Vector2)player.position);
            foreach (PathFNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                if (path != null)
                {

                    if (path.Contains(n))
                    {
                        //Debug.Log("Path");

                        Gizmos.color = Color.green;
                    }
                }
                if(playerNode.worldPosition == n.worldPosition)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - spaceBetweenNodes));
            }

        }
    }
}
