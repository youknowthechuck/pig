using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DamagerBehavior))]
public class ProjectileBase : PigScript
{
    [SerializeField]
    float m_speed = 1.0f;

    public float Speed
    {
        get { return m_speed; }
    }

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
    }

    //show/hide are hacky work arounds for the projectile transform not being correct until targeting is done
    //which is very apparent with beam projectiles
    public void Hide()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void Show()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
