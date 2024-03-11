using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class GridTile
{
    
    public bool isWalkable;
    public bool isGrass;

    public Vector3 worldPos;

    public Vector2 gridPos;

    public int g;
    public int h;
    public int f { get { return g + h; } }

    public GridTile previousTile;

    public GridTile(Vector3 worldPos, Vector2 gridPos)
    {
        isGrass = false;
        isWalkable = true;
        this.gridPos = gridPos;
        this.worldPos = worldPos;
    }
}
