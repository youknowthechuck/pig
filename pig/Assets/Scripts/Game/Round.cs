using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : PigScript
{
    [Serializable]
    private struct SpawnParams
    {
        public PathTravelSpawner Spawner;
        public List<WaveDefinition> Waves;
    }

    [SerializeField]
    private List<SpawnParams> m_spawns = null;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Begin()
    {
        foreach (SpawnParams spawnParams in m_spawns)
        {
            spawnParams.Spawner.Init(spawnParams.Waves);
        }
    }

    public bool IsComplete()
    {
        foreach (SpawnParams spawnParams in m_spawns)
        {
            if (spawnParams.Spawner.HasPendingSpawns())
            {
                return false;
            }
        }
        return true;
    }
}
