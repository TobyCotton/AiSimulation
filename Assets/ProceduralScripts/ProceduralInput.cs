using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
    private GameObject[] m_Tiles;
    private GameObject[,] m_Grid;
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        length = 9;//(int)mySize.x;
        width = 9;//(int)mySize.z;
        m_squares = new int[length, width];
        m_Tiles = new GameObject[7];
        m_Grid = new GameObject[length, width];
        m_Tiles[0] = m_tile1;
        m_Tiles[1] = m_tile2;
        m_Tiles[2] = m_tile3;
        m_Tiles[3] = m_tile4;
        m_Tiles[4] = m_tile5;
        m_Tiles[5] = m_tile6;
        m_Tiles[6] = m_tile7;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                m_squares[i,j] = 0;
            }
        }
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
        List<GameObject> usableTiles = new List<GameObject>();
        for(int i = 0; i < m_Tiles.Length;i++)
        {
            usableTiles.Add(m_Tiles[i]);
        }
        GameObject[] revertCopy = usableTiles.ToArray();
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!m_Grid[i, j])
                {
                    if (i == 0 && j == 0)
                    {
                        usableTiles.Clear();
                        usableTiles.Add(m_Tiles[3]);
                        GameObject chosen = usableTiles[Random.Range(0, usableTiles.Count)];
                        m_Grid[i, j] = chosen;
                        Instantiate(chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), chosen.transform.rotation);
                        usableTiles = revertCopy.ToList();
                    }
                    else
                    {
                        if (i == 0) { }
                        else
                        {
                            GameObject itemCheck = m_Grid[i - 1, j];
                            TileScript checkScript = itemCheck.GetComponent<TileScript>();
                            if (checkScript != null)
                            {
                                for (int k = 0; k < usableTiles.Count; k++)
                                {
                                    TileScript compareScript = usableTiles[k].GetComponent<TileScript>();
                                    if (compareScript)
                                    {
                                        if (compareScript.m_x2 != checkScript.m_x1)
                                        {
                                            usableTiles.Remove(usableTiles[k]);
                                        }
                                    }
                                }
                            }
                        }
                        if (j == 0) { }
                        else
                        {
                            GameObject itemCheck = m_Grid[i, j - 1];
                            TileScript checkScript = itemCheck.GetComponent<TileScript>();
                            if (checkScript != null)
                            {
                                for (int k = 0; k < usableTiles.Count; k++)
                                {
                                    TileScript compareScript = usableTiles[k].GetComponent<TileScript>();
                                    if (compareScript)
                                    {
                                        if (compareScript.m_z2 != checkScript.m_z1)
                                        {
                                            usableTiles.Remove(usableTiles[k]);
                                        }
                                    }
                                }
                            }
                        }
                        GameObject chosen = usableTiles[Random.Range(0, usableTiles.Count)];
                        chosen.GetComponent<TileScript>().i = i;
                        chosen.GetComponent<TileScript>().j = j;
                        m_Grid[i, j] = chosen;
                        Instantiate(chosen, new Vector3(i + 0.5f, 0.1f, j + 0.5f), chosen.transform.rotation);
                        usableTiles.Clear();
                        
                        for (int o = 0; o < m_Tiles.Length; o++)
                        {
                            usableTiles.Add(m_Tiles[o]);
                        }
                        Debug.Log(usableTiles.Count());
                    }
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
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
