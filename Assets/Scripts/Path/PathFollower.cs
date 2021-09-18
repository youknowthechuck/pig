using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private float m_t;

    // For debugging
    [SerializeField]
    private float m_speed;     //per second

    // For debugging
    [SerializeField]
    private uint m_pathId;

    public float PathRatio
    {
        get { return m_t; }
    }

    public float Speed
    {
        get { return m_speed; }
    }

    public uint PathId
    {
        get { return m_pathId; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        PathSystemComponent pathSystem = PathSystemComponent.Instance;

        uint pathId = m_pathId;
        PathData path = pathSystem.Paths.Find(path => path.id == pathId);

        //this can be precomputed...
        float travelTimeSeconds = path.length / m_speed;

        float timeStep = Time.deltaTime / travelTimeSeconds;

        //this can be computed once per segment...
        m_t += timeStep;

        PathNode startNode = GetSegmentStartNode(m_t, path.nodes);

        float localTime = startNode.GetLocalTime(m_t, path.length);
        Vector3 position = transform.position;

        if (startNode.m_interpFlags == ENodeInterpolation.interp_cubic)
        {
            uint prevIndex = startNode.m_index == 0 ? 0 : startNode.m_index - 1;
            uint nextIndex = (uint)Mathf.Min((int)startNode.m_index + 1, path.nodes.Count);
            uint nextNextIndex = (uint)Mathf.Min((int)nextIndex + 1, (int)nextIndex);

            position = CubicInterpUtils.Eval_Hermite(path.nodes[(int)prevIndex].m_transform.position,
                                                            startNode.m_transform.position,
                                                            path.nodes[(int)nextIndex].m_transform.position,
                                                            path.nodes[(int)nextNextIndex].m_transform.position,
                                                            m_t,
                                                            0,
                                                            0);
        }
        else if (startNode.m_interpFlags == ENodeInterpolation.interp_linear)
        {
            uint nextIndex = (uint)Mathf.Min((int)startNode.m_index + 1, path.nodes.Count);
            PathNode nextNode = path.nodes[(int)nextIndex];
            Vector3 line = nextNode.m_transform.position - startNode.m_transform.position;
            position = startNode.m_transform.position + line * localTime;
        }

        transform.position = position;
    }

    PathNode GetSegmentStartNode(float t, List<PathNode> nodes)
    {
        PathNode start = nodes[0];
        //walk nodes until we go past t, then return the previous
        foreach (PathNode node in nodes)
        {
            if (node.m_start1D > t)
            {
                break;
            }
            else
            {
                start = node;
            }
        }
        return start;
    }
}
