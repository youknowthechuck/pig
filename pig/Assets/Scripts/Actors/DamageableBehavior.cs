using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatRepository))]
public class DamageableBehavior : PigScript
{
    [SerializeField]
    protected Renderer m_healthBarUI;

    protected EHealthPool m_hpType;

    [SerializeField]
    protected StatBase m_baseHealthStat;

    protected int m_currentHealth;

    protected string m_shaderNameCurrentHealth = "Vector1_b99262e463b94ad9b76ae68c7d134048";
    protected string m_shaderNameHealthPool = "Vector1_fcec30fafb6f4688a492e604506b656d";
    protected string m_shaderNameFillColor = "Color_e6bdc935bd4242df9480e778f410a5b4";
    protected string m_shaderNameSegmentLength = "Vector1_e52cc9fbe38b4e9982e9fa833fa5865d";

    public EHealthPool HealthPoolType
    {
        get { return m_hpType; }
    }

    public int BaseHealth
    {
        get 
        {
            float statValue = 0.0f;
            GetComponent<StatRepository>().TryGetStatValue(m_baseHealthStat.Name, out statValue);

            return Mathf.FloorToInt(statValue);
        }
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

    private void Awake()
    {
        if (m_healthBarUI != null)
        {
            m_healthBarUI.material.SetFloat(m_shaderNameHealthPool, BaseHealth);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_currentHealth = BaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_healthBarUI != null)
        {
            m_healthBarUI.material.SetFloat(m_shaderNameCurrentHealth, m_currentHealth);
        }
    }

    //returns how much damage was taken, not necessarily damageInstance.damageAmmount
    public virtual int TakeDamage(DamageInstance damageInstance)
    {
        return -1;
    }
}
