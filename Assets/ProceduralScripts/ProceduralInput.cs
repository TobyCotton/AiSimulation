using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    private int[,] m_squares;
    private int length;
    private int width;
    private TileInfo[,] m_Grid;
    private List<TileInfo> m_toActivate = new List<TileInfo>();
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        length = 9;//(int)mySize.x;
        width = 9;//(int)mySize.z;
        m_squares = new int[length, width];
        m_Grid = new TileInfo[length, width];
        for (int o = 0; o < length; o++)
        {
            for (int p = 0; p < width; p++)
            {
                m_squares[o,p] = 0;
            }
        }
        for (int o = 0; o < length; o++)
        {
            for (int p = 0; p < width; p++)
            {
                m_Grid[o, p] = new TileInfo();
            }
        }
        int i = length / 2;
        int j = width / 2;
        m_Grid[i, j].i = i;
        m_Grid[i, j].j = j;
        m_toActivate.Add(m_Grid[i, j]);
        CreateRoads();
        //for (int i = 0; i < 100; i++)
        //{
        //    int choice = Random.Range(1, 4);
        //    if (choice == 1)
        //    {
        //        int x = Random.Range(5, 95);
        //        int z = Random.Range(5, 95);
        //        int rotationAmount = Random.Range(0, 4) * 90;
        //        m_house1.transform.rotation = transform.rotation;
        //        m_house1.transform.Rotate(0, rotationAmount, 0, Space.Self);
        //        Vector3 size = m_house1.transform.localScale;
        //        if (ValidPosition(x, z, size, rotationAmount))
        //        {
        //            var housePosition = new Vector3(x, 5.0f, z);
        //            Instantiate(m_house1, housePosition, m_house1.transform.rotation);
        //        }
        //    }
        //    if (choice == 2)
        //    {
        //        int x = Random.Range(5, 95);
        //        int z = Random.Range(5, 95);
        //        int rotationAmount = Random.Range(0, 4) * 90;
        //        m_house2.transform.rotation = transform.rotation;
        //        m_house2.transform.Rotate(0, rotationAmount, 0, Space.Self);
        //        Vector3 size = m_house2.transform.localScale;
        //        if (ValidPosition(x, z, size, rotationAmount))
        //        {
        //            var housePosition = new Vector3(x, 5.0f, z);
        //            Instantiate(m_house2, housePosition, m_house2.transform.rotation);
        //        }
        //    }
        //    if (choice == 3)
        //    {
        //        int x = Random.Range(5, 95);
        //        int z = Random.Range(5, 95);
        //        int rotationAmount = Random.Range(0, 4) * 90;
        //        m_house3.transform.rotation = transform.rotation;
        //        m_house3.transform.Rotate(0, rotationAmount, 0, Space.Self);
        //        Vector3 size = m_house3.transform.localScale;
        //        if (ValidPosition(x, z, size, rotationAmount))
        //        {
        //            var housePosition = new Vector3(x, 10.0f, z);
        //            Instantiate(m_house3, housePosition, m_house3.transform.rotation);
        //        }
        //    }
        //}
    }

    private void CreateRoads()
    {
        //List<GameObject> usableTiles = new List<GameObject>();
        //for(int i = 0; i < m_Tiles.Length;i++)
        //{
        //    usableTiles.Add(m_Tiles[i]);
        //}
        //GameObject[] revertCopy = usableTiles.ToArray();
        //for (int i = 0; i < length; i++)
        //{
        //    for (int j = 0; j < width; j++)
        //    {
        //        if (!m_Grid[i, j])
        //        {
        //            if (i == 0 && j == 0)
        //            {
        //                usableTiles.Clear();
        //                usableTiles.Add(m_Tiles[3]);
        //                GameObject chosen = usableTiles[Random.Range(0, usableTiles.Count)];
        //                m_Grid[i, j] = chosen;
        //                Instantiate(chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), chosen.transform.rotation);
        //                usableTiles = revertCopy.ToList();
        //            }
        //            else
        //            {
        //                if (i == 0) { }
        //                else
        //                {
        //                    GameObject itemCheck = m_Grid[i - 1, j];
        //                    TileScript checkScript = itemCheck.GetComponent<TileScript>();
        //                    if (checkScript != null)
        //                    {
        //                        for (int k = 0; k < usableTiles.Count; k++)
        //                        {
        //                            TileScript compareScript = usableTiles[k].GetComponent<TileScript>();
        //                            if (compareScript)
        //                            {
        //                                if (compareScript.m_x2 != checkScript.m_x1)
        //                                {
        //                                    usableTiles.Remove(usableTiles[k]);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (j == 0) { }
        //                else
        //                {
        //                    GameObject itemCheck = m_Grid[i, j - 1];
        //                    TileScript checkScript = itemCheck.GetComponent<TileScript>();
        //                    if (checkScript != null)
        //                    {
        //                        for (int k = 0; k < usableTiles.Count; k++)
        //                        {
        //                            TileScript compareScript = usableTiles[k].GetComponent<TileScript>();
        //                            if (compareScript)
        //                            {
        //                                if (compareScript.m_z2 != checkScript.m_z1)
        //                                {
        //                                    usableTiles.Remove(usableTiles[k]);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                GameObject chosen = usableTiles[Random.Range(0, usableTiles.Count)];
        //                chosen.GetComponent<TileScript>().i = i;
        //                chosen.GetComponent<TileScript>().j = j;
        //                m_Grid[i, j] = chosen;
        //                Instantiate(chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), chosen.transform.rotation);
        //                usableTiles.Clear();
                        
        //                for (int o = 0; o < m_Tiles.Length; o++)
        //                {
        //                    usableTiles.Add(m_Tiles[o]);
        //                }
        //                Debug.Log(usableTiles.Count());
        //            }
        //        }
        //    }
        //}
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
        //Debug.Log("x:" + m_squares.Length.ToString());
        //Debug.Log("x:" + x.ToString());
        //Debug.Log("z:" + z.ToString());
        //Debug.Log("Usex:" + UseX.ToString());
        //Debug.Log("UseZ:" + UseZ.ToString());
        for (int i = -UseX; i < UseX; i++)
        {
            for (int j = -UseZ; j < UseZ; j++)
            {
                if (m_squares[x + i, z + j] != 0)
                {
                    return false;
                }
            }
        }
        for (int i = -UseX; i < UseX; i++)
        {
            for (int j = -UseZ; j < UseZ; j++)
            {
                m_squares[x + i, z + j] = 1;
            }
        }
        return true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<TileInfo> toAdd = new List<TileInfo>();
            while(m_toActivate.Count != 0)
            {
                int i = m_toActivate[0].i;
                int j = m_toActivate[0].j;
                m_toActivate[0].m_chosen = m_toActivate[0].m_availableTiles[Random.Range(0, m_toActivate[0].m_availableTiles.Count)];
                Instantiate(m_toActivate[0].m_chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), m_toActivate[0].m_chosen.transform.rotation);
                if (i == 0) 
                {
                    if (m_Grid[i+1,j].m_chosen == null)
                    {
                        SelectTile(m_Grid[i + 1, j], m_Grid[i , j].m_chosen.m_x1,Side.m_x2);
                        toAdd.Add(m_Grid[i+1 , j]);
                    }
                }
                else if (i == length)
                {
                    if (m_Grid[i-1,j].m_chosen == null)
                    {
                        SelectTile(m_Grid[i - 1, j], m_Grid[i, j].m_chosen.m_x2, Side.m_x1);
                        toAdd.Add(m_Grid[i - 1, j]);
                    }
                }
                else
                {
                    if (m_Grid[i + 1, j].m_chosen == null)
                    {
                        SelectTile(m_Grid[i + 1, j], m_Grid[i, j].m_chosen.m_x1, Side.m_x2);
                        toAdd.Add(m_Grid[i + 1, j]);
                    }
                    if (m_Grid[i - 1, j].m_chosen == null)
                    {
                        SelectTile(m_Grid[i - 1, j], m_Grid[i, j].m_chosen.m_x2, Side.m_x1);
                        toAdd.Add(m_Grid[i - 1, j]);
                    }
                }
                if (j == 0)
                {
                    if (m_Grid[i,j+1].m_chosen == null)
                    {
                        SelectTile(m_Grid[i, j + 1], m_Grid[i, j].m_chosen.m_z2, Side.m_z1);
                        toAdd.Add(m_Grid[i, j+1]);
                    }
                }
                else if (j == width)
                {
                    if (m_Grid[i, j + 1].m_chosen == null)
                    {
                        SelectTile(m_Grid[i, j - 1], m_Grid[i, j].m_chosen.m_z1, Side.m_z2);
                        toAdd.Add(m_Grid[i, j - 1]);
                    }
                }
                else
                {
                    if (m_Grid[i, j + 1].m_chosen == null)
                    {
                        SelectTile(m_Grid[i, j + 1], m_Grid[i, j].m_chosen.m_z2, Side.m_z1);
                        toAdd.Add(m_Grid[i, j - 1]);
                    }
                    if (m_Grid[i, j + 1].m_chosen == null)
                    {
                        SelectTile(m_Grid[i, j - 1], m_Grid[i, j].m_chosen.m_z1, Side.m_z2);
                        toAdd.Add(m_Grid[i, j + 1]);
                    }
                }
            }
            foreach(TileInfo tile in toAdd)
            {
                m_toActivate.Add(tile);
            }
        }
    }
    void SelectTile(TileInfo toSelect,Orientation a, Side opposite)
    {
        Orientation b;
        var availableTiles = toSelect.m_availableTiles;
        List<TileScript> toRemove = new List<TileScript>();
        foreach (TileScript tile in availableTiles)
        {
            switch (opposite)
            {
                case Side.m_x1:
                    b = tile.m_x1;
                    break;
                case Side.m_x2:
                    b = tile.m_x2;
                    break;
                case Side.m_z1:
                    b = tile.m_z1;
                    break;
                case Side.m_z2:
                    b = tile.m_z2;
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
        foreach (TileScript tile in toRemove)
        {
            availableTiles.Remove(tile);
        }
        toSelect.m_chosen = toSelect.m_availableTiles[Random.Range(0, toSelect.m_availableTiles.Count)];
    }
}
