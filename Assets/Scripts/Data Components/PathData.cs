using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PathData : IComponentData
{
    public PathNode[] m_nodes;
    public float m_length;
    public uint m_id;
}
