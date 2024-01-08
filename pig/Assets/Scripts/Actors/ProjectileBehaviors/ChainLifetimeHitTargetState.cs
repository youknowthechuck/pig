using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLifetimeHitTargetState : ProjectileLifetimeHitTarget
{
    [SerializeField]
    private float m_lifeSpan = 0.5f;

    [SerializeField]
    private float m_chainRadius = 60.0f;

    private float m_lifetime = 0.0f;

    private GameObject m_chainTarget = null;

    private Vector3 m_endPoint = Vector3.zero;


    void FindChainTarget()
    {
        List<GameObject> freshTargets = new List<GameObject>();
        List<GameObject> shockedTargets = new List<GameObject>();

        Vector3 targetPos = m_target.transform.position;

        float r2 = m_chainRadius * m_chainRadius;

        foreach (TargetBase potentialTarget in Object.FindObjectsOfType<TargetBase>())
        {
            //don't chain to the last thing we hit
            if (potentialTarget == m_target)
            {
                continue;
            }

            Vector3 nextPos = potentialTarget.gameObject.transform.position;

            if ( (nextPos - targetPos).sqrMagnitude > r2 )
            {
                continue;
            }

            else if (potentialTarget.GetComponent<LightningStruck>() != null) 
            {
                shockedTargets.Add(potentialTarget.gameObject);
            }
            else
            {
                freshTargets.Add(potentialTarget.gameObject);
            }
        }

        System.Random nextIndex = new System.Random();

        //chain to a random potential target, preferring a target that hasn't been hit yet
        if (freshTargets.Count > 0)
        {
            m_chainTarget = freshTargets[nextIndex.Next(freshTargets.Count)];
        }
        else if (shockedTargets.Count > 0)
        {
            m_chainTarget = shockedTargets[nextIndex.Next(shockedTargets.Count)];
        }
    }

    public override bool HitStillAlive()
    {
        bool hasHealth = m_health != null ? m_health.Alive : false;
        bool hasTarget = m_chainTarget != null;

        bool goNext = (m_lifetime >= m_lifeSpan) && hasHealth && hasTarget;

        //big jank. update the base projectile position to draw the next chain from the oringinal target location
        if (goNext)
        {
            m_damager.GetComponent<ProjectileBase>().Hide();
            m_damager.gameObject.transform.position = m_endPoint;
        }

        return goNext;
    }

    public override bool HitDead()
    {
        return (m_lifetime >= m_lifeSpan) && !HitStillAlive();
    }

    public override void Enter(object[] input)
    {
        m_lifetime = 0.0f;
        m_chainTarget = null;

        m_target = input[0] as TargetBase;
        Debug.Assert(m_target != null);

        m_damager?.TryDoDamage(m_target.gameObject);

        m_endPoint = m_target.transform.position;
        m_endPoint.y += 20.0f; //jank

        //mark target as shocked if it isn't alraedy, to deprioritize it as a chain target
        LightningStruck shock = m_target.gameObject.GetComponent<LightningStruck>();
        if (shock == null)
        {
            shock = m_target.gameObject.AddComponent<LightningStruck>();
        }
        shock.ResetLifetime();

        FindChainTarget();

        if (m_health != null)
        {
            //jank, kill ourselves
            DamageInstance selfDamage = new DamageInstance();

            selfDamage.damageAmmount = 1;
            selfDamage.damageType = EDamageType.DT_Base;
            selfDamage.damageOwner = m_health.gameObject;

            m_health.TakeDamage(selfDamage);
        }
    }

    public override object[] Exit()
    {

        return new object[] { m_chainTarget };
    }

    public override void Tick()
    {
        m_lifetime += Time.deltaTime;
    }
}
