using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathObject))]
public class PathViewer : Editor
{
    PathObject m_path;
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        m_path = (PathObject)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.color = Color.red;
        Vector3 boxSize = new Vector3(.5f, .5f, .5f);
        Handles.DrawWireCube(m_path.Nodes[0].m_transform.position, boxSize);

        for (int i = 0, j = 1; j < m_path.Nodes.Count; ++i, ++j)
        {
            int prevIndex = Math.Max(i - 1, 0);
            int nextIndex = Math.Min(j + 1, m_path.Nodes.Count - 1);

            PathNode first = m_path.Nodes[i];
            PathNode second = m_path.Nodes[j];
            PathNode prev = m_path.Nodes[prevIndex];
            PathNode next = m_path.Nodes[nextIndex];

            Handles.DrawWireCube(second.m_transform.position, boxSize);

            if (first.m_interpFlags == ENodeInterpolation.interp_cubic)
            {
                const float dT = 1.0f / 20.0f;

                float t = dT;
                Vector3 start = first.m_transform.position;

                //float precision lul
                while(t <= 1.001f)
                {
                    Vector3 end = CubicInterpUtils.Eval_Hermite(prev.m_transform.position, first.m_transform.position, second.m_transform.position, next.m_transform.position, t, 0, 0);
                    Handles.DrawLine(start, end, 4);
                    start = end;
                    t += dT;
                }
            }
            else if (first.m_interpFlags == ENodeInterpolation.interp_linear)
            {
                Handles.DrawLine(first.m_transform.position, second.m_transform.position, 4);
            }
        }
    }
}
