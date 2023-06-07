using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum EStatModifierOperator
{
    OP_Multiply,
    OP_Add,
    OP_Count
};

[System.Serializable]
public class StatModifier
{
    public string Name = "";

    public float Modifier;

    public EStatModifierOperator Operator;

    public float Modify(float baseValue)
    {
        float modifiedValue = baseValue;
        switch (Operator)
        {
            case EStatModifierOperator.OP_Multiply:
                modifiedValue *= Modifier;
                break;
            case EStatModifierOperator.OP_Add:
                modifiedValue += Modifier;
                break;
            default:
                Debug.LogError(string.Format("Modifier for stat {0} has unknown operator {1}", Name, Operator.ToString()));
                break;
        }
        return modifiedValue;
    }
}