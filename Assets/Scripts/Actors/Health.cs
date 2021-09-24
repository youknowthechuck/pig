using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int m_baseHealth = 0;

    private int m_currentHealth;

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

    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = m_baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int TakeDamage(int damage)
    {
        m_currentHealth -= damage;
        return m_currentHealth;
    }
}
