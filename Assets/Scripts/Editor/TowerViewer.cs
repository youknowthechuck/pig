using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerBase))]
public class TowerViewer : Editor
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.color = Color.yellow;

        TowerBase[] sceneTowers = Object.FindObjectsOfType<TowerBase>();

        foreach (TowerBase tower in sceneTowers)
        {
            Handles.DrawWireDisc(tower.transform.position, Vector3.up, tower.Range, 2);
        }
    }
}
