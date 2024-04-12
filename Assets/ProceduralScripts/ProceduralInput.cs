using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
    public GameObject m_house4;
    public GameObject m_house5;
    public GameObject m_house6;
    public GameObject m_house7;
    public GameObject m_tile1;
    public GameObject m_tile2;
    public GameObject m_tile3;
    public GameObject m_tile4;
    public GameObject m_tile5;
    public GameObject m_tile6;
    public GameObject m_tile7;
    public GameObject m_tile8;
    public Grid grid;
    public int length;
    public int width;
    public Sprite sprite;
    public bool VisualiseGrid;
    private TileInfo[,] m_Grid;
    private List<TileInfo> m_toActivate = new List<TileInfo>();
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        length = (int)mySize.x;
        width =  (int)mySize.z;
        grid = new Grid(length,width);
        grid.sprite = this.sprite;
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
                m_Grid[o, p] = new TileInfo(m_tile1, m_tile2, m_tile3, m_tile4, m_tile5, m_tile6, m_tile7,m_tile8, o, p);
            }
        }
        int l = length / 2;
        int d = width / 2;
        m_Grid[l, d].i = l;
        m_Grid[l, d].j = d;
        m_Grid[l, d].m_chosen = m_Grid[l, d].m_availableTiles[Random.Range(0, m_Grid[l, d].m_availableTiles.Count)];
        m_toActivate.Add(m_Grid[l, d]);
        CreateRoads();
        int amountOfEach = 10;
        while (amountOfEach >0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house1.transform.rotation = transform.rotation;
            m_house1.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house1.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house1, housePosition, m_house1.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house2.transform.rotation = transform.rotation;
            m_house2.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house2.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house2, housePosition, m_house2.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house3.transform.rotation = transform.rotation;
            m_house3.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house3.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house3, housePosition, m_house3.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house4.transform.rotation = transform.rotation;
            m_house4.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house4.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house4, housePosition, m_house4.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house5.transform.rotation = transform.rotation;
            m_house5.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house5.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house5, housePosition, m_house5.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house6.transform.rotation = transform.rotation;
            m_house6.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house6.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house6, housePosition, m_house6.transform.rotation);
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        amountOfEach = 10;
        while (amountOfEach > 0)
        {
            int x = Random.Range(5, 95);
            int z = Random.Range(5, 95);
            int rotationAmount = Random.Range(0, 4) * 90;
            m_house7.transform.rotation = transform.rotation;
            m_house7.transform.Rotate(0, rotationAmount, 0, Space.Self);
            Vector3 size = m_house7.transform.localScale;
            if (ValidPosition(x, z, size, rotationAmount))
            {
                amountOfEach--;
                var housePosition = new Vector3(x, 2.0f, z);
                GameObject house = Instantiate(m_house7, housePosition, m_house7.transform.rotation);
                
                var entrance = house.transform.Find("Entrance");
                if (entrance != null)
                {
                    GridTile tile = grid.TileFromWorldPoint(entrance.transform.position);
                    tile.isWalkable = true;
                    tile.isEntrance = true;
                    PostAvailable(rotationAmount, size, tile.gridPos.x, tile.gridPos.y);
                }
            }
        }
        foreach(GridTile tile in grid.gridArray)
        {
            if(tile.availablePost)
            {
                tile.isWalkable = true;
            }
        }

        if (VisualiseGrid)
        {
            grid.RenderTiles();
        }
    }
    void PostAvailable(int rotation, Vector3 size,float fi, float fj)
    {
        int i = (int)fi;
        int j = (int)fj;
        int UseX = (int)size.x / 2;
        int UseZ = (int)size.z / 2;
        if (rotation == 0 || rotation == 180)
        {
            int temp = UseZ;
            UseZ = UseX;
            UseX = temp;
        }

        if (rotation == 90)
        {
            int TempX = UseX+1;
            for (int u = 0; u < TempX; u++)
            {
                grid.gridArray[i, j - u].availablePost = true;
            }
            TempX = UseX;
            for (int u = 0; u < TempX; u++)
            {
                grid.gridArray[i, j + u].availablePost = true;
            }
        }
        else if(rotation == 270)
        {
            int TempX = UseX;
            for (int u = 0; u < TempX; u++)
            {
                grid.gridArray[i, j - u].availablePost = true;
            }
            TempX = UseX+1;
            for (int u = 0; u < TempX; u++)
            {
                grid.gridArray[i, j + u].availablePost = true;
            }
        }
        else if (rotation == 0)
        {
            int TempZ = UseZ - 1;
            for (int u = 1; u < TempZ + 1; u++)
            {
                grid.gridArray[i - u, j].availablePost = true;
            }
            TempZ = UseZ;
            for (int u = 1; u < TempZ + 1; u++)
            {
                grid.gridArray[i + u, j].availablePost = true;
            }
        }
        else
        {
            int TempZ = UseZ;
            for (int u = 1; u < TempZ + 1; u++)
            {
                grid.gridArray[i - u, j].availablePost = true;
            }
            TempZ = UseZ - 1;
            for (int u = 1; u < TempZ + 1; u++)
            {
                grid.gridArray[i + u, j].availablePost = true;
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
                if (grid.gridArray[x + i, z + j].isWalkable != true || !grid.gridArray[x + i, z + j].isGrass)
                {
                    return false;
                }
            }
        }
        for (int i = -UseX; i < UseX; i++)
        {
            for (int j = -UseZ; j < UseZ; j++)
            {
                if (grid.gridArray[x + i, z + j].isEntrance == false)
                {
                    grid.gridArray[x + i, z + j].isWalkable = false;
                }
                else
                {
                    //if(upDown)
                    //{
                    //    if(rotation == 90)
                    //    {
                    //        int TempX = UseX-1;
                    //        for(int u = 0; u < TempX; u++)
                    //        {
                    //            grid.gridArray[x + i - u, z + j].availablePost = true;
                    //        }
                    //        TempX = UseX;
                    //        for(int u = 0; u < TempX; u++)
                    //        {
                    //            grid.gridArray[x + i + u, z + j].availablePost = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int TempX = UseX;
                    //        for (int u = 0; u < TempX; u++)
                    //        {
                    //            grid.gridArray[x + i - u, z + j].availablePost = true;
                    //        }
                    //        TempX = UseX-1;
                    //        for (int u = 0; u < TempX; u++)
                    //        {
                    //            grid.gridArray[x + i + u, z + j].availablePost = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (rotation == 0)
                    //    {
                    //        int TempZ = UseZ - 1;
                    //        for (int u = 1; u < TempZ + 1; u++)
                    //        {
                    //            grid.gridArray[x + i, z + j - u].availablePost = true;
                    //        }
                    //        TempZ = UseZ;
                    //        for (int u = 1; u < TempZ + 1; u++)
                    //        {
                    //            grid.gridArray[x + i, z + j + u].availablePost = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int TempZ = UseZ;
                    //        for (int u = 1; u < TempZ + 1; u++)
                    //        {
                    //            grid.gridArray[x + i, z + j - u].availablePost = true;
                    //        }
                    //        TempZ = UseZ-1;
                    //        for (int u = 1; u < TempZ + 1; u++)
                    //        {
                    //            grid.gridArray[x + i, z + j + u].availablePost = true;
                    //        }
                    //    }
                    //}
                }
            }
        }
        return true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
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
    void CreateRoads()
    {
        List<TileInfo> toAdd = new List<TileInfo>();
        while (m_toActivate.Count != 0)
        {
            int i = m_toActivate[0].i;
            int j = m_toActivate[0].j;
            if (!m_toActivate[0].m_enabled)
            {
                List<GameObject> toRemove = new List<GameObject>();
                for (int l = 0; l < m_toActivate[0].m_priority.Count; l++)
                {
                    if (!m_toActivate[0].m_availableTiles.Contains(m_toActivate[0].m_priority[l]))
                    {
                        toRemove.Add(m_toActivate[0].m_priority[l]);
                    }
                }
                foreach (GameObject tile in toRemove)
                {
                    m_toActivate[0].m_priority.Remove(tile);
                }
                if (m_toActivate[0].m_priority.Count > 0)
                {
                    m_toActivate[0].m_chosen = m_toActivate[0].m_priority[Random.Range(0, m_toActivate[0].m_priority.Count)];
                }
                else
                {
                    m_toActivate[0].m_chosen = m_toActivate[0].m_availableTiles[Random.Range(0, m_toActivate[0].m_availableTiles.Count)];
                }
                m_toActivate[0].m_enabled = true;
                if (m_toActivate[0].m_chosen == m_tile5)
                {
                    grid.gridArray[i, j].SetIsGrass();
                }
                else if (m_toActivate[0].m_chosen == m_tile6 && !m_toActivate[0].m_priority.Contains(m_tile6))//only access if it isn't already a priority
                {
                    if(i < length/2)
                    {
                        for (int l = 1; l < 5; l++)//Straights 5 long
                        {
                            if(i-l > 0)
                            {
                                m_Grid[i - l, j].m_priority.Add(m_tile6);
                            }
                        }
                        if (i - 5 > 0)
                        {
                            m_Grid[i - 5, j].m_priority.Add(m_tile8);//Cross road after straights
                        }
                        if (i - 6 > 0)
                        {
                            if (m_Grid[i-6,j].m_availableTiles.Contains(m_tile8))//dont have 2 Crossroads in a row
                            {
                                m_Grid[i - 6, j].m_availableTiles.Remove(m_tile8);
                            }
                        }
                    }
                    else
                    {
                        for (int l = 1; l < 5; l++)//Straights 5 long
                        {
                            if (i + l < length)
                            {
                                m_Grid[i + l, j].m_priority.Add(m_tile6);
                            }
                        }
                        if (i + 5 < length)
                        {
                            m_Grid[i + 5, j].m_priority.Add(m_tile8);//Cross road after straights
                        }
                        if (i + 6 < length)
                        {
                            if (m_Grid[i + 6, j].m_availableTiles.Contains(m_tile8))//dont have 2 Crossroads in a row
                            {
                                m_Grid[i + 6, j].m_availableTiles.Remove(m_tile8);
                            }
                        }
                    }
                }
                else if (m_toActivate[0].m_chosen == m_tile7 && !m_toActivate[0].m_priority.Contains(m_tile7))//only access if it isn't already a priority
                {
                    if(j < width/2)
                    {
                        for(int l =1; l < 5; l++)//Straights 5 long
                        {
                            if (j-l > 0)
                            {
                                m_Grid[i, j - l].m_priority.Add(m_tile7);
                            } 
                        }
                        if (j - 5 > 0)
                        {
                            m_Grid[i, j-5].m_priority.Add(m_tile8);//Cross road after straights
                        }
                        if (j - 6 > 0)
                        {
                            if (m_Grid[i, j-6].m_availableTiles.Contains(m_tile8))//dont have 2 Crossroads in a row
                            {
                                m_Grid[i, j-6].m_availableTiles.Remove(m_tile8);
                            }
                        }
                    }
                    else
                    {
                        for (int l = 1; l < 5; l++)//Straights 5 long
                        {
                            if (j + l < width)
                            {
                                m_Grid[i, j + l].m_priority.Add(m_tile7);
                            }
                        }
                        if (j + 5 < width)
                        {
                            m_Grid[i, j+5].m_priority.Add(m_tile8);//Cross road after straights
                        }
                        if (j + 6 < width)
                        {
                            if (m_Grid[i, j + 6].m_availableTiles.Contains(m_tile8))//dont have 2 Crossroads in a row
                            {
                                m_Grid[i, j + 6].m_availableTiles.Remove(m_tile8);
                            }
                        }
                    }
                }
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
