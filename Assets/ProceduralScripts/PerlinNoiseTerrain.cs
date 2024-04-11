using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PerlinNoiseTerrain : MonoBehaviour
{
    public Terrain m_Terrain;
    private TerrainData m_TerrainData;
    private float[,] m_heights;

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

        for (int z = 0; z < heightMapResolution;z++)
        {
            for(int x = 0; x < heightMapResolution;x++)
            {
                float cos = Mathf.Cos(x);
                float sin = - Mathf.Sin(z);
                m_heights[x, z] = (cos - sin) / 250;
            }
        }

        m_TerrainData.SetHeights(0,0, m_heights);
    }
}
