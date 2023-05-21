using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : PigScript
{
    private float m_t;

    [SerializeField]
    private float m_speed = 1.0f;     //per second

    [SerializeField]
    private PathObject m_path;

    public float LifeTime
    {
        get { return m_t; }
    }

    //[0,1] representing percentage of path traveled
    public float LifeTimeNormalized
    {
        get { return PathTime(m_t); }
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
        m_t += Time.deltaTime;

        transform.position = EvalPosition(m_t);
        transform.forward = EvalForward(m_t);

        //@todo: move destroy logic somewhere to do more heavy lifting when there is HP or whatever
        if (PathTime(m_t) >= 1.0f)
        {
            FinishTrip();
        }
    }

    float PathTime(float worldTime)
    {
        //this can be precomputed...
        float travelTimeSeconds = m_path.Length / m_speed;

        float life = worldTime / travelTimeSeconds;

        return life;
    }

    //exposed for prediction
    public Vector3 EvalPosition(float t)
    {
        float pathTime = PathTime(t);

        int startNodeIndex = GetSegmentStartNodeIndex(pathTime, m_path.Nodes);

        PathNode startNode = Path.Nodes[startNodeIndex];

        float segmentTime = startNode.GetLocalTime(pathTime, m_path.Length);

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
                                                            segmentTime,
                                                            0,
                                                            0);
        }
        else if (startNode.m_interpFlags == ENodeInterpolation.interp_linear)
        {
            int nextIndex = Math.Min(startNodeIndex + 1, Path.Nodes.Count - 1);
            PathNode nextNode = m_path.Nodes[nextIndex];
            Vector3 line = nextNode.m_transform.position - startNode.m_transform.position;
            position = startNode.m_transform.position + line * segmentTime;
        }

        return position;
    }

    public Vector3 EvalForward(float t)
    {
        float pathTime = PathTime(t);

        int startNodeIndex = GetSegmentStartNodeIndex(pathTime, m_path.Nodes);

        PathNode startNode = Path.Nodes[startNodeIndex];

        float segmentTime = startNode.GetLocalTime(pathTime, m_path.Length);

        Vector3 forward = transform.forward;

        if (startNode.m_interpFlags == ENodeInterpolation.interp_cubic)
        {
            //this shit sucks move the next node/previous node logic into the path
            int prevIndex = Math.Max(startNodeIndex - 1, 0);
            int nextIndex = Math.Min(startNodeIndex + 1, Path.Nodes.Count - 1);
            int nextNextIndex = Math.Min(nextIndex + 1, Path.Nodes.Count - 1);

            forward = CubicInterpUtils.Eval_Tangent_Hermite(m_path.Nodes[prevIndex].m_transform.position,
                                                            startNode.m_transform.position,
                                                            m_path.Nodes[nextIndex].m_transform.position,
                                                            m_path.Nodes[nextNextIndex].m_transform.position,
                                                            segmentTime,
                                                            0,
                                                            0);
        }
        else if (startNode.m_interpFlags == ENodeInterpolation.interp_linear)
        {
            int nextIndex = Math.Min(startNodeIndex + 1, Path.Nodes.Count - 1);
            PathNode nextNode = m_path.Nodes[nextIndex];
            Vector3 line = nextNode.m_transform.position - startNode.m_transform.position;

            forward = line.normalized;
        }

        return forward;
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

    void FinishTrip()
    {
        DamagerBehavior endDamager = GetComponent<DamagerBehavior>();

        if (endDamager != null)
        {
            Player player = UnityEngine.Object.FindObjectOfType(typeof(Player)) as Player;

            endDamager.TryDoDamage(player.gameObject);
        }

        Destroy(gameObject);
    }
}
