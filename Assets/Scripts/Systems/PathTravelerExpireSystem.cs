using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PathTravelerExpireSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<PathTravelerData>().ForEach((Entity entity, ref PathTravelerData traveler) =>
        {
            if (traveler.m_t >= 1.0f)
            {
                EntityManager.DestroyEntity(entity);
            }
        });
    }
}
