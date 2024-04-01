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
    public enum PathingType
    {
        AStar,
        Dijkstra
    }

    public PathingType aiPathingType;
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
        GameObject TargetBuilding = GetClosestObjectWithTag(TargetTag);
        Vector3 TargetPosition = TargetBuilding.transform.Find("Entrance").position;
        AStarPathing(componentTransform.position, TargetPosition);
    }
    void AStarPathing(Vector3 startPos, Vector3 endPos)
    {
        List<GridTile> openSet = new List<GridTile>();
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = GetNextTile(openSet);
            

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

                int CostToNeighbour = tile.g + GetDistance(tile, neighbour) + tile.weight;

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

    GridTile GetNextTile(List<GridTile> openSet)
    {
        if (aiPathingType == PathingType.AStar)
        {
            return openSet.OrderBy(x => x.f).First();
        }
        return openSet.OrderBy(x => x.g).First();
    }

    GameObject GetClosestObjectWithTag(string TargetTag)
    {
        GameObject Closest = new GameObject();
        float ClosestDistance = 9999;
        
        GameObject[] ObjectsWithTag = GameObject.FindGameObjectsWithTag(TargetTag);
        foreach (var Object in ObjectsWithTag)
        {
            float Distance = Vector3.Distance(componentTransform.position, Object.transform.position);

            if (Distance < ClosestDistance)
            {
                Closest = Object;
                ClosestDistance = Distance;
            }
        }

        return Closest;
    }
}
