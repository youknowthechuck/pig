using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : PigScript
{
    private struct SpawnInterval
    {
        public SpawnInterval(PathTravelSpawner spawnPoint, PathFollower prefab, float t)
        {
            spawner = spawnPoint;
            spawnObject = prefab;
            spawnTime = t;
        }

        public PathTravelSpawner spawner;
        public PathFollower spawnObject;
        public float spawnTime;
    }

    private List<SpawnInterval> m_pendingSpawns = new List<SpawnInterval>();

    private float m_timer;

    private bool m_ready = false;

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
        m_ready = false;
        m_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ready)
        {
            m_pendingSpawns?.RemoveAll(interval =>
                CheckInterval(interval, Time.deltaTime) == true
            );
            m_timer += Time.deltaTime;
        }
    }

    public void Begin()
    {
        m_timer = 0.0f;

        m_pendingSpawns.Clear();

        foreach (SpawnParams spawnParams in m_spawns)
        {
            foreach (WaveDefinition wave in spawnParams.Waves)
            {
                for (int i = 0; i < wave.m_spawnCount; ++i)
                {
                    SpawnInterval interval = new SpawnInterval(
                        spawnParams.Spawner,
                        wave.m_travelerPrefab,
                        wave.m_initialSpawnDelay + wave.m_spawnInterval * i);
                    m_pendingSpawns.Add(interval);
                }
            }
        }

        m_ready = true;
    }

    bool CheckInterval(SpawnInterval spawnInterval, float dT)
    {
        bool shouldActivate = spawnInterval.spawnTime >= m_timer && spawnInterval.spawnTime <= m_timer + dT;

        if (shouldActivate)
        {
            spawnInterval.spawner.SpawnObject(spawnInterval.spawnObject);
        }

        return shouldActivate;
    }

    public bool IsComplete()
    {
        return m_ready && m_pendingSpawns.Count == 0;
    }
}
