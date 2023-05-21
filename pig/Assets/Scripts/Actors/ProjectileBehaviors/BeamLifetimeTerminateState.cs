using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamLifetimeTerminateState : ProjectileLifetimeTerminate
{
    private float m_lifeTime;

    [SerializeField]
    private float m_lifeSpan = 1.0f;

    public override bool Dead()
    {
        return m_lifeTime >= m_lifeSpan;
    }

    public override void Enter(object[] input)
    {
    }

    public override void Tick()
    {
        m_lifeTime += Time.deltaTime;

        if (Dead())
        {
            Object.Destroy(m_parent.gameObject);
        }
    }
}