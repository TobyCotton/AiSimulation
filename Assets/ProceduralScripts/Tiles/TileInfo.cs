using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//change available tiles to enum add prefabs to proceduralinput find the prefab based on the enum
public class TileInfo : MonoBehaviour
{
    public int i;
    public int j;
    public List<TileScript> m_availableTiles;
    [SerializeField] TileScript m_tile1;
    [SerializeField] TileScript m_tile2;
    [SerializeField] TileScript m_tile3;
    [SerializeField] TileScript m_tile4;
    [SerializeField] TileScript m_tile5;
    [SerializeField] TileScript m_tile6;
    [SerializeField] TileScript m_tile7;
    public TileScript m_chosen;
    public TileInfo()
    {
        m_availableTiles.Add(m_tile1);
        m_availableTiles.Add(m_tile2);
        m_availableTiles.Add(m_tile3);
        m_availableTiles.Add(m_tile4);
        m_availableTiles.Add(m_tile5);
        m_availableTiles.Add(m_tile6);
        m_availableTiles.Add(m_tile7);
        Debug.Log("Help");
    }
}
