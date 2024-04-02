using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    public List<GridTile> GetNeighbours(GridTile tile) 
    {
        List<GridTile> neighbours = new List<GridTile>();
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
                GameObject tile = new GameObject("x: " + gridArray[i, j].worldPos.x + " z: " + gridArray[i, j].worldPos.z);
                tile.transform.position = gridArray[i, j].worldPos;
                tile.transform.Rotate(new Vector3(90, 0, 0));
                var s = tile.AddComponent<SpriteRenderer>();
                s.sprite = sprite;
                if (gridArray[i, j].isWalkable)
                {
                    if (gridArray[i,j].isGrass)
                    {
                        s.color = Color.green;
                    }
                    else
                    {
                        s.color = Color.blue;
                    }
                }
                else
                {
                    s.color = Color.red;
                }
            }
        }
    }
}
