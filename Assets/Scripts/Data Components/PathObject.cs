using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathObject : MonoBehaviour
{
    public PathData pathData = new PathData();

    void Start()
    {
        UpdateNodeListFromChildren();
        CalulateTotalLength();
    }

    void Update()
    {
        if (Application.isEditor)
        {
            UpdateNodeListFromChildren();
            CalulateTotalLength();
        }
    }

    void UpdateNodeListFromChildren()
    {
        foreach (Transform child in transform)
        {
            if (!pathData.nodes.Exists(node => node.m_transform == child))
            {
                PathNode newNode = new PathNode();
                newNode.m_transform = child;
                newNode.m_interpFlags |= ENodeInterpolation.interp_cubic;
                pathData.nodes.Add(newNode);
            }
        }
    }

    void CalulateTotalLength()
    {
        pathData.length = 0.0f;
        for (int i = 0, j = 1; j < pathData.nodes.Count; ++i, ++j)
        {
            int prevIndex = Math.Max(i - 1, 0);
            int nextIndex = Math.Min(j + 1, pathData.nodes.Count - 1);

            PathNode first = pathData.nodes[i];
            PathNode second = pathData.nodes[j];
            PathNode prev = pathData.nodes[prevIndex];
            PathNode next = pathData.nodes[nextIndex];

            if (first.m_interpFlags == ENodeInterpolation.interp_cubic)
            {
                const float dT = 1.0f / 100.0f;

                float t = dT;
                Vector3 start = first.m_transform.position;

                //float precision lul
                while (t <= 1.001f)
                {
                    Vector3 end = CubicInterpUtils.Eval_Hermite(prev.m_transform.position, first.m_transform.position, second.m_transform.position, next.m_transform.position, t, 0, 0);
                    Vector3 segment = (end - start);
                    first.m_length = segment.magnitude;
                    first.m_start1D = pathData.length;
                    pathData.length += first.m_length;
                    start = end;
                    t += dT;
                }

            }
            else if (first.m_interpFlags == ENodeInterpolation.interp_linear)
            {
                Vector3 segment = (second.m_transform.position - first.m_transform.position);
                first.m_length = segment.magnitude;
                first.m_start1D = pathData.length;
                pathData.length += first.m_length;
            }
        }
    }
}
