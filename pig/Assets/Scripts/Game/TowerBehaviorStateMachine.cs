using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerBehaviorStatePlacement : State
{
    bool placed = false;

    public override void Enter(object[] input)
    {
    }

    public override void Tick()
    {
    }

    public bool IsPlaced()
    {
        return placed;
    }

    public void SetPlaced(bool isPlaced)
    {
        placed = isPlaced;
    }
}

public class TowerBehaviorStateWaitingToFire : State
{
    float baseAttackTime;

    float lastFireTime = Mathf.NegativeInfinity;

    bool canFire;

    public void Init(float BAT)
    {
        baseAttackTime = BAT;
    }

    public override void Enter(object[] input)
    {
        canFire = false;

        if (input?.Length > 0)
        {
            lastFireTime = (float)input[0];
        }
    }

    public override void Tick()
    {
        canFire = (Time.time - lastFireTime >= GetFireRatePerSecond());
    }

    public bool CanFire()
    {
        return canFire;
    }

    float GetFireRatePerSecond()
    {
        //this exists so that more complicated things can modify fire rates
        return baseAttackTime;
    }
}

public class TowerBehaviorStateAquireTarget : State
{
    public TargetingBehavior targetingBehavior;

    float range;

    TargetBase target;

    public void Init(TargetingBehavior targeting, float r)
    {
        targetingBehavior = targeting;
        range = r;
    }

    public override void Enter(object[] input)
    {
        target = null;
    }

    public override void Tick()
    {
        target = targetingBehavior.FindTarget(range);
    }

    public bool HasTarget()
    {
        return target != null;
    }

    public override object[] Exit() 
    { 
        return new object[] { target };
    }
}

public class TowerBehaviorStateFireProjectile : State
{
    ProjectileBase projectilePrefab = null;

    Transform projectileOrigin;

    GameObject tower;

    TargetBase target;

    bool fired = false;

    public void Init(ProjectileBase prefab, Transform origin, GameObject parent)
    {
        projectilePrefab = prefab;
        projectileOrigin = origin;
        tower = parent;
    }

    public override void Enter(object[] input)
    {
        target = null;
        fired = false;

        if (input.Length > 0)
        {
            target = input[0] as TargetBase;
        }
    }

    public override void Tick()
    {
        if (target != null)
        {
            ProjectileBase projectile = Object.Instantiate(projectilePrefab.gameObject, projectileOrigin.position, Quaternion.identity, projectileOrigin.transform).GetComponent<ProjectileBase>();
            projectile.transform.localScale = projectilePrefab.gameObject.transform.localScale;


            ProjectileLifetimeStateMachine sm = projectile.GetComponent<ProjectileLifetimeStateMachine>();
            sm.SetTarget(target.gameObject);

        }
        //we tried our best
        fired = true;
    }

    public override object[] Exit()
    {
        return new object[] { Time.time };
    }

    public bool DidFire()
    {
        return fired;
    }
}


/// <summary>
/// Basic tower behavior:
///         placement
///            |
///            v
///       wait to fire
///         /       ^
///        v         \
///  find target -> fire zee missile
/// </summary>
public class TowerBehaviorStateMachine : PigScript
{
    private StateMachine m_internalStateMachine;

    private TargetingBehavior[] m_targetingBehaviors;
    private int m_activeTargetingBehavior = 0;

    public int NumTargetingBehaviors
    {
        get { return m_targetingBehaviors.Length; }
    }
    public int ActiveTargetingBehaviorIndex
    {
        get { return m_activeTargetingBehavior; }
    }

    public TargetingBehavior GetTargetingBehavior(int index)
    {
        Debug.Assert(index >= 0 && index < m_targetingBehaviors.Length);
        return m_targetingBehaviors[index];
    }

    void Awake()
    {
        TowerBase parent = gameObject.GetComponent<TowerBase>();

        m_targetingBehaviors = parent.GetComponents<TargetingBehavior>();

        if (parent != null)
        {
            m_internalStateMachine = new StateMachine();

            TowerBehaviorStatePlacement placementState = gameObject.AddComponent<TowerBehaviorStatePlacement>();
            TowerBehaviorStateWaitingToFire waitState = gameObject.AddComponent<TowerBehaviorStateWaitingToFire>();
            waitState.Init(parent.BaseAttackTime);
            TowerBehaviorStateAquireTarget targetState = gameObject.AddComponent<TowerBehaviorStateAquireTarget>();
            targetState.Init(m_targetingBehaviors[m_activeTargetingBehavior], parent.Range);
            TowerBehaviorStateFireProjectile fireState = gameObject.AddComponent<TowerBehaviorStateFireProjectile>();
            fireState.Init(parent.ProjectileClass, parent.m_projectileOrigin, gameObject);

            m_internalStateMachine.AddState(placementState);
            m_internalStateMachine.AddState(waitState);
            m_internalStateMachine.AddState(targetState);
            m_internalStateMachine.AddState(fireState);

            placementState.AddStateLink(new StateLink(waitState, placementState.IsPlaced));
            waitState.AddStateLink(new StateLink(targetState, waitState.CanFire));
            targetState.AddStateLink(new StateLink(fireState, targetState.HasTarget));
            fireState.AddStateLink(new StateLink(waitState, fireState.DidFire));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_internalStateMachine.Start<TowerBehaviorStatePlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        m_internalStateMachine?.Tick();
    }

    public void SetTargetingBehavior(int index)
    {
        Debug.Assert(index >= 0 && index < m_targetingBehaviors.Length);
        m_activeTargetingBehavior = index;

        m_internalStateMachine.GetState<TowerBehaviorStateAquireTarget>().targetingBehavior = m_targetingBehaviors[m_activeTargetingBehavior];
    }

    [AutoRegisterEvent]
    void HandleTowerPlacedEvent(TowerPlacedEvent e)
    {
        m_internalStateMachine.GetState<TowerBehaviorStatePlacement>().SetPlaced(true);
    }
}
