using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{
    public float[,] heights;
    private int m_number = 0;
    public string m_name;

    public void OutputMap()//Used to output to explorer
    {
        int width = heights.GetLength(0);
        int height = heights.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];

        for(int y =0; y < height;y++)
        {
            for(int x = 0; x < width; x++)
            {
                colourMap[(y * width) + x] = Color.Lerp(Color.black, Color.white, heights[x,y]);//Make height map between the colors of black and white
            }
        }
        texture.SetPixels(colourMap);
        texture.Apply();

        byte[] outputMap = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/../SaveImages/";
        if (!System.IO.Directory.Exists(dirPath))//Make sure dir exists if not create it
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath+ "Image"+m_name+Convert.ToString(m_number)+".png", outputMap);
        m_number++;
    }
}
