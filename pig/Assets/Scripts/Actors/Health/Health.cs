using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : DamagedBehavior
{
    public Health()
    {
        m_hpType = EHealthPool.HP_Base;
    }

    // Update is called once per frame
    void Update()
    {
        
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
