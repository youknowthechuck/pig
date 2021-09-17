using Unity.Entities;

public struct PathTravelerData : IComponentData
{
    public float m_t;
    //per second
    public float m_speed;

    public uint m_pathId;
}
