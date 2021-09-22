using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public int m_baseHP;

    [HideInInspector]
    public int m_currentHP;

    // Start is called before the first frame update
    void Start()
    {
        m_currentHP = m_baseHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
