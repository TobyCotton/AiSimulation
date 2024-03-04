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

    private Agent PlannerAgent;

    void Start()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<ProceduralInput>();
        grid = Terrain.grid;
        componentTransform = GetComponent<Transform>();
        PlannerAgent = GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (grid.path.Count > 0)
        {
            componentTransform.position = Vector3.MoveTowards(componentTransform.position, grid.path[0].worldPos, speed * Time.deltaTime);
            if (Vector3.Distance(componentTransform.position, grid.path[0].worldPos) < 0.000001f)
            {
                grid.path.RemoveAt(0);
            }
        }
        else
        {
            PlannerAgent.NotifyReachedGoal();
        }

        //if (Vector3.Distance(componentTransform.position, TargetPosition) > 0.5)
        //{
        //    componentTransform.position = Vector3.MoveTowards(componentTransform.position, TargetPosition, speed * Time.deltaTime);
        //}
        //else
        //{
        //    PlannerAgent.NotifyReachedGoal();
        //}
    }

    public void moveTo(string TargetTag)
    {
        AStarPathing(componentTransform.position, new Vector3(100, 1, 100));
    }
    
    void moveAlongPath(Vector3 endPositon)
    {
        
        //Vector2 GridDistance = WorldPosToSquarePos(endPositon);
        //float SquaresLength = GridDistance.x / Terrain.length;
        //float SquaresWidth = GridDistance.y / Terrain.width;
        //Vector3 MovementVector = new Vector3(GridDistance.x, 0, GridDistance.y);
        //print("MovementVector :" + MovementVector.ToString());
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

        GridTile startTile = grid.gridArray[Convert.ToInt32(gridStartPos.x), Convert.ToInt32(gridStartPos.y)];
        GridTile targetTile = grid.gridArray[Convert.ToInt32(gridEndPos.x), Convert.ToInt32(gridEndPos.y)];

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].f < tile.f || openSet[i].f == tile.f)
                {
                    if (openSet[i].h < tile.h)
                        tile = openSet[i];
                }
            }

            openSet.Remove(tile);
            closedSet.Add(tile);

            if (tile == targetTile)
            {
                retracePath(startTile, targetTile);
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                    continue;

                int CostToNeighbour = tile.g + GetDistance(tile, neighbour);

                if (CostToNeighbour < neighbour.g || !openSet.Contains(neighbour))
                {
                    neighbour.g = CostToNeighbour;
                    neighbour.h = GetDistance(neighbour, targetTile);
                    neighbour.previousTile = tile;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
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

        grid.path = newPath;
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
