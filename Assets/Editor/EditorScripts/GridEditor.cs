
using Map;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridMap))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var gridMap = (GridMap)target;

        gridMap.StartPosition = EditorGUILayout.Vector3Field("Start Position", gridMap.StartPosition);
        gridMap.EndPosition = EditorGUILayout.Vector3Field("End Position", gridMap.EndPosition);

        if (GUILayout.Button("Start Pathfinding"))
        {
            gridMap.StartPathfinding();
        }
    }
}
