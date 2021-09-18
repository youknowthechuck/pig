using UnityEngine;
using System.Collections.Generic;

public class PathSystemComponent : SingletonBehaviour<PathSystemComponent>
{
    private List<PathData> m_paths = new List<PathData>();
    private bool m_initialized;

    public List<PathData> Paths
    {
        get 
        { 
            if(!m_initialized)
            {
                GetPaths();
                m_initialized = true;
            }

            return m_paths;
        }
    }

    private void GetPaths()
    {
        m_paths.Clear();
        foreach(PathObject proxy in GameObject.FindObjectsOfType<PathObject>())
        {
            m_paths.Add(proxy.pathData);
        }
    }
}

