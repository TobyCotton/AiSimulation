using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public static class PerlinNoiseTerrain
{
    public static float[,] GenerateNoise(int width,int height, float scale,int octaves, float persistance, float octaveIncrease,int seed, Vector2 offset)
    {
        float[,] heights = new float[width,height];
        Vector2[] octaveOffsets = new Vector2[octaves];


        System.Random rand = new System.Random(seed);
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = rand.Next(-10000, 10000) + offset.x;
            float offsetY = rand.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float max = float.MinValue;
        float min = float.MaxValue;

        float halfW = width/2;
        float halfH = height/2;

        if(scale <= 0.251f)
        {
            scale = 0.251f;
        }
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x- halfW) / (scale * frequency) + octaveOffsets[i].x;
                    float sampleY = (y-halfH) / (scale * frequency) + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 -1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= octaveIncrease;
                }
                heights[x, y] = noiseHeight;
                if(noiseHeight > max)
                {
                    max = noiseHeight;
                }
                else if(noiseHeight < min)
                {
                    min = noiseHeight;
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heights[x, y] = Mathf.InverseLerp(min, max, heights[x, y]);// will return 0 -1 range based on min and max
            }
        }

        return heights;
    }
}
