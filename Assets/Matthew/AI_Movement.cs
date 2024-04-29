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
        //this chooses the pathing type to be used
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
        
        //this creates the comparison used for the heap
        comparison = (lhs, rhs) =>
        {
            int compare = lhs.f.CompareTo(rhs.f);
            if (compare == 0) {
                compare = lhs.h.CompareTo(rhs.h);
            }
            return -compare;
        };
        
        
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        //sets the heap
        openSet = new Heap<GridTile>(grid.MaxSize,comparer);
        List<GridTile> closedSet = new List<GridTile>();

        //gets the grid tiles for the start and end
        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            //gets next tile in heap
            GridTile tile = openSet.RemoveFirst();
            closedSet.Add(tile);
            
            //checks if current tile is the end tile
            if (tile.gridPos == targetTile.gridPos)
            {
                timer.Stop();
                retracePath(startTile, targetTile);
                TimeSpan timespan = timer.Elapsed;
                Debug.Log("A*'s: " + timespan.Milliseconds + " milliseconds");
                openSet.Clear();
                return;
            }

            //checks the neighbours
            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                //checks if neighbour is blocked or already explored
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //creates new cost for the neighbour tile
                float CostToNeighbour = tile.g + GetDistance(tile, neighbour);

                //adds weight for tile if it is being used
                if (usePaths)
                {
                    CostToNeighbour += neighbour.weight;
                }

                //checks if the new g cost is less than the neighbours current or if it is not in the heap
                if (CostToNeighbour < neighbour.g || !openSet.Contains(neighbour))
                {
                    //sets the G cost, H cost and the parent tile for the neighbour
                    neighbour.g = CostToNeighbour;
                    neighbour.h = GetDistance(neighbour, targetTile);
                    neighbour.previousTile = tile;

                    //adds the tile to the heap if its not already in, or updates it if it is
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

            //for the visualisation of the grid
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
        
        //this creates the comparison used for the heap
        Comparison<GridTile> comparison;
        comparison = (lhs, rhs) =>
        {
            int compare = lhs.g.CompareTo(rhs.g);
            return -compare;
        };

        //creates list of distances for each tile
        Dictionary<GridTile, float> dist = new Dictionary<GridTile, float>();
        foreach (GridTile tile in grid.gridArray)
        {
            dist.Add(tile, float.PositiveInfinity);
        }
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        //sets the heap
        openSet = new Heap<GridTile>(grid.MaxSize, comparer);
        List<GridTile> closedSet = new List<GridTile>();

        //gets the grid tiles for the start and end from the world positions
        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            //gets next tile in heap
            GridTile tile = openSet.RemoveFirst();
            closedSet.Add(tile);

            //checks if current tile is the end tile
            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                openSet.Clear();
                timer.Stop();
                TimeSpan timespan = timer.Elapsed;
                Debug.Log("Dijkstra's: " + timespan.Milliseconds + " milliseconds");
                return;
            }

            //checks the neighbours
            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                //checks if neighbour is blocked or has already been explored
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //creates new cost for the neighbour tile
                float CostToNeighbour = tile.g + GetDistance(tile, neighbour);
                
                //adds weight for tile if it is being used
                if (usePaths)
                {
                    CostToNeighbour += neighbour.weight;
                }
                //checks if the cost to the neighbour is less than its stored distance
                if (CostToNeighbour < dist[neighbour])
                {
                    //updates the distance, g cost and neighbours parent
                    dist[neighbour] = CostToNeighbour;
                    neighbour.g = CostToNeighbour;
                    neighbour.previousTile = tile;

                    //adds it to the heap or updates it if already in
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

            //for the visualisation of the grid
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
        
        //creates queue for explorable tile
        Queue<GridTile> openSet = new Queue<GridTile>();
        
        //creates list for explored tile
        List<GridTile> closedSet = new List<GridTile>();

        //gets the grid tiles for the start and end from the world positions
        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        //adds the start tile to the queue
        openSet.Enqueue(startTile);

        while (openSet.Count > 0)
        {
            //removes the tile from the front of the queue
            GridTile tile = openSet.Dequeue();
            
            closedSet.Add(tile);

            //checks if current tile is the end tile
            if (tile.gridPos == targetTile.gridPos)
            {
                retracePath(startTile, targetTile);
                timer.Stop();
                TimeSpan timespan = timer.Elapsed;
                Debug.Log("Breadth first time: " + timespan.Milliseconds + " milliseconds");
                return;
            }
            
            //checks the neighbours
            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                //checks if neighbour is blocked or has already been explored
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                
                //checks if tile has already been explored
                if (!openSet.Contains(neighbour))
                {
                    //sets the tiles parent to be the current tile
                    neighbour.previousTile = tile;
                    //adds it to the queue if its not already in it
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour);
                    }
                }
            }
            //for visualisation
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
        
        //creates comparision for heap
        Comparison<GridTile> comparison = (lhs, rhs) =>
        {
            int compare = lhs.h.CompareTo(rhs.h);
            return -compare;
        };
        
        Comparer<GridTile> comparer = Comparer<GridTile>.Create(comparison);
        //sets heap
        Heap<GridTile> openSet = new Heap<GridTile>(grid.MaxSize,comparer);
        
        //gets the grid tiles for the start and end from the world positions
        GridTile startTile = grid.TileFromWorldPoint(startPos);
        GridTile targetTile = grid.TileFromWorldPoint(endPos);

        List<GridTile> closedSet = new List<GridTile>();
        
        openSet.Add(startTile);
        closedSet.Add(startTile);

        while (openSet.Count > 0)
        {
            GridTile tile = openSet.RemoveFirst();

            //checks if current tile is the end tile
            if (tile == targetTile)
            {
                retracePath(startTile, targetTile);
                timer.Stop();
                Debug.Log("Best First Search: " + timer.Elapsed.Milliseconds + " milliseconds");
                openSet.Clear();
                return;
            }

            foreach (GridTile neighbour in grid.GetNeighbours(tile, diagonal))
            {
                //checks if neighbour is blocked or has already been explored
                if (!closedSet.Contains(neighbour) && neighbour.isWalkable)
                {
                    //sets the h cost and tiles parent and adds it to the heap
                    neighbour.h = GetDistance(neighbour, targetTile);
                    openSet.Add(neighbour);
                    closedSet.Add(neighbour);
                    neighbour.previousTile = tile;
                }
            }
            //for visualisation
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

        //goes through each tiles parent starting with the end tile until it reaches the start tile to get the path
        while (tile != start) {
            newPath.Add(tile);
            tile = tile.previousTile;
        }

        //reverse the path so that it starts with the start tile
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
        //gets the closest object with a tag so that the building being moved to is the closest instead of the first in the list
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
