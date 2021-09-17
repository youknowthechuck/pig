using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathProxyObject : MonoBehaviour
{
    public List<PathNode> m_nodes;
    public uint m_id = 0;
    public float m_length = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateNodeListFromChildren();
        CalulateTotalLength();
    }

    // Update is called once per frame
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
            if (!m_nodes.Exists(node => node.m_transform == child))
            {
                PathNode newNode = new PathNode();
                newNode.m_transform = child;
                newNode.m_interpFlags |= ENodeInterpolation.interp_cubic;
                m_nodes.Add(newNode);
            }
        }
    }

    void CalulateTotalLength()
    {
        m_length = 0.0f;
        for (int i = 0, j = 1; j < m_nodes.Count; ++i, ++j)
        {
            int prevIndex = Math.Max(i - 1, 0);
            int nextIndex = Math.Min(j + 1, m_nodes.Count - 1);

            PathNode first = m_nodes[i];
            PathNode second = m_nodes[j];
            PathNode prev = m_nodes[prevIndex];
            PathNode next = m_nodes[nextIndex];

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
                    first.m_start1D = m_length;
                    m_length += first.m_length;
                    start = end;
                    t += dT;
                }

            }
            else if (first.m_interpFlags == ENodeInterpolation.interp_linear)
            {
                Vector3 segment = (second.m_transform.position - first.m_transform.position);
                first.m_length = segment.magnitude;
                first.m_start1D = m_length;
                m_length += first.m_length;
            }
        }
    }
}
