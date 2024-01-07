using UnityEngine;

//interp types are bit flags because a point on a path can have multiple interp types.
//we could be at the end of a linear section and the start of a spline, eg.
public enum ENodeInterpolation
{
    interp_linear,
    interp_cubic,
    interp_terminator
}

//a path node defines the characteristics of the path that comes after this node and before the next
[System.Serializable]
public class PathNode
{
    public Transform m_transform;

    public int m_index;

    public ENodeInterpolation m_interpFlags;

    public float m_width = 100.0f;

    [HideInInspector]
    public float m_length;
    [HideInInspector]
    public float m_start1D;

    public float GetLocalTime(float t, float totalLength )
    {
        float segmentSpan = m_length / totalLength;

        return (t - m_start1D) / (segmentSpan);
    }
}
