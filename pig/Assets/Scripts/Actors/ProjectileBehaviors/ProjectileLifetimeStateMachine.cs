using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetimeWaitForTarget : State
{
    GameObject m_target = null;

    float m_lifeTime = 0.0f;

    public virtual bool HasTarget()
    {
        return m_target != null;
    }

    public virtual bool Error()
    {
        //we *should* get a target immediately?
        //@bug - without this sometimes projectiles get stuck and do nothing,
        ///probably cause their target dies as this state is entered and they get a null target forever
        return m_lifeTime > 0.1f;
    }

    public override void Enter(object[] input)
    {
    }

    public override void Tick()
    {
        m_lifeTime += Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        m_target = target;
    }

    public override object[] Exit()
    {
        return new object[] { m_target };
    }
}

public class ProjectileLifetimeTravelToTarget : State
{
    protected GameObject m_target;

    protected ProjectileBase m_projectileObject;

    protected TargetBase m_hitTarget = null;

    protected bool m_timedOut = false;

    //default implementation - move towards target location in r3
    private Vector3 m_direction;

    public virtual bool HitTarget()
    {
        return m_hitTarget != null;
    }

    public virtual bool TimedOut()
    {
        return m_timedOut;
    }

    public void SetParent(ProjectileBase parent)
    {
        m_projectileObject = parent;
    }

    void OnTriggerEnter(Collider other)
    {
        m_hitTarget = other.gameObject.GetComponent<TargetBase>();
    }

    public override void Enter(object[] input)
    {
        m_target = input[0] as GameObject;
        Debug.Assert(m_target != null);
        Debug.Assert(m_projectileObject != null);

        PathFollower pathTarget = m_target.GetComponent<PathFollower>();
        if (pathTarget != null)
        {
            //assume that the time to time from shot origin to target doesn't change as the target moves along the path, because predicting that would be really hard
            Vector3 vectorToTarget = m_target.transform.position - m_projectileObject.transform.position;

            float distToTarget = vectorToTarget.sqrMagnitude;

            float timeToTarget = distToTarget / (m_projectileObject.Speed * m_projectileObject.Speed);

            Vector3 prediction = pathTarget.EvalPosition(pathTarget.LifeTime + timeToTarget);

            m_direction = (prediction - m_projectileObject.transform.position).normalized;
        }
        else
        {
            m_direction = (m_target.transform.position - m_projectileObject.transform.position).normalized;
        }

        m_projectileObject.Show();
    }

    public override object[] Exit()
    {
        return new object[] { m_hitTarget };
    }

    public override void Tick()
    {
        Debug.Assert(m_projectileObject != null);

        m_projectileObject.transform.position += m_direction * m_projectileObject.Speed * Time.deltaTime;

        //assumes y = 0 is ground plane, kill projectiles that get too far underneath
        const float KILL_THRESHOLD = -5.0f;

        if (m_projectileObject.transform.position.y <= KILL_THRESHOLD)
        {
            m_timedOut = true;
        }
    }
    
}

public class ProjectileLifetimeHitTarget : State
{
    protected TargetBase m_target;

    protected DamagerBehavior m_damager;

    protected Health m_health;

    public virtual bool HitStillAlive()
    {
        return m_health != null ? m_health.Alive : false;
    }

    public virtual bool HitDead()
    {
        //if we somehow don't have hp, say we're dead
        return m_health != null ? !m_health.Alive : true;
    }

    public void SetDamager(DamagerBehavior damager)
    {
        m_damager = damager;
    }

    public void SetHealth(Health hp)
    {
        m_health = hp;
    }

    public override void Enter(object[] input)
    {
        m_target = input[0] as TargetBase;
        Debug.Assert(m_target != null);

        m_damager?.TryDoDamage(m_target.gameObject);

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
        return new object[] { m_target };
    }

    public override void Tick()
    {
    }
}

public class ProjectileLifetimeTimeout : State
{
    //passthrough, we dead
    public virtual bool DidTimeout()
    {
        return true;
    }


    public override void Enter(object[] input)
    {
    }

    public override void Tick()
    {
    }
}

public class ProjectileLifetimeTerminate : State
{
    protected ProjectileBase m_parent;

    public void SetParent(ProjectileBase parent)
    {
        m_parent = parent;
    }

    public virtual bool Dead()
    {
        return true;
    }

    public override void Enter(object[] input)
    {
        Object.Destroy(m_parent.gameObject);
    }

    public override void Tick()
    {
    }
}

/// <summary>
/// Basic projectile lifetime:
///       wait for target
///            |
///            v
///       travel to target // miss target and timeout
///            |    ^               |
///            v    |               |
///       do big deeps              |
///            |                    |
///            v                    v
///           die, with fancy sparkles
/// </summary>
public class ProjectileLifetimeStateMachine : PigScript
{
    private StateMachine m_internalStateMachine;

    [SerializeField]
    ProjectileLifetimeWaitForTarget m_waitState;
    [SerializeField]
    ProjectileLifetimeTravelToTarget m_travelState;
    [SerializeField]
    ProjectileLifetimeHitTarget m_hitState;
    [SerializeField]
    ProjectileLifetimeTimeout m_timeoutState;
    [SerializeField]
    ProjectileLifetimeTerminate m_terminateState;

    public void SetTarget(GameObject target)
    {
        m_waitState.SetTarget(target);
    }

    void InitDefaultStates()
    {
        if (m_waitState == null)
        {
            m_waitState = gameObject.AddComponent<ProjectileLifetimeWaitForTarget>();
        }
        if (m_travelState == null)
        {
            m_travelState = gameObject.AddComponent<ProjectileLifetimeTravelToTarget>();
        }
        if (m_hitState == null)
        {
            m_hitState = gameObject.AddComponent<ProjectileLifetimeHitTarget>();
        }
        if (m_timeoutState == null)
        {
            m_timeoutState = gameObject.AddComponent<ProjectileLifetimeTimeout>();
        }
        if (m_terminateState == null)
        {
            m_terminateState = gameObject.AddComponent<ProjectileLifetimeTerminate>();
        }

    }

    void Awake()
    {
        InitDefaultStates();

        ProjectileBase parent = gameObject.GetComponent<ProjectileBase>();

        if (parent != null)
        {
            m_travelState.SetParent(parent);
            m_hitState.SetDamager(parent.GetComponent<DamagerBehavior>());
            m_hitState.SetHealth(parent.GetComponent<Health>());
            m_terminateState.SetParent(parent);

            m_internalStateMachine = new StateMachine();

            m_internalStateMachine.AddState(m_waitState);
            m_internalStateMachine.AddState(m_travelState);
            m_internalStateMachine.AddState(m_hitState);
            m_internalStateMachine.AddState(m_timeoutState);
            m_internalStateMachine.AddState(m_terminateState);

            m_waitState.AddStateLink(new StateLink(m_travelState, m_waitState.HasTarget));
            m_waitState.AddStateLink(new StateLink(m_terminateState, m_waitState.Error));
            m_travelState.AddStateLink(new StateLink(m_hitState, m_travelState.HitTarget));
            m_travelState.AddStateLink(new StateLink(m_timeoutState, m_travelState.TimedOut));
            m_hitState.AddStateLink(new StateLink(m_travelState, m_hitState.HitStillAlive));
            m_hitState.AddStateLink(new StateLink(m_terminateState, m_hitState.HitDead));
            m_timeoutState.AddStateLink(new StateLink(m_terminateState, m_timeoutState.DidTimeout));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_internalStateMachine?.Start<ProjectileLifetimeWaitForTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        m_internalStateMachine?.Tick();
    }
}
