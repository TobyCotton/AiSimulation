using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public enum Side
{
    m_x1,
    m_x2,
    m_z1,
    m_z2,
}
public class ProceduralInput : MonoBehaviour
{
    public GameObject m_house1;
    public GameObject m_house2;
    public GameObject m_house3;
    public GameObject m_tile1;
    public GameObject m_tile2;
    public GameObject m_tile3;
    public GameObject m_tile4;
    public GameObject m_tile5;
    public GameObject m_tile6;
    public GameObject m_tile7;
    public Grid grid;
    public int length;
    public int width;
    private TileInfo[,] m_Grid;
    private List<TileInfo> m_toActivate = new List<TileInfo>();
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        length = (int)mySize.x;
        width =  (int)mySize.z;
        grid = new Grid(length,width);
        m_Grid = new TileInfo[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid.gridArray[i, j].isWalkable = true;
            }
        }
        for (int o = 0; o < length; o++)
        {
            for (int p = 0; p < width; p++)
            {
                m_Grid[o, p] = new TileInfo(m_tile1, m_tile2, m_tile3, m_tile4, m_tile5, m_tile6, m_tile7, o, p);
            }
        }
        int l = length / 2;
        int d = width / 2;
        m_Grid[l, d].i = l;
        m_Grid[l, d].j = d;
        m_Grid[l, d].m_chosen = m_Grid[l, d].m_availableTiles[Random.Range(0, m_Grid[l, d].m_availableTiles.Count)];
        m_toActivate.Add(m_Grid[l, d]);
        for (int i = 0; i < 100; i++)
        {
            int choice = Random.Range(1, 4);
            if (choice == 1)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                int rotationAmount = Random.Range(0, 4) * 90;
                m_house1.transform.rotation = transform.rotation;
                m_house1.transform.Rotate(0, rotationAmount, 0, Space.Self);
                Vector3 size = m_house1.transform.localScale;
                if (ValidPosition(x, z, size, rotationAmount))
                {
                    var housePosition = new Vector3(x, 5.0f, z);
                    Instantiate(m_house1, housePosition, m_house1.transform.rotation);
                }
            }
            if (choice == 2)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                int rotationAmount = Random.Range(0, 4) * 90;
                m_house2.transform.rotation = transform.rotation;
                m_house2.transform.Rotate(0, rotationAmount, 0, Space.Self);
                Vector3 size = m_house2.transform.localScale;
                if (ValidPosition(x, z, size, rotationAmount))
                {
                    var housePosition = new Vector3(x, 5.0f, z);
                    Instantiate(m_house2, housePosition, m_house2.transform.rotation);
                }
            }
            if (choice == 3)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                int rotationAmount = Random.Range(0, 4) * 90;
                m_house3.transform.rotation = transform.rotation;
                m_house3.transform.Rotate(0, rotationAmount, 0, Space.Self);
                Vector3 size = m_house3.transform.localScale;
                if (ValidPosition(x, z, size, rotationAmount))
                {
                    var housePosition = new Vector3(x, 10.0f, z);
                    Instantiate(m_house3, housePosition, m_house3.transform.rotation);
                }
            }
        }
    }

    bool ValidPosition(int x,int z,Vector3 size,int rotation)
    {
        int UseX = (int)size.x / 2;
        int UseZ = (int)size.z / 2;
        if (rotation == 90 || rotation == 270)
        {
            int temp = UseZ;
            UseZ = UseX;
            UseX = temp;
        }
        if (x+UseX >= length || x-UseX < 0 || z+UseZ >= width || z-UseZ < 0)
        {
            return false;
        }
        //Debug.Log("x:" + grid.gridArray.Length.ToString());
        //Debug.Log("x:" + x.ToString());
        //Debug.Log("z:" + z.ToString());
        //Debug.Log("Usex:" + UseX.ToString());
        //Debug.Log("UseZ:" + UseZ.ToString());
        for (int i = -UseX; i < UseX; i++)
        {
            for (int j = -UseZ; j < UseZ; j++)
            {
                if (grid.gridArray[x + i, z + j].isWalkable != true)
                {
                    return false;
                }
            }
        }
        for (int i = -UseX; i < UseX; i++)
        {
            for (int j = -UseZ; j < UseZ; j++)
            {
                grid.gridArray[x + i, z + j].isWalkable = false;
            }
        }
        return true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<TileInfo> toAdd = new List<TileInfo>();
            while (m_toActivate.Count != 0)
            {
                int i = m_toActivate[0].i;
                int j = m_toActivate[0].j;
                if (!m_toActivate[0].m_enabled)
                {
                    m_toActivate[0].m_chosen = m_toActivate[0].m_availableTiles[Random.Range(0, m_toActivate[0].m_availableTiles.Count)];
                    m_toActivate[0].m_enabled = true;
                    Instantiate(m_toActivate[0].m_chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), m_toActivate[0].m_chosen.transform.rotation);
                    if (i == 0)
                    {
                        if (!m_Grid[i + 1, j].m_enabled)
                        {
                            SelectTile(m_Grid[i + 1, j], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_x1, Side.m_x2);
                            m_toActivate.Add(m_Grid[i + 1, j]);
                        }
                    }
                    else if (i == length - 1)
                    {
                        if (!m_Grid[i - 1, j].m_enabled)
                        {
                            SelectTile(m_Grid[i - 1, j], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_x2, Side.m_x1);
                            m_toActivate.Add(m_Grid[i - 1, j]);
                        }
                    }
                    else
                    {
                        if (!m_Grid[i + 1, j].m_enabled)
                        {
                            SelectTile(m_Grid[i + 1, j], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_x1, Side.m_x2);
                            m_toActivate.Add(m_Grid[i + 1, j]);
                        }
                        if (!m_Grid[i - 1, j].m_enabled)
                        {
                            SelectTile(m_Grid[i - 1, j], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_x2, Side.m_x1);
                            m_toActivate.Add(m_Grid[i - 1, j]);
                        }
                    }
                    if (j == 0)
                    {
                        if (!m_Grid[i, j + 1].m_enabled)
                        {
                            SelectTile(m_Grid[i, j + 1], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_z1, Side.m_z2);
                            m_toActivate.Add(m_Grid[i, j + 1]);
                        }
                    }
                    else if (j == width - 1)
                    {
                        if (!m_Grid[i, j - 1].m_enabled)
                        {
                            SelectTile(m_Grid[i, j - 1], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_z2, Side.m_z1);
                            m_toActivate.Add(m_Grid[i, j - 1]);
                        }
                    }
                    else
                    {
                        if (!m_Grid[i, j + 1].m_enabled)
                        {
                            SelectTile(m_Grid[i, j + 1], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_z1, Side.m_z2);
                            m_toActivate.Add(m_Grid[i, j + 1]);
                        }
                        if (!m_Grid[i, j - 1].m_enabled)
                        {
                            SelectTile(m_Grid[i, j - 1], m_Grid[i, j].m_chosen.GetComponent<TileScript>().m_z2, Side.m_z1);
                            m_toActivate.Add(m_Grid[i, j - 1]);
                        }
                    }
                }
                m_toActivate.Remove(m_toActivate[0]);
            }
        }
    }
    void SelectTile(TileInfo toSelect, Orientation a, Side opposite)
    {
        Orientation b;
        var availableTiles = toSelect.m_availableTiles;
        List<GameObject> toRemove = new List<GameObject>();
        foreach (GameObject tile in availableTiles)
        {
            TileScript ScriptComponent = tile.GetComponent<TileScript>();
            switch (opposite)
            {
                case Side.m_x1:
                    b = ScriptComponent.m_x1;
                    break;
                case Side.m_x2:
                    b = ScriptComponent.m_x2;
                    break;
                case Side.m_z1:
                    b = ScriptComponent.m_z1;
                    break;
                case Side.m_z2:
                    b = ScriptComponent.m_z2;
                    break;
                default:
                    b = Orientation.Road;
                    break;
            }
            if (a != b)
            {
                toRemove.Add(tile);
            }
        }
        foreach (GameObject tile in toRemove)
        {
            availableTiles.Remove(tile);
        }
    }
}
