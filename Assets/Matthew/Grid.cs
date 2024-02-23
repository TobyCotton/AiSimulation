using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
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
                gridArray[i, j] = new GridTile(new Vector3((1 * i) + 1 / 2, 1, (1 * j) + 1 / 2), new Vector2(i, j));
            }
        }
    }
}
