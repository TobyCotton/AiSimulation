using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralInput : MonoBehaviour
{
    public GameObject m_house1;
    public GameObject m_house2;
    public GameObject m_house3;
    //private void Start()
    //{
    //    Instantiate(m_house1);
    //    Instantiate(m_house2);
    //    Instantiate(m_house3);
    //}
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            int choice = Random.Range(1, 4);
            if (choice == 1)
            {
                var housePosition = new Vector3(Random.Range(5, 95), 5.0f, Random.Range(5, 95));
                Instantiate(m_house1, housePosition, transform.rotation);
            }
            if (choice == 2)
            {
                var housePosition = new Vector3(Random.Range(5, 95), 5.0f, Random.Range(5, 95));
                Instantiate(m_house2, housePosition, transform.rotation);
            }
            if (choice == 3)
            {
                var housePosition = new Vector3(Random.Range(5, 95), 10.0f, Random.Range(5, 95));
                Instantiate(m_house3, housePosition, transform.rotation);
            }
        }
    }
}
