#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneGridCulling))]
public class SceneGridCullingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneGridCulling myScript = (SceneGridCulling)target;
        if (GUILayout.Button("Build Scene Grid"))
        {
            myScript.buildGrid();
        }
    }
}

#endif