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
        Vector3 boxSize = new Vector3(5f, 5f, 5f);
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

                    Vector3 tan = CubicInterpUtils.Eval_Tangent_Hermite(prev.m_transform.position, first.m_transform.position, second.m_transform.position, next.m_transform.position, t, 0, 0).normalized;

                    Vector3 side = Vector3.Cross(tan, Vector3.up).normalized;
                    
                    float halfW = first.m_width / 2;

                    Vector3 p0 = start - side * halfW;

                    Vector3 p1 = start + side * halfW;

                    Vector3 p2 = end - side * halfW;

                    Vector3 p3 = end + side * halfW;

                    Handles.color = Color.blue;
                    Handles.DrawWireCube(p0, new Vector3(1.0f, 1.0f, 1.0f));
                    Handles.color = Color.yellow;
                    Handles.DrawWireCube(p1, new Vector3(1.0f, 1.0f, 1.0f)); 
                    Handles.color = Color.cyan;
                    Handles.DrawWireCube(p2, new Vector3(1.0f, 1.0f, 1.0f));
                    Handles.color = Color.white;
                    Handles.DrawWireCube(p3, new Vector3(1.0f, 1.0f, 1.0f));


                    Handles.color = Color.red;
                    Handles.DrawLine(start, end, 4);
                    start = end;
                    t += dT;
                }
            }
            else if (first.m_interpFlags == ENodeInterpolation.interp_linear)
            {
                Vector3 side = Vector3.Cross(second.m_transform.position - first.m_transform.position, Vector3.up).normalized;

                float halfW = first.m_width / 2;

                Vector3 p0 = first.m_transform.position - side * halfW;

                Vector3 p1 = first.m_transform.position + side * halfW;

                Vector3 p2 = second.m_transform.position - side * halfW;

                Vector3 p3 = second.m_transform.position + side * halfW;

                Handles.color = Color.blue;
                Handles.DrawWireCube(p0, new Vector3(1.0f, 1.0f, 1.0f));
                Handles.color = Color.yellow;
                Handles.DrawWireCube(p1, new Vector3(1.0f, 1.0f, 1.0f));
                Handles.color = Color.cyan;
                Handles.DrawWireCube(p2, new Vector3(1.0f, 1.0f, 1.0f));
                Handles.color = Color.white;
                Handles.DrawWireCube(p3, new Vector3(1.0f, 1.0f, 1.0f));

                Handles.color = Color.red;

                Handles.DrawLine(first.m_transform.position, second.m_transform.position, 4);
            }
        }
    }
}
