using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TowerBase))]
public class TowerViewer : Editor
{
    bool DebugClosestPoint = true;
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

            if (DebugClosestPoint)
            {
                PathObject[] scenePaths = Object.FindObjectsOfType<PathObject>();
                foreach(PathObject path in scenePaths)
                {
                    Vector3 pathPoint = CubicInterpUtils.Closest_Point(tower.gameObject.transform.position, path);
                    Handles.DrawWireCube(pathPoint, new Vector3(.2f, .2f, .2f));
                }
            }
        }
    }
}
