using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathProxyObject))]
public class PathViewer : Editor
{
    PathProxyObject m_path;
    // Start is called before the first frame update
    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        m_path = (PathProxyObject)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Handles.color = Color.red;
        Vector3 boxSize = new Vector3(.5f, .5f, .5f);
        Handles.DrawWireCube(m_path.m_nodes[0].m_transform.position, boxSize);

        for (int i = 0, j = 1; j < m_path.m_nodes.Count; ++i, ++j)
        {
            int prevIndex = Math.Max(i - 1, 0);
            int nextIndex = Math.Min(j + 1, m_path.m_nodes.Count - 1);

            PathNode first = m_path.m_nodes[i];
            PathNode second = m_path.m_nodes[j];
            PathNode prev = m_path.m_nodes[prevIndex];
            PathNode next = m_path.m_nodes[nextIndex];

            Handles.DrawWireCube(second.m_transform.position, boxSize);

            ENodeInterpolation drawMode = first.m_interpFlags;

            if (drawMode.HasFlag(ENodeInterpolation.interp_cubic))
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
            else if (drawMode.HasFlag(ENodeInterpolation.interp_linear))
            {
                Handles.DrawLine(first.m_transform.position, second.m_transform.position, 4);
            }
        }
    }
}
