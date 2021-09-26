using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EDamageType
{
    DT_Base,
    DT_BypassArmor,
    DT_DamageArmorFully,
    DT_BypassShield
}

//if you add something here make sure you add damage types above that make sense
//these are in priority order. enum value == order damage is applied in (ie. first shields, then armor, then base hp)
public enum EHealthPool
{
    HP_Shield,
    HP_Armor,
    HP_Base,
    HP_Count
}

public struct DamageInstance
{
    public EDamageType damageType;

    public int damageAmmount;

    public GameObject damageOwner;
}
