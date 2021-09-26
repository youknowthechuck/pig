using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedBehavior : PigScript
{
    [SerializeField]
    protected EHealthPool m_hpType;

    [SerializeField]
    protected int m_baseHealth = 0;

    protected int m_currentHealth;

    public int BaseHealth
    {
        get { return m_baseHealth; }
    }
    public int CurrentHealth
    {
        get { return m_currentHealth; }
    }

    public bool Alive
    {
        get { return CurrentHealth > 0; }
    }

    public EHealthPool Type
    {
        get { return m_hpType; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = m_baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //returns how much damage was taken, not necessarily damageInstance.damageAmmount
    public virtual int TakeDamage(DamageInstance damageInstance)
    {
        return -1;
    }
}
