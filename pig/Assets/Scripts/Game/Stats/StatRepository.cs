using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

public class StatRepository : PigScript
{
    //keep these all separate. for ...reasons?
    public Dictionary<string, float> BaseStats = new Dictionary<string, float>();

    //modifiers are an kept in an array of lists where array length == EStatModifierOperator::OP_Count
    //when calculating final stat values, modifiers are applied in the operator order defined in the enum
    public Dictionary<string, List<StatModifier>[]> StatModifiers = new Dictionary<string, List<StatModifier>[]>();

    public Dictionary<string, float> FinalStats = new Dictionary<string, float>();

    public void Awake()
    {
        FullRebuild();
    }

    public void Reset()
    {
        //gameobject components just changed so recalculate stats in case modifiers were added or removed
        FullRebuild();
    }

    void FullRebuild()
    {
        CompileBaseStats();
        CompileStatMods();
        CalculateFinalStats();
    }

    void CompileBaseStats()
    {
        Component[] allComponents = GetComponentsInChildren<Component>();

        BaseStats.Clear();

        foreach (Component comp in allComponents)
        {
            foreach (var fieldInfo in comp.GetType().GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ))
            {
                if (fieldInfo.FieldType == typeof(StatBase))
                {
                    StatBase statValue = fieldInfo.GetValue(comp) as StatBase;
                    Debug.Assert(statValue != null);

                    if (!BaseStats.ContainsKey(statValue.Name))
                    {
                        BaseStats.Add(statValue.Name, statValue.BaseValue);
                    }
                    else 
                    {
                        Debug.LogError(string.Format("StatRepository found duplicate stat {0} on object {1}", statValue.Name, gameObject.name));
                    }
                }
            }
        }
    }

    void CompileStatMods()
    {
        Component[] allComponents = GetComponentsInChildren<Component>();

        StatModifiers.Clear();

        foreach (Component comp in allComponents)
        {
            foreach (var fieldInfo in comp.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (fieldInfo.FieldType == typeof(StatModifier))
                {
                    StatModifier modValue = fieldInfo.GetValue(comp) as StatModifier;
                    Debug.Assert(modValue != null);

                    List<StatModifier>[] mods = null;
                    if (StatModifiers.TryGetValue(modValue.Name, out mods))
                    {

                    }
                    else
                    {
                        mods = new List<StatModifier>[(int)EStatModifierOperator.OP_Count];
                        for (int modListIndex = 0; modListIndex < (int)EStatModifierOperator.OP_Count; ++modListIndex)
                        {
                            mods[modListIndex] = new List<StatModifier>();
                        }
                    }

                    mods[(int)modValue.Operator].Add(modValue);

                    StatModifiers[modValue.Name] = mods;
                }
            }
        }
    }

    void CalculateFinalStats()
    {
        FinalStats.Clear();

        foreach (var baseStat in BaseStats)
        {
            float statValue = baseStat.Value;

            if (StatModifiers.ContainsKey(baseStat.Key))
            {
                //@bug? are these ordered correct?
                foreach (var statModList in StatModifiers[baseStat.Key])
                {
                    foreach (var statMod in statModList)
                    {
                        statValue = statMod.Modify(statValue);
                    }
                }
            }

            FinalStats.Add(baseStat.Key, statValue);
        }
    }
}
