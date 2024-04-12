using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PerlinNoiseTerrain : MonoBehaviour
{
    public Terrain m_Terrain;
    private TerrainData m_TerrainData;
    private float[,] m_heights;
    int[] permutation = {151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7,
                        225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247,
                        120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
                        88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134,
                        139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220,
                        105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80,
                        73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
                        164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38,
                        147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
                        28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101,
                        155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232,
                        178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12,
                        191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181,
                        199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236,
                        205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180};

    void Start()
    {
        m_TerrainData = m_Terrain.terrainData;
        generateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int heightMapResolution = m_TerrainData.heightmapResolution;
            for (int z = 0; z < heightMapResolution; z++)
            {
                for (int x = 0; x < heightMapResolution; x++)
                {
                    m_heights[x, z] = 0;
                }
            }
            m_TerrainData.SetHeights(0, 0, m_heights);
        }
    }

    private void generateTerrain()
    {
        int heightMapResolution = m_TerrainData.heightmapResolution;

        m_heights = m_TerrainData.GetHeights(0, 0,heightMapResolution,heightMapResolution);
        float previous = 0.0f;
        for (int z = 0; z < heightMapResolution;z++)
        {
            for(int x = 0; x < heightMapResolution;x++)
            {
                if(Random.Range(0, 2) == 0)
                {
                    previous += 0.001f;
                }
                else
                {
                    previous -= 0.001f;
                }
                m_heights[x, z] = previous;
                if(x+1 == heightMapResolution)
                {
                    if (z + 1 < heightMapResolution)
                    {
                        previous = m_heights[0, z + 1];
                    }
                }
            }
        }

        m_TerrainData.SetHeights(0,0, m_heights);
    }

    private float Noise3D(float x,float y,float z)
    {
        double newX = Math.Floor(x)/255;
        double newY = Math.Floor(y)/255;
        double newZ = Math.Floor(z)/255;

        float xf = (float)(x - Math.Floor(x));
        float yf = (float)(y - Math.Floor(y));
        float zf = (float)(z - Math.Floor(z));

        Vector3 cornerA = new Vector3(xf - 1.0f, yf - 1.0f, zf - 1.0f);
        Vector3 cornerB = new Vector3(xf - 1.0f, yf + 1.0f, zf - 1.0f);
        Vector3 cornerC = new Vector3(xf - 1.0f, yf + 1.0f, zf + 1.0f);
        Vector3 cornerD = new Vector3(xf - 1.0f, yf - 1.0f, zf + 1.0f);
        Vector3 cornerE = new Vector3(xf + 1.0f, yf - 1.0f, zf - 1.0f);
        Vector3 cornerF = new Vector3(xf + 1.0f, yf + 1.0f, zf - 1.0f);
        Vector3 cornerG = new Vector3(xf + 1.0f, yf + 1.0f, zf + 1.0f);
        Vector3 cornerH = new Vector3(xf + 1.0f, yf - 1.0f, zf + 1.0f);

        int valueA = permutation[permutation[permutation[newX+0] +0] + 0];
        int valueB = permutation[permutation[permutation[newX+0] + 1] + 0];
        int valueC = permutation[permutation[permutation[0] + 1] + 1];
        int valueD = permutation[permutation[permutation[0] + 0] + 1];
        int valueE = permutation[permutation[permutation[1] + 0] + 0];
        int valueF = permutation[permutation[permutation[1] + 1] + 0];
        int valueG = permutation[permutation[permutation[1] + 1] + 1];
        int valueH = permutation[permutation[permutation[1] + 0] + 1];
        return 0.0f;
    }
}
