using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using TMPro;
using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;

public class AI_Movement : MonoBehaviour
{
    private Grid grid;
    private Transform componentTransform;
    private ProceduralInput Terrain;
    private int speed = 10;
    private List<GridTile> path;

    private Agent PlannerAgent;

    void Start()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<ProceduralInput>();
        componentTransform = GetComponent<Transform>();
        PlannerAgent = GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (path.Count > 0)
        {
            componentTransform.position = Vector3.MoveTowards(componentTransform.position, path[0].worldPos, speed * Time.deltaTime);
            if (Vector3.Distance(componentTransform.position, path[0].worldPos) < 0.000001f)
            {
                path.RemoveAt(0);
            }
            if (path.Count == 0 ) 
            {
                PlannerAgent.NotifyReachedGoal();
            }
        }
    }

    public void moveTo(string TargetTag)
    {
        AStarPathing(componentTransform.position, new Vector3(99, 1, 99));
    }
    
    void moveAlongPath(Vector3 endPositon)
    {
        componentTransform.position = Vector2.MoveTowards(componentTransform.position, endPositon, Time.deltaTime) * Time.deltaTime;
    }

    Vector2 WorldPosToSquarePos(Vector3 worldPos)
    {
        return new Vector2(Convert.ToInt32(MathF.Floor(worldPos.x)), Convert.ToInt32(MathF.Floor(worldPos.z)));
    }

    Vector3 SquarePosToWorldPos(Vector2 squarePos)
    {
        return new Vector3(squarePos.x + 0.5f, 1, squarePos.y + 0.5f);
    }

    void AStarPathing(Vector3 startPos, Vector3 endPos)
    {
        List<GridTile> openSet = new List<GridTile>();
        List<GridTile> closedSet = new List<GridTile>();
        Vector2 gridStartPos = WorldPosToSquarePos(startPos);
        Vector2 gridEndPos = WorldPosToSquarePos(endPos);

        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.OrderBy(x => x.f).First();

            openSet.Remove(tile);
            closedSet.Add(tile);

            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                return;
            }

            foreach (GridTile neighbour in Terrain.grid.GetNeighbours(tile))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int CostToNeighbour = tile.g + GetDistance(tile, neighbour);

                if (CostToNeighbour < neighbour.g || !openSet.Contains(neighbour))
                {
                    neighbour.g = CostToNeighbour;
                    neighbour.h = GetDistance(neighbour, targetTile);
                    neighbour.previousTile = tile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void retracePath(GridTile start, GridTile end)
    {
        List<GridTile> newPath = new List<GridTile>();

        GridTile tile = end;

        while (tile != start) {
            newPath.Add(tile);
            tile = tile.previousTile;
        }

        newPath.Reverse();

        path = newPath;
    }

    int GetDistance(GridTile tileA, GridTile tileB)
    {
        int dstX = Mathf.Abs(Convert.ToInt32(tileA.gridPos.x - tileB.gridPos.x));
        int dstY = Mathf.Abs(Convert.ToInt32(tileA.gridPos.y - tileB.gridPos.y));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
