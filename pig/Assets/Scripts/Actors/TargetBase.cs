using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Health))]
public class TargetBase : PigScript
{
    DamagedBehavior[] m_healthBehaviors = new DamagedBehavior[((int)EHealthPool.HP_Count)];

    void Start()
    {
        m_healthBehaviors[((int)EHealthPool.HP_Shield)] = null;
        m_healthBehaviors[((int)EHealthPool.HP_Armor)] = null;
        m_healthBehaviors[((int)EHealthPool.HP_Base)] = null;

        DamagedBehavior[] damageSections = GetComponents<DamagedBehavior>();
        foreach (DamagedBehavior damageSection in damageSections)
        {
            m_healthBehaviors[((int)damageSection.Type)] = damageSection;
        }
    }

    public int GetCurrentHealthTotal()
    {
        int total = 0;
        foreach (DamagedBehavior damageSection in m_healthBehaviors)
        {
            if (damageSection != null)
            {
                //this ignores the armor section damage reduction...
                total += damageSection.CurrentHealth;
            }
        }
        return total;
    }

    [AutoRegisterEvent]
    void HandleDamageEvent(DamageEvent e)
    {
        foreach (DamagedBehavior damageSection in m_healthBehaviors)
        {
            if (damageSection != null)
            {
                e.damageInstance.damageAmmount -= damageSection.TakeDamage(e.damageInstance);
                //todo: make sure damage ammount cant go negative?
                Assert.IsFalse(e.damageInstance.damageAmmount < 0);
            }
        }

        Health baseHp = m_healthBehaviors[((int)EHealthPool.HP_Base)] as Health;
        if (!baseHp.Alive)
        {
            //tell the damage owner they did lethal damage
            UnitKillEvent killEvent = new UnitKillEvent();
            killEvent.killed = gameObject;

            EventCore.SendTo<UnitKillEvent>(this, e.damageInstance.damageOwner, killEvent);

            Destroy(gameObject);
        }
    }
}
