using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        TileMap mapTarget = target as TileMap;

        DrawDefaultInspector();

        if (GUILayout.Button("Re-build"))
        {
            mapTarget.DeleteTiles();
            mapTarget.CreateTiles();
        }
    }
}