using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    private int[,] m_squares;
    private Transform componentTransform;
    private ProceduralInput Terrain;
    private bool moving = true;
    private int speed = 1;
    void Start()
    {
        Terrain = GameObject.Find("Terrain").GetComponent<ProceduralInput>();
        m_squares = Terrain.m_squares;
        componentTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (moving)
        {
            moveTo(new Vector3(100, 1, 100));
        }
    }

    void moveTo(Vector3 endPositon)
    {
        var direction = (componentTransform.position - endPositon).normalized;
        Vector3 distance = endPositon - componentTransform.position;
        var distanceBetween = distance.magnitude;
        //current distance is less than a very small amount
        if (Vector3.Distance(componentTransform.position, endPositon) > 0.5)
        {
            componentTransform.position = Vector3.MoveTowards(componentTransform.position, endPositon, speed * Time.deltaTime);
        }

    }
    
    void moveOnGrid(Vector3 endPositon)
    {
        //Vector2 GridDistance = WorldPosToSquarePos(endPositon);
        //float SquaresLength = GridDistance.x / Terrain.length;
        //float SquaresWidth = GridDistance.y / Terrain.width;
        //Vector3 MovementVector = new Vector3(GridDistance.x, 0, GridDistance.y);
        //print("MovementVector :" + MovementVector.ToString());
        componentTransform.position = Vector2.MoveTowards(componentTransform.position, endPositon, Time.deltaTime) * Time.deltaTime;
    }

    Vector2 WorldPosToSquarePos(Vector3 worldPos)
    {
        return new Vector2(worldPos.x - 0.5f, worldPos.z - 0.5f);
    }

    Vector3 SquarePosToWorldPos(Vector2 squarePos)
    {
        return new Vector3(squarePos.x + 0.5f, 1, squarePos.y + 0.5f);
    }
}
