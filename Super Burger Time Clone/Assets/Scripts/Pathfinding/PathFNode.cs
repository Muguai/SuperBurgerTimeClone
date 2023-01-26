using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFNode : MonoBehaviour
{
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public PathFNode parent;

    public PathFNode(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public override string ToString()
    {
        return "X: " + gridX + " Y: " + gridY;
    }

    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        PathFNode p = obj as PathFNode;
        if ((System.Object)p == null)
        {
            return false;
        }

     //   Debug.Log("CompareX: " + worldPosition.x == p.worldPosition.x + " CompareY: " + (worldPosition.y == p.worldPosition.y));
        // Return true if the fields match:
        return (gridX == p.gridX) && (gridY == p.gridY);
    }
    /*
    public bool Equals(PathFNode p)
    {
        // If parameter is null return false:
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (worldPosition.x == p.worldPosition.x) && (worldPosition.y == p.worldPosition.y);
    }
    */
    /*
    public override int GetHashCode()
    {
        return gridX ^ gridY;
    }
    */
    
}
