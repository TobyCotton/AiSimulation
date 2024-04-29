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
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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
    public Grid grid;
    private Transform componentTransform;
    private ProceduralInput Terrain;
    private int speed = 10;
    private float WaitTime;
    private List<GridTile> path;
    private Agent PlannerAgent;
    private Heap<GridTile> openSet;

    public bool Visualise;
    public bool diagonal;
    public bool usePaths;
    public bool test;

    void Start()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<ProceduralInput>();
        if (Terrain != null)
        {
            if (Terrain.grid != null)
            {
                grid = Terrain.grid;
                
                Debug.Log("ADDING GRID");
            }
        }
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
        if (!test)
        {
            GameObject TargetBuilding = GetClosestObjectWithTag(TargetTag);
            Vector3 TargetPosition = TargetBuilding.transform.Find("Entrance").position;
            StartPathing(componentTransform.position, TargetPosition);
        }
    }

    public void StartPathing(Vector3 startPos, Vector3 endPos)
    {
        

        if (grid == null)
        {
            grid = Terrain.grid;
        }
        if (Visualise)
        {
            resetTileColours();
        }
        switch (aiPathingType)
        {
            case PathingType.AStar:
                AStarPathing(startPos, endPos);
                break;
            case PathingType.Dijkstra:
                Dijkstra(startPos, endPos);
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
        Stopwatch timer = Stopwatch.StartNew();

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
        openSet = new Heap<GridTile>(grid.MaxSize,comparer);
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();
            closedSet.Add(tile);
            
            if (tile.gridPos == targetTile.gridPos)
            {
                timer.Stop();
                retracePath(startTile, targetTile);
                openSet.Clear();
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                float CostToNeighbour = tile.g + GetDistance(tile, neighbour);

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

    async void Dijkstra(Vector3 startPos, Vector3 endPos)
    {
        Stopwatch timer = Stopwatch.StartNew();

        Comparison<GridTile> comparison;
        comparison = (lhs, rhs) =>
        {
            int compare = lhs.g.CompareTo(rhs.g);
            return -compare;
        };

        Dictionary<GridTile, float> dist = new Dictionary<GridTile, float>();
        foreach (GridTile tile in grid.gridArray)
        {
            dist.Add(tile, float.PositiveInfinity);
        }
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        openSet = new Heap<GridTile>(grid.MaxSize, comparer);
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();
            closedSet.Add(tile);

            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                openSet.Clear();
                timer.Stop();
                TimeSpan timespan = timer.Elapsed;
                Debug.Log("Dijkstra's: " + timespan.Milliseconds);
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }


                float CostToNeighbour = tile.g + GetDistance(tile, neighbour);
                if (usePaths)
                {
                    CostToNeighbour += neighbour.weight;
                }
                if (CostToNeighbour < dist[neighbour])
                {
                    dist[neighbour] = CostToNeighbour;
                    neighbour.g = CostToNeighbour;
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
        Stopwatch timer = Stopwatch.StartNew();
        Queue<GridTile> openSet = new Queue<GridTile>();
        
        List<GridTile> closedSet = new List<GridTile>();

        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        openSet.Enqueue(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.Dequeue();
            
            closedSet.Add(tile);

            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                timer.Stop();
                TimeSpan timespan = timer.Elapsed;
                Debug.Log("Breadth first time: " + timespan.Milliseconds);
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
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
        Stopwatch timer = Stopwatch.StartNew();
        Comparison<GridTile> comparison = (lhs, rhs) =>
        {
            int compare = lhs.h.CompareTo(rhs.h);
            return -compare;
        };
        
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        Heap<GridTile> openSet = new Heap<GridTile>(grid.MaxSize,comparer);
        
        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        List<GridTile> closedSet = new List<GridTile>();
        
        openSet.Add(startTile);
        closedSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();

            if (tile == targetTile)
            {
                retracePath(startTile, targetTile);
                timer.Stop();
                Debug.Log("Best First Search: " + timer.Elapsed.Milliseconds);
                openSet.Clear();
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                if (!closedSet.Contains(neighbour) && neighbour.isWalkable)
                {
                    neighbour.h = GetDistance(neighbour, targetTile);
                    openSet.Add(neighbour);
                    closedSet.Add(neighbour);
                    neighbour.previousTile = tile;
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

        switch (aiPathingType)
        {
            case PathingType.AStar:
                Debug.Log("A Star path length: " + path.Count);
                break;
            case PathingType.Dijkstra:
                Debug.Log("Dijkstra path length: " + path.Count);
                break;
            case PathingType.BreadthFirstSearch:
                Debug.Log("BreadthFirstSearch path length: " + path.Count);
                break;
            case PathingType.BestFirstSearch:
                Debug.Log("BestFirstSearch length: " + path.Count);
                break;
        }
    }

    int GetDistance(GridTile tileA, GridTile tileB)
    {
        int dstX = Mathf.Abs(Convert.ToInt32(tileA.gridPos.x - tileB.gridPos.x));
        int dstY = Mathf.Abs(Convert.ToInt32(tileA.gridPos.y - tileB.gridPos.y));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
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
        for (int i = 0; i < grid.gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < grid.gridArray.GetLength(1); j++)
            {
                var s = grid.gridArray[i,j].renderer;
                if (grid.gridArray[i,j].isGrass)
                {
                    if (grid.gridArray[i,j].isWalkable)
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
        for (int i = 0; i < openSet.Count; i++)
        {
            GridTile tile = openSet.items[i];
            var s = tile.renderer;
            s.color = Color.white;
        }

        var visualiser = targetTile.renderer;
        visualiser.color = Color.magenta;
    }
}
