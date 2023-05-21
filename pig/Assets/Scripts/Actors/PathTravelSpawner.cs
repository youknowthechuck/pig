using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTravelSpawner : MonoBehaviour
{
    [SerializeField]
    private PathObject m_travelPath = null;

    public PathObject TravelPath
    {
        get { return m_travelPath; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnObject(PathFollower prefab)
    {
        
        PathFollower spawned = Object.Instantiate<PathFollower>(prefab, m_travelPath.Nodes[0].m_transform.position, m_travelPath.Nodes[0].m_transform.rotation, transform);
        if (spawned != null)
        {
            spawned.Path = m_travelPath;
        }
    }
}
