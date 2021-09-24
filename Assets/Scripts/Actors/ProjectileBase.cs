using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : PigScript
{
    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    float m_speed = 1.0f;

    private Bounds m_bounds;

    public int Damage
    { 
        get { return m_damage; } 
    }

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
        DamageEvent e = new DamageEvent();
        e.damageAmmount = m_damage;

        EventCore.SendTo<DamageEvent>(this, other.gameObject, e);

        Health myHP = GetComponent<Health>();
        if (myHP != null)
        {
            myHP.TakeDamage(1);
            if (!myHP.Alive)
            {
                Destroy(gameObject);
            }
        }
    }
}
