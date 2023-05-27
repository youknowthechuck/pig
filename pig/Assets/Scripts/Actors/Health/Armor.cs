using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//armor reduces incoming damage as it is applied, unless the damage type fully damages armor
public class Armor : DamagedBehavior
{
    private void Awake()
    {
        if (m_healthBarUI != null)
        {
            m_healthBarUI.material.SetColor(m_shaderNameFillColor, new Color(0.5f, 0.5f, 0.5f));
            m_healthBarUI.material.SetFloat(m_shaderNameSegmentLength, 1.0f);
            m_healthBarUI.material.SetFloat(m_shaderNameHealthPool, m_baseHealth);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //double the armor base health pool, effectively halving incoming damage
        m_currentHealth = 2 * m_baseHealth;
    }

    public Armor()
    {
        m_hpType = EHealthPool.HP_Armor;
    }

    public override int TakeDamage(DamageInstance damageInstance)
    {
        int damageAbsorbed = 0;
        if (!damageInstance.damageType.HasFlag(EDamageType.DT_BypassArmor))
        {
            int adjustedDamage = damageInstance.damageAmmount;
            if (damageInstance.damageType.HasFlag(EDamageType.DT_DamageArmorFully))
            {
                adjustedDamage *= 2;
            }
            damageAbsorbed = Math.Min(adjustedDamage, m_currentHealth);
            m_currentHealth -= damageAbsorbed;
        }
        if (damageInstance.damageType.HasFlag(EDamageType.DT_DamageArmorFully))
        {
            damageAbsorbed /= 2;
        }
        return damageAbsorbed;
    }
}