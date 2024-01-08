using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStruck : PigScript
{
    [SerializeField]
    private float m_duration = 2.0f;

    private float m_lifetime = 0.0f;

    public GameObject Instigator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_lifetime += Time.deltaTime;

        if (m_lifetime >= m_duration)
        {
            Terminate();
        }
    }

    void Terminate()
    {
        //remove this component from gameobject
        Destroy(this);
    }

    public void ResetLifetime()
    {
        m_lifetime = 0.0f;
    }
}
