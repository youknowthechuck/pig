using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerBehavior : PigScript
{
    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    [EnumFlags]
    EDamageType m_damageType = EDamageType.DT_Base;

    public int Damage
    {
        get { return m_damage; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryDoDamage(GameObject target)
    {
        DamageEvent e = new DamageEvent();
        e.damageInstance = new DamageInstance();

        e.damageInstance.damageAmmount = m_damage;
        e.damageInstance.damageType = m_damageType;
        e.damageInstance.damageOwner = gameObject;

        EventCore.SendTo<DamageEvent>(this, target, e);
    }
}
