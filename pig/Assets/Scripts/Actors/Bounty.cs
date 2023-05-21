using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : PigScript
{
    [SerializeField]
    List<Component> m_rewards;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public T GetReward<T>() where T : Component
    {
        foreach (Component rewardObject in m_rewards)
        {
            if (rewardObject is T)
            {
                return rewardObject as T;
            }
        }

        return null;
    }
}
