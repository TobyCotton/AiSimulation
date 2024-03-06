using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public TileScript m_tile1;
    public TileScript m_tile2;
    public TileScript m_tile3;
    public TileScript m_tile4;
    public TileScript m_tile5;
    public TileScript m_tile6;
    public TileScript m_tile7;
    public List<TileScript> m_tiles = new List<TileScript>();
    public TileHolder()
    {
        m_tiles.Add(m_tile1);
        m_tiles.Add(m_tile2);
        m_tiles.Add(m_tile3);
        m_tiles.Add(m_tile4);
        m_tiles.Add(m_tile5);
        m_tiles.Add(m_tile6);
        m_tiles.Add(m_tile7);
    }
}
