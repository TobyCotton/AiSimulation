using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PerlinNoiseTerrain
{
    public static float[,] GenerateNoise(int width,int height, float scale,int octaves, float persistance, float octaveIncrease,int seed, Vector2 offset, int[] Permutation, AnimationCurve slope)
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

                    float perlinValue = Noise2D(sampleX, sampleY, Permutation) * 2 -1;
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
                heights[x, y] = slope.Evaluate(Mathf.InverseLerp(min, max, heights[x, y]));//mathf inverselerp will return 0-1 with the min and max
            }//animation curve evaluate will then further edit these values to allow more custimisation by the user to get there ideal terrain instead of just slopes
        }

        return heights;
    }
    public static float Noise2D(float x, float y, int[] Permutation)//Based of pseudocode from https://rtouti.github.io/graphics/perlin-noise-algorithm
    {
        int X = (int)Math.Floor(x) & 255;
        int Y = (int)Math.Floor(y) & 255;

        float xf = x - (float)Math.Floor(x);
        float yf = y - (float)Math.Floor(y);

        Vector2 topRight = new Vector2(xf - 1.0f, yf - 1.0f);
        Vector2 topLeft = new Vector2(xf, yf - 1.0f);
        Vector2 bottomRight = new Vector2(xf - 1.0f, yf);
        Vector2 bottomLeft = new Vector2(xf, yf);

        int valueTopRight = Permutation[(Permutation[(X + 1)&255] + Y + 1) & 255];//Added in bitwise operations to stop overflow
        int valueTopLeft = Permutation[(Permutation[X] + Y + 1) & 255];// also allows for it to overflow down to 0 instead of going to 256
        int valueBottomRight = Permutation[(Permutation[(X + 1) & 255] + Y) & 255];
        int valueBottomLeft = Permutation[(Permutation[X] + Y) & 255];

        int temp1 = (int)valueTopRight&3;
        int temp2 = (int)valueTopLeft & 3;
        int temp3 = (int)valueBottomRight & 3;
        int temp4 = (int)valueBottomLeft & 3;

        Vector2 topRightVector = SelectCorner(temp1);
        Vector2 topLeftVector = SelectCorner(temp2);
        Vector2 bottomRightVector = SelectCorner(temp3);
        Vector2 bottomLeftVector = SelectCorner(temp4);

        float dotTopRight = (topRight.x * topRightVector.x) + (topRight.y * topRightVector.y);//added in brackets as bidmas isn't followed in c# here
        float dotTopLeft = (topLeft.x * topLeftVector.x) + (topLeft.y * topLeftVector.y);
        float dotBottomRight = (bottomRight.x * bottomRightVector.x) + (bottomRight.y * bottomRightVector.y);
        float dotBottomLeft = (bottomLeft.x * bottomLeftVector.x) + (bottomLeft.y * bottomLeftVector.y);

        float u = ((6 * xf - 15) * xf + 10) * xf * xf * xf;// Fade from the point to the edges
        float v = ((6 * yf - 15) * yf + 10) * yf * yf * yf;

        return LerpMine(u,
            LerpMine(v, dotBottomLeft, dotTopLeft),
            LerpMine(v, dotBottomRight, dotTopRight)
        );
    }
    private static Vector2 SelectCorner(int c)
    {
        switch (c)
        {
            case 0:
                return new Vector2(1.0f, 1.0f);
            case 1:
                return new Vector2(-1.0f, 1.0f);
            case 2:
                return new Vector2(-1.0f, -1.0f);
            case 3:
                return new Vector2(1.0f, -1.0f);
        }

        return new Vector2(-2.0f, -2.0f);
    }
    private static float LerpMine(float a, float b, float c)
    {
        return b + a * (c - b);
    }
}
