using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTravelSpawner : MonoBehaviour
{
    private struct SpawnInterval
    {
        public SpawnInterval(PathFollower prefab, float t)
        {
            spawnObject = prefab;
            spawnTime = t;
        }

        public PathFollower spawnObject;
        public float spawnTime;
    }

    private List<SpawnInterval> m_pendingSpawns = new List<SpawnInterval>();

    [SerializeField]
    private PathObject m_travelPath = null;

    private List<WaveDefinition> m_spawnWaves = null;

    private float m_timer;

    private bool m_ready = false;

    public PathObject TravelPath
    {
        get { return m_travelPath; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ready = false;
    }

    public void Init(List<WaveDefinition> waves)
    {
        m_timer = 0.0f;

        m_spawnWaves = waves;

        foreach (WaveDefinition wave in m_spawnWaves)
        {
            for (int i = 0; i < wave.m_spawnCount; ++i)
            {
                SpawnInterval interval = new SpawnInterval(
                    wave.m_travelerPrefab,
                    wave.m_initialSpawnDelay + wave.m_spawnInterval * i);
                m_pendingSpawns.Add(interval);
            }
        }

        m_ready = true;
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

    bool CheckInterval(SpawnInterval spawnInterval, float dT)
    {
        bool shouldActivate = spawnInterval.spawnTime >= m_timer && spawnInterval.spawnTime <= m_timer + dT;

        if (shouldActivate)
        {
            PathFollower spawned = Object.Instantiate<PathFollower>(spawnInterval.spawnObject, transform);
            if (spawned != null)
            {
                spawned.Path = m_travelPath;
            }
        }

        return shouldActivate;
    }

    public bool HasPendingSpawns()
    {
        return m_pendingSpawns.Count > 0;
    }
}
