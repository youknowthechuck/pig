using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private float m_t;

    [SerializeField]
    private float m_speed = 1.0f;     //per second

    [SerializeField]
    private PathObject m_path;

    public float PathRatio
    {
        get { return m_t; }
    }

    public float Speed
    {
        get { return m_speed; }
    }

    public PathObject Path
    {
        get { return m_path; }
        set { m_path = value; }
    }

    void Start()
    {
        m_t = 0.0f;
    }

    void Update()
    {
        //this can be precomputed...
        float travelTimeSeconds = m_path.Length / m_speed;

        float timeStep = Time.deltaTime / travelTimeSeconds;

        //this can be computed once per segment...
        m_t += timeStep;

        int startNodeIndex = GetSegmentStartNodeIndex(m_t, m_path.Nodes);

        PathNode startNode = Path.Nodes[startNodeIndex];

        float localTime = startNode.GetLocalTime(m_t, m_path.Length);
        Vector3 position = transform.position;

        if (startNode.m_interpFlags == ENodeInterpolation.interp_cubic)
        {
            //this shit sucks move the next node/previous node logic into the path
            int prevIndex = Math.Max(startNodeIndex - 1, 0);
            int nextIndex = Math.Min(startNodeIndex + 1, Path.Nodes.Count - 1);
            int nextNextIndex = Math.Min(nextIndex + 1, Path.Nodes.Count - 1);

            position = CubicInterpUtils.Eval_Hermite(m_path.Nodes[prevIndex].m_transform.position,
                                                            startNode.m_transform.position,
                                                            m_path.Nodes[nextIndex].m_transform.position,
                                                            m_path.Nodes[nextNextIndex].m_transform.position,
                                                            localTime,
                                                            0,
                                                            0);
        }
        else if (startNode.m_interpFlags == ENodeInterpolation.interp_linear)
        {
            int nextIndex = Math.Min(startNodeIndex + 1, Path.Nodes.Count - 1);
            PathNode nextNode = m_path.Nodes[nextIndex];
            Vector3 line = nextNode.m_transform.position - startNode.m_transform.position;
            position = startNode.m_transform.position + line * localTime;
        }

        transform.position = position;
    }

    int GetSegmentStartNodeIndex(float t, List<PathNode> nodes)
    {
        int i = 0;
        for (; i < nodes.Count; ++i)
        {
            if (nodes[i].m_start1D > t)
            {
                break;
            }
        }
        return i-1;
    }
}
