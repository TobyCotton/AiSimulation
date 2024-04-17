using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Generator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Generator gen = target as Generator;
        if(DrawDefaultInspector())
        {
            if(gen.m_autoUpdate)
            {
                gen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            gen.GenerateMap();
        }
    }
}
