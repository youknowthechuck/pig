using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveDefinition
{
    public PathFollower m_travelerPrefab;

    public float m_initialSpawnDelay;

    public float m_spawnInterval;

    public int m_spawnCount;
}
