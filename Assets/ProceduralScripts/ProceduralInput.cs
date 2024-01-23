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
    private int[,] m_squares;
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        int length = (int) mySize.x;
        int width =  (int)mySize.z;
        Debug.Log(length.ToString());
        Debug.Log(width.ToString());
        m_squares = new int[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                m_squares[i,j] = 0;
            }
        }
    }

    bool ValidPosition(int x,int z,Vector3 size)
    {
        int UseX = (int)size.x / 2;
        int UseZ = (int)size.z / 2;
        if(x+UseX > 99 || x-UseX < 0 || z+UseZ > 99 || z-UseZ < 0)
        {
            return false;
        }
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
            int choice = Random.Range(1, 4);
            if (choice == 1)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                Vector3 size = m_house1.transform.localScale;
                if (ValidPosition(x,z,size))
                {
                    var housePosition = new Vector3(x, 5.0f, z);
                    Instantiate(m_house1, housePosition, transform.rotation);
                }
            }
            if (choice == 2)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                Vector3 size = m_house2.transform.localScale;
                if (ValidPosition(x, z, size))
                {
                    var housePosition = new Vector3(x, 5.0f, z);
                    Instantiate(m_house2, housePosition, transform.rotation);
                }
            }
            if (choice == 3)
            {
                int x = Random.Range(5, 95);
                int z = Random.Range(5, 95);
                Vector3 size = m_house3.transform.localScale;
                if (ValidPosition(x, z, size))
                {
                    var housePosition = new Vector3(x, 10.0f, z);
                    Instantiate(m_house3, housePosition, transform.rotation);
                }
            }
        }
    }
}
