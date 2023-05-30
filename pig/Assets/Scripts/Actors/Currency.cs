using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Currency : PigScript
{
    [SerializeField]
    int m_amount = 0;

    public int Amount
    {
        get { return m_amount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Award(int amount)
    {
        m_amount += amount;
    }

    public bool CanAfford(int cost)
    {
        return cost <= m_amount;
    }

    public bool Spend(int cost)
    {
        Assert.IsTrue(CanAfford(cost));

        m_amount -= cost;

        return m_amount >= 0;
    }
}
