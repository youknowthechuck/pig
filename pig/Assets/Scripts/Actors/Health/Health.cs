using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : DamagedBehavior
{
    public Health()
    {
        m_hpType = EHealthPool.HP_Base;
    }

    private void Awake()
    {
        if (m_healthBarUI != null)
        {
            m_healthBarUI.material.SetColor(m_shaderNameFillColor, new Color(0.0f, 1.0f, 0.0f));
            m_healthBarUI.material.SetFloat(m_shaderNameHealthPool, m_baseHealth);
        }
    }

    public override int TakeDamage(DamageInstance damageInstance)
    {
        if (damageInstance.damageType.HasFlag(EDamageType.DT_Base))
        {
            m_currentHealth -= damageInstance.damageAmmount;
        }
        return damageInstance.damageAmmount;
    }
}
