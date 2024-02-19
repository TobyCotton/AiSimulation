using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    
    public bool isWalkable;

    public Vector3 worldPos;

    public Vector2 gridPos;

    public int g;
    public int h;
    public int f;

    public GridTile previousTile;
}
