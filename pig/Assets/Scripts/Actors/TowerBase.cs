using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerBehaviorStateMachine))]
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

    private TowerBehaviorStateMachine m_behaviorSM;

    public TowerBase()
    {
        m_behaviorSM = new TowerBehaviorStateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
