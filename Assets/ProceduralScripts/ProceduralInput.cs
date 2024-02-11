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
    public int[,] m_squares;
    public int length;
    public int width;
    private void Start()
    {
        Vector3 mySize = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.size;
        length = (int)mySize.x;
        width =  (int)mySize.z;
        m_squares = new int[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                m_squares[i,j] = 0;
            }
        }
        for(int i = 0; i < 100; i++)
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
