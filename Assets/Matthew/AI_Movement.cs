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
using System.Threading.Tasks;

public class AI_Movement : MonoBehaviour
{
    public enum PathingType
    {
        AStar,
        Dijkstra,
        BreadthFirstSearch,
        BestFirstSearch
    }

    public enum VisualisationSpeed
    {
        Fast,
        Average,
        Slow
    }

    public PathingType aiPathingType;
    public VisualisationSpeed VisulisationSpeed;
    private Grid grid;
    private Transform componentTransform;
    private ProceduralInput Terrain;
    private int speed = 10;
    private float WaitTime;
    private List<GridTile> path;
    private Agent PlannerAgent;

    public bool Visualise;
    public bool diagonal;
    public bool usePaths;

    void Start()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<ProceduralInput>();
        componentTransform = GetComponent<Transform>();
        PlannerAgent = GetComponent<Agent>();
        switch (VisulisationSpeed)
        {
            case VisualisationSpeed.Fast:
                WaitTime = 0.01f;
                break;
            case VisualisationSpeed.Average:
                WaitTime = 0.2f;
                break;
            case VisualisationSpeed.Slow:
                WaitTime = 0.5f;
                break;
            default:
                WaitTime = 0.01f;
                break;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        if (path != null)
        {
            if (path.Count > 0)
            {
                if (Visualise)
                {
                    foreach (GridTile visualisingTile in path)
                    {
                        GameObject visualisedTile =
                            GameObject.Find("x: " + visualisingTile.worldPos.x + " z: " + visualisingTile.worldPos.z);
                        var s = visualisedTile.GetComponent<SpriteRenderer>();
                        s.color = Color.yellow;
                    }
                }

                componentTransform.position = Vector3.MoveTowards(componentTransform.position, path[0].worldPos,
                    speed * Time.deltaTime);
                if (Vector3.Distance(componentTransform.position, path[0].worldPos) < 0.000001f)
                {
                    path.RemoveAt(0);
                }

                if (path.Count == 0)
                {
                    PlannerAgent.NotifyReachedGoal();
                }
            }
        }
    }

    public void moveTo(string TargetTag)
    {
        GameObject TargetBuilding = GetClosestObjectWithTag(TargetTag);
        Vector3 TargetPosition = TargetBuilding.transform.Find("Entrance").position;
        aiPathingType = Terrain.grid.TileFromWorldPoint(TargetPosition).entrancePathingType;
        StartPathing(componentTransform.position, TargetPosition);
    }

    private void StartPathing(Vector3 startPos, Vector3 endPos)
    {
        if (Visualise)
        {
            resetTileColours();
        }
        switch (aiPathingType)
        {
            case PathingType.AStar:
            case PathingType.Dijkstra:
                AStarPathing(startPos, endPos);
                break;
            case PathingType.BreadthFirstSearch:
                BreadthFirstSearch(startPos, endPos);
                break;
            case PathingType.BestFirstSearch:
                BestFirstSearch(startPos, endPos);
                break;
        }
    }
    async void AStarPathing(Vector3 startPos, Vector3 endPos)
    {
        Comparison<GridTile> comparison;
        if (aiPathingType == PathingType.AStar)
        {
            comparison = (lhs, rhs) =>
            {
                int compare = lhs.f.CompareTo(rhs.f);
                if (compare == 0) {
                    compare = lhs.h.CompareTo(rhs.h);
                }
                return -compare;
            };
        }
        else
        {
            comparison = (lhs, rhs) =>
            {
                int compare = lhs.g.CompareTo(rhs.g);
                return -compare;
            };
        }
        
        
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        Heap<GridTile> openSet = new Heap<GridTile>(Terrain.grid.MaxSize,comparer);
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();
            closedSet.Add(tile);
            
            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                return;
            }

            foreach (GridTile neighbour in Terrain.grid.GetNeighbours(tile, diagonal))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int CostToNeighbour = tile.g + GetDistance(tile, neighbour);
                if (usePaths)
                {
                    CostToNeighbour += neighbour.weight;
                }

                if (CostToNeighbour < neighbour.g || !openSet.Contains(neighbour))
                {
                    neighbour.g = CostToNeighbour;
                    neighbour.h = GetDistance(neighbour, targetTile);
                    neighbour.previousTile = tile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }

            if (Visualise)
            {
                VisualisePathing(openSet, closedSet, targetTile);
                await WaitAsync();
            }
        }
    }
    
    private async Task WaitAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(WaitTime));
    }

    async void BreadthFirstSearch(Vector3 startPos, Vector3 endPos)
    {
        Queue<GridTile> openSet = new Queue<GridTile>();
        
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);

        openSet.Enqueue(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.Dequeue();
            
            closedSet.Add(tile);

            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                return;
            }

            foreach (GridTile neighbour in Terrain.grid.GetNeighbours(tile, diagonal))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                

                if (!openSet.Contains(neighbour))
                {
                    neighbour.previousTile = tile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour);
                    }
                }
            }
            if (Visualise)
            {
                foreach (GridTile visualisingTile in openSet)
                {
                    var s = visualisingTile.renderer;
                    s.color = Color.white;
                }
                foreach (GridTile visualisingTile in closedSet)
                {
                    var s = visualisingTile.renderer;
                    if (visualisingTile.isWalkable && !visualisingTile.isGrass)
                    {
                
                        s.color = Color.grey;
                    }
                    else
                    {
                        s.color = Color.black;
                    }
                }
                var visualiser = targetTile.renderer;
                visualiser.color = Color.magenta;
                
                await WaitAsync();
            }
        }
    }

    async void BestFirstSearch(Vector3 startPos, Vector3 endPos)
    {
        Comparison<GridTile> comparison = (lhs, rhs) =>
        {
            int compare = lhs.h.CompareTo(rhs.h);
            return -compare;
        };
        
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        Heap<GridTile> openSet = new Heap<GridTile>(Terrain.grid.MaxSize,comparer);
        
        GridTile startTile = Terrain.grid.TileFromWorldPoint(startPos);
        GridTile targetTile = Terrain.grid.TileFromWorldPoint(endPos);

        List<GridTile> closedSet = new List<GridTile>();
        
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();

            closedSet.Add(tile);

            if (tile == targetTile)
            {
                retracePath(startTile, targetTile);
                return;
            }

            foreach (GridTile neighbour in Terrain.grid.GetNeighbours(tile, diagonal))
            {
                if (!closedSet.Contains(neighbour) && neighbour.isWalkable)
                {
                    neighbour.h = GetDistance(neighbour, targetTile);
                    if (tile.previousTile == null)
                    {
                        tile.previousTile = neighbour;
                    }
                    else
                    {
                        neighbour.previousTile = tile;
                    }
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
            if (Visualise)
            {
                VisualisePathing(openSet, closedSet, targetTile);
                await WaitAsync();
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
        else if (aiPathingType == PathingType.BestFirstSearch)
        {
            return openSet.OrderBy(x => x.h).First();
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

    void resetTileColours()
    {
        for (int i = 0; i < Terrain.grid.gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < Terrain.grid.gridArray.GetLength(1); j++)
            {
                var s = Terrain.grid.gridArray[i,j].renderer;
                if (Terrain.grid.gridArray[i,j].isGrass)
                {
                    if (Terrain.grid.gridArray[i,j].isWalkable)
                    {
                        s.color = Color.green;
                    }
                    else
                    {
                        s.color = Color.red;
                    }
                }
                else
                {
                    s.color = Color.blue;
                }
            }
        }
    }

    void VisualisePathing(Heap<GridTile> openSet, List<GridTile> closedSet, GridTile targetTile)
    {
        for (int i = 0; i < openSet.Count; i++)
        {
            GridTile tile = openSet.items[i];
            var s = tile.renderer;
            s.color = Color.white;
        }
        foreach (GridTile visualisingTile in closedSet)
        {
            var s = visualisingTile.renderer;
            if (visualisingTile.isWalkable && !visualisingTile.isGrass)
            {
                
                s.color = Color.grey;
            }
            else
            {
                s.color = Color.black;
            }
        }

        var visualiser = targetTile.renderer;
        visualiser.color = Color.magenta;
    }
}
