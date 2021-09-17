using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PathData : IComponentData
{
    // Can't use dynamic arrays in a POD component
    // You can use fixed arrays in a component marked unsafe? Idk it seems kind of silly.
    //public PathNode[] m_nodes;
    public float m_length;
    public uint m_id;
}
