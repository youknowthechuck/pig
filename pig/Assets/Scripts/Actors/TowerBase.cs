using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : PigScript
{
    //1.0 == 1 projectile per second
    [SerializeField]
    private float m_baseAttackTime = 1.0f;
    [SerializeField]
    private ProjectileBase m_projectilePrefab = null;
    [SerializeField]
    private float m_range = 1.0f;

    public Transform m_projectileOrigin;

    public float BaseAttackTime
    {
        get { return m_baseAttackTime; }
    }

    public ProjectileBase ProjectileClass
    {
        get { return m_projectilePrefab; }
    }

    public float Range
    {
        get { return m_range; }
    }

    //transient properties
    float m_lastFireTime;

    private TargetingBehavior m_targetingBehavior;

    // Start is called before the first frame update
    void Start()
    {
        m_lastFireTime = Mathf.NegativeInfinity;

        m_targetingBehavior = GetComponent<TargetingBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_targetingBehavior == null)
        {
            return;
        }
        else if (Time.time - m_lastFireTime < GetFireRatePerSecond())
        {
            //not time to fire, don't do any targeting work
            return;
        }

        TargetBase target = m_targetingBehavior.FindTarget(m_range);

        if (target != null)
        {
            ProjectileBase projectile = GameObject.Instantiate<ProjectileBase>(m_projectilePrefab, m_projectileOrigin.position, Quaternion.identity);
            projectile.SetTarget(target.gameObject);
            //todo: projectile targeting
            m_lastFireTime = Time.time;
        }
    }


    float GetFireRatePerSecond()
    {
        //this exists so that more complicated things can modify fire rates
        return m_baseAttackTime;
    }
}
