using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid
{
    public Sprite sprite;
    int width;
    int height;
    public GridTile[,] gridArray;
    
    public Grid(int width, int height)
    {

        this.width = width;
        this.height = height;

        gridArray = new GridTile[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++) 
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = new GridTile(new Vector3((1 * i) + 0.5f, 1, (1 * j) + 0.5f), new Vector2(i, j));
            }
        }
    }

    public List<GridTile> GetNeighbours(GridTile tile, bool diagonal) 
    {
        List<GridTile> neighbours = new List<GridTile>();
        if (diagonal)
        {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = Convert.ToInt32(tile.gridPos.x) + x;
                    int checkY = Convert.ToInt32(tile.gridPos.y) + y;

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) {
                        neighbours.Add(gridArray[checkX, checkY]);
                    }
                }
            }
        }
        else
        {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if ((x == 0 && y != 0) || (x != 0 && y == 0))
                    {
                        int checkX = Convert.ToInt32(tile.gridPos.x) + x;
                        int checkY = Convert.ToInt32(tile.gridPos.y) + y;

                        if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) {
                            neighbours.Add(gridArray[checkX, checkY]);
                        }
                    }
                   
                }
            }
        }
        return neighbours;
    }

    public GridTile TileFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(MathF.Floor(worldPosition.x));
        int y = Mathf.RoundToInt(MathF.Floor(worldPosition.z));
        return gridArray[x, y];
    }

    public void RenderTiles()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j].Tile = new GameObject("x: " + gridArray[i, j].worldPos.x + " z: " + gridArray[i, j].worldPos.z);
                gridArray[i, j].Tile.transform.position = gridArray[i, j].worldPos;
                gridArray[i, j].Tile.transform.position = new Vector3(gridArray[i, j].Tile.transform.position.x, 0.15f, gridArray[i, j].Tile.transform.position.z);
                gridArray[i, j].Tile.transform.Rotate(new Vector3(90, 0, 0));
                gridArray[i, j].Tile.layer = LayerMask.NameToLayer("GridTile");
                
                var s = gridArray[i, j].Tile.AddComponent<SpriteRenderer>();
                s.sprite = sprite;
               
                if (gridArray[i, j].isWalkable)
                {
                    s.color = gridArray[i,j].isGrass ? Color.green : Color.blue;
                }
                else
                {
                    s.color = Color.red;
                }
                gridArray[i, j].renderer = s;
            }
        }
    }

    public int MaxSize
    {
        get { return width * height; }
    }
}
