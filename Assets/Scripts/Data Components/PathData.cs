using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PathData : IComponentData
{
    PathNode[] m_nodes;
}
