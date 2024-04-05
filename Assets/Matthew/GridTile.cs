using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GridTile : IHeapItem<GridTile>
{
     
    public bool isWalkable;
    public bool isGrass;
    public bool isEntrance;
    public int weight;

    public Vector3 worldPos;

    public Vector2 gridPos;

    public int g;
    public int h;
    public int f { get { return g + h; } }

    public GridTile previousTile;

    public SpriteRenderer renderer;
    private int heapIndex;

    public GridTile(Vector3 worldPos, Vector2 gridPos)
    {
        isGrass = false;
        isWalkable = true;
        this.gridPos = gridPos;
        this.worldPos = worldPos;
    }

    public void SetIsGrass()
    {
        isGrass = true;
        weight = 16;
    }

    public int HeapIndex
    {
        get { return heapIndex;} 
        set { heapIndex = value; }
    }
}
