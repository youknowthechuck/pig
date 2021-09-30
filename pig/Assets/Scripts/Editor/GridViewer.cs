using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridViewer : Editor
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

        GridGenerator[] grids = Object.FindObjectsOfType<GridGenerator>();

        foreach (GridGenerator grid in grids)
        {
            Vector3 cellSize = new Vector3(grid.cellSize, 0.25f, grid.cellSize);
            List <Vector3> gridCenters = grid.GetGridCenters();
            foreach (Vector3 pos in gridCenters)
            {
                Handles.DrawWireCube(grid.transform.position + pos, cellSize);
            }
        }
    }
}
