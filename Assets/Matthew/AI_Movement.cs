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
        //path = new List<GridTile>();
        if (grid == null )
        {
            Debug.Log("GRID EMPTY");
        }
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
        AStarPathing(componentTransform.position, new Vector3(99, 1, 99));
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
        Debug.Log("ASTAR PATHING TRIGGERED");
        List<GridTile> openSet = new List<GridTile>();
        List<GridTile> closedSet = new List<GridTile>();
        Vector2 gridStartPos = WorldPosToSquarePos(startPos);
        Vector2 gridEndPos = WorldPosToSquarePos(endPos);

        Debug.Log("start X Pos: " + gridStartPos.x + " start Y Pos: " + gridStartPos.y);
        Debug.Log("end X Pos: " + gridEndPos.x + " end Y Pos: " + gridEndPos.y);

        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);
        if (startTile.gridPos == targetTile.gridPos) 
        {
            Debug.Log(" start tile X Pos: " + startTile.gridPos.x + " start Tile Y Pos: " + startTile.gridPos.y);
            Debug.Log("target tile X Pos: " + targetTile.gridPos.x + " target Tile Y Pos: " + targetTile.gridPos.y);
            Debug.Log("THE SAME");
        }

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

            Debug.Log("BEFORE NEIGHBOURS CHECK");
            foreach (GridTile neighbour in Terrain.grid.GetNeighbours(tile))
            {
                Debug.Log("CHECKING NEIGHBOURS");
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    if (!neighbour.isWalkable)
                    {
                        Debug.Log("NOT WALKABLE");
                    }
                    if (closedSet.Contains(neighbour))
                    {
                        Debug.Log("IN CLOSED SET");
                    }
                    Debug.Log("HERE 3");
                    continue;
                }

                Debug.Log("HERE 1");
                int CostToNeighbour = tile.g + GetDistance(tile, neighbour);

                if (CostToNeighbour < neighbour.g || !openSet.Contains(neighbour))
                {
                    Debug.Log("HERE 2");
                    neighbour.g = CostToNeighbour;
                    neighbour.h = GetDistance(neighbour, targetTile);
                    neighbour.previousTile = tile;

                    if (!openSet.Contains(neighbour))
                    {
                        Debug.Log("ADDING NEIGHBOURS");
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void retracePath(GridTile start, GridTile end)
    {
        Debug.Log("RETRACING PATH");
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
