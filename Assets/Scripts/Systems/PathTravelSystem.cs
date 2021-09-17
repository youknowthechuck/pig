using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PathTravelSystem : ComponentSystem
{
    List<PathProxyObject> m_paths = new List<PathProxyObject>();

    protected override void OnUpdate()
    {
        //this should only be done on map events (load, path opened?)...
        GatherPaths();

        Entities.WithAll<PathTravelerData>().ForEach((ref Translation translation, ref PathTravelerData traveler) =>
        {
            uint pathId = traveler.m_pathId;
            PathProxyObject path = m_paths.Find(path => path.m_id == pathId);

            //this can be precomputed...
            float travelTimeSeconds = path.m_length / traveler.m_speed;

            float timeStep = Time.DeltaTime / travelTimeSeconds;

            //this can be computed once per segment...
            traveler.m_t += timeStep;

            PathNode startNode = GetSegmentStartNode(traveler.m_t, path.m_nodes.ToArray());

            float localTime = startNode.GetLocalTime(traveler.m_t, path.m_length);
            Vector3 position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);

            if (startNode.m_interpFlags == ENodeInterpolation.interp_cubic)
            {
                uint prevIndex = startNode.m_index == 0 ? 0 : startNode.m_index - 1;
                uint nextIndex = Math.Min(startNode.m_index + 1, (uint)path.m_nodes.Count);
                uint nextNextIndex = Math.Min(nextIndex + 1, nextIndex);

                position = CubicInterpUtils.Eval_Hermite(path.m_nodes[(int)prevIndex].m_transform.position,
                                                                startNode.m_transform.position,
                                                                path.m_nodes[(int)nextIndex].m_transform.position,
                                                                path.m_nodes[(int)nextNextIndex].m_transform.position,
                                                                traveler.m_t,
                                                                0,
                                                                0);
            }
            else if (startNode.m_interpFlags == ENodeInterpolation.interp_linear)
            {
                uint nextIndex = Math.Min(startNode.m_index + 1, (uint)path.m_nodes.Count);
                PathNode nextNode = path.m_nodes[(int)nextIndex];
                Vector3 line = nextNode.m_transform.position - startNode.m_transform.position;
                position = startNode.m_transform.position + line * localTime;
            }

            translation.Value = new float3(position.x, position.y, position.z);
        });
    }

    void GatherPaths()
    {
        m_paths.Clear();

        m_paths = new List<PathProxyObject>(GameObject.FindObjectsOfType<PathProxyObject>());
    }

    PathNode GetSegmentStartNode(float t, PathNode[] nodes)
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
