using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//shields act just like base health, theyre just another hp pool that sits on top of base and soaks damage first
public class Shield : DamagedBehavior
{
    public Shield()
    {
        m_hpType = EHealthPool.HP_Shield;
    }

    public override int TakeDamage(DamageInstance damageInstance)
    {
        int damageApplied = 0;
        if (!damageInstance.damageType.HasFlag(EDamageType.DT_BypassShield))
        {
            damageApplied = Math.Min(damageInstance.damageAmmount, m_currentHealth);
            m_currentHealth -= damageApplied;
        }
        return damageApplied;
    }
}
