using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingModeSelect : PigScript
{
    Dropdown m_input = null;

    private TowerBehaviorStateMachine m_tower = null;

    // Start is called before the first frame update
    void Awake()
    {
        m_input = GetComponent<Dropdown>();

        m_input.onValueChanged.AddListener(OnModeSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectingTower(TowerBehaviorStateMachine tower)
    {
        if (m_input != null)
        {
            m_tower = tower;

            m_input.ClearOptions();

            List<string> behaviorNames = new List<string>();
            for (int i = 0; i < m_tower.NumTargetingBehaviors; ++i)
            {
                TargetingBehavior behavior = m_tower.GetTargetingBehavior(i);

                behaviorNames.Add(behavior.PrettyName);
            }

            m_input.AddOptions(behaviorNames);

            m_input.value = m_tower.ActiveTargetingBehaviorIndex;
        }
    }

    void OnModeSelect(int modeIndex)
    {
        m_tower.SetTargetingBehavior(modeIndex);
    }
}
