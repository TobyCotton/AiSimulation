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
        if(DrawDefaultInspector())//When anything changes in inspector do this
        {
            if(gen.m_autoUpdate)
            {
                gen.GenerateMap();
            }
        }

        if(GUILayout.Button("Generate"))// reset heights
        {
            gen.GenerateMap();
        }

        if (GUILayout.Button("Output Map"))
        {
            gen.GenerateMap();
            gen.m_display.OutputMap();
        }
    }
}
