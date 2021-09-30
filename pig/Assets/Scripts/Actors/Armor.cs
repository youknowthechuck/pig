using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//armor reduces incoming damage as it is applied, unless the damage type fully damages armor
public class Armor : DamagedBehavior
{
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

    // Update is called once per frame
    void Update()
    {

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