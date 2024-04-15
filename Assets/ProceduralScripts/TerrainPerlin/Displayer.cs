using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{
    public Renderer m_textureRenderer;

    public void DrawMap(float[,] heights)
    {
        int width = heights.GetLength(0); ;
        int height = heights.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colors = new Color[width*height];

        for(int y =0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                colors[y * width + x] = Color.Lerp(Color.black, Color.white, heights[x, y]);
            }
        }
        texture.SetPixels(colors);
        texture.Apply();

        m_textureRenderer.sharedMaterial.mainTexture = texture;
        m_textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
