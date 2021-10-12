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
}

public class TowerBehaviorStateWaitingToFire : State
{
    float baseAttackTime;

    float lastFireTime = Mathf.NegativeInfinity;

    bool canFire;

    public TowerBehaviorStateWaitingToFire(float BAT)
    {
        baseAttackTime = BAT;
    }

    public override void Enter(object[] input)
    {
        canFire = false;

        if (input.Length > 0)
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
    TargetingBehavior targetingBehavior;

    float range;

    TargetBase target;

    public TowerBehaviorStateAquireTarget(TargetingBehavior targeting, float r)
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

    TargetBase target;

    bool fired = false;

    public TowerBehaviorStateFireProjectile(ProjectileBase prefab, Transform origin)
    {
        projectilePrefab = prefab;
        projectileOrigin = origin;
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
            ProjectileBase projectile = GameObject.Instantiate<ProjectileBase>(projectilePrefab, projectileOrigin.position, Quaternion.identity);
            projectile.SetTarget(target.gameObject);
            //todo: projectile targeting
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
public class TowerBehaviorStateMachine : MonoBehaviour
{
    private StateMachine m_internalStateMachine;

    public TowerBehaviorStateMachine(TowerBase tower)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        TowerBase parent = gameObject.GetComponent<TowerBase>(); 

        if (parent != null)
        {
            m_internalStateMachine = new StateMachine();

            TowerBehaviorStatePlacement placementState = new TowerBehaviorStatePlacement();
            TowerBehaviorStateWaitingToFire waitState = new TowerBehaviorStateWaitingToFire(parent.BaseAttackTime);
            TowerBehaviorStateAquireTarget targetState = new TowerBehaviorStateAquireTarget(parent.GetComponent<TargetingBehavior>(), parent.Range);
            TowerBehaviorStateFireProjectile fireState = new TowerBehaviorStateFireProjectile(parent.ProjectileClass, parent.m_projectileOrigin);

            m_internalStateMachine.AddState(placementState);
            m_internalStateMachine.AddState(waitState);
            m_internalStateMachine.AddState(targetState);
            m_internalStateMachine.AddState(fireState);

            placementState.AddStateLink(new StateLink(waitState, placementState.IsPlaced));
            waitState.AddStateLink(new StateLink(targetState, waitState.CanFire));
            targetState.AddStateLink(new StateLink(fireState, targetState.HasTarget));
            fireState.AddStateLink(new StateLink(waitState, fireState.DidFire));

            m_internalStateMachine.Start<TowerBehaviorStateWaitingToFire>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_internalStateMachine.Tick();
    }
}
