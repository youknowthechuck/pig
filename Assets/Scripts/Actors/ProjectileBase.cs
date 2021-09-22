using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private int m_damage;

    [SerializeField]
    float m_speed;

    private Bounds m_bounds;

    public int Damage
    { 
        get { return m_damage;} 
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
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += m_direction * m_speed * Time.deltaTime;

        if (transform.position.y < m_bounds.extents.y)
        {
            Destroy(gameObject);
        }
    }
}
