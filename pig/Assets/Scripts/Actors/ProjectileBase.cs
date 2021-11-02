using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamagerBehavior))]
public class ProjectileBase : PigScript
{
    [SerializeField]
    float m_speed = 1.0f;

    private Bounds m_bounds;

    DamagerBehavior m_damager;

    public float Speed
    {
        get { return m_speed; }
    }

    private Vector3 m_direction;

    public void SetTarget(GameObject target)
    {
        m_direction = (target.transform.position - transform.position).normalized;
    }

    private void Start()
    {
        m_bounds = GetComponent<MeshRenderer>().bounds;

        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>());

        m_damager = GetComponent<DamagerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += m_direction * m_speed * Time.deltaTime;

        //if the projectile is some distance away from the origin beyond this arbitrary number, kill it cause its far away
        const float KILL_THRESHOLD = 10000.0f;

        if (transform.position.sqrMagnitude > KILL_THRESHOLD)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //@todo: this should be done with layers or whatever bullshit
        if (other.gameObject.GetComponent<TargetBase>() != null)
        {
            m_damager.TryDoDamage(other.gameObject);
        }

        Health myHP = GetComponent<Health>();
        if (myHP != null)
        {
            //jank, kill ourselves
            DamageInstance selfDamage = new DamageInstance();

            selfDamage.damageAmmount = 1;
            selfDamage.damageType = EDamageType.DT_Base;
            selfDamage.damageOwner = gameObject;

            myHP.TakeDamage(selfDamage);

            if (!myHP.Alive)
            {
                Destroy(gameObject);
            }
        }
    }
}
