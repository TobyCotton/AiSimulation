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
    public bool availablePost;
    public float weight;

    public Vector3 worldPos;

    public Vector2 gridPos;

    public float g;
    public float h;
    public float f { get { return g + h; } }

    public GridTile previousTile;

    public SpriteRenderer renderer;
    private int heapIndex;

    public GameObject Tile;

    public GridTile(Vector3 worldPos, Vector2 gridPos)
    {
        isGrass = false;
        isWalkable = true;
        this.gridPos = gridPos;
        this.worldPos = worldPos;
    }

    public void SetIsGrass()
    {
        //sets the tile to be grass and sets the weight for the tile
        isGrass = true;
        weight = 1.6f;
    }

    public void SetIsPath()
    {
        //sets the tile to not be grass and sets the weight for the tile 
        isGrass = false;
        weight = 0;
    }

    public int HeapIndex
    {
        //the tiles position within the heap
        get { return heapIndex;} 
        set { heapIndex = value; }
    }

    void OnMouseEnter()
    {
        Debug.Log(Tile.name);
    }
    
}
