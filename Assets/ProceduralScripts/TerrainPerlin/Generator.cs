using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int m_width;
    public int m_height;
    public float m_scale;
    public bool m_autoUpdate;

    public int m_octaves;
    [Range(0,1)]
    public float m_persistance;
    public float m_octaveFrequency;
    public int m_seed;
    public Vector2 m_offset;

    public void GenerateMap()
    {
        float[,] heights = PerlinNoiseTerrain.GenerateNoise(m_width, m_height, m_scale,m_octaves,m_persistance,m_octaveFrequency,m_seed, m_offset);
        Displayer display = FindObjectOfType<Displayer>();
        display.DrawMap(heights);
    }

    void OnValidate()
    {
        if(m_width < 1)
        {
            m_width = 1;
        }
        if( m_height < 1)
        {
            m_height = 1;
        }
        if(m_octaveFrequency < 1)
        {
            m_octaveFrequency = 1;
        }
        if(m_octaves < 1)
        {
            m_octaves = 1;
        }
    }
}


