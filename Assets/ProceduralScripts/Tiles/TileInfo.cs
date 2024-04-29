using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//change available tiles to enum add prefabs to proceduralinput find the prefab based on the enum
public class TileInfo : MonoBehaviour
{
    public int i;
    public int j;
    public List<GameObject> m_availableTiles = new List<GameObject>();
    public List<GameObject> m_priority = new List<GameObject>();
    public bool m_enabled = false;
    public GameObject m_chosen;
    public TileInfo(GameObject tile1, GameObject tile2, GameObject tile3, GameObject tile4, GameObject tile5, GameObject tile6, GameObject tile7, GameObject tile8, int i1,int j1)
    {
        m_availableTiles.Add(tile1);
        m_availableTiles.Add(tile2);
        m_availableTiles.Add(tile3);
        m_availableTiles.Add(tile4);
        for(int i =0; i < 100; i++)//More heavily weight these tiles
        {
            m_availableTiles.Add(tile5);
        }
        for (int i = 0; i < 15; i++)//More heavily weight these tiles
        {
            m_availableTiles.Add(tile6);
        }
        for (int i = 0; i < 15; i++)//More heavily weight these tiles
        {
            m_availableTiles.Add(tile7);
        }
        m_availableTiles.Add(tile8);
        i = i1;
        j = j1;
    }
}
