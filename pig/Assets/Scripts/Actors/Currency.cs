using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Currency : PigScript
{
    [SerializeField]
    int m_ammount = 0;

    public int Ammount
    {
        get { return m_ammount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Award(int ammount)
    {
        m_ammount += ammount;
    }

    public bool CanAfford(int cost)
    {
        return cost <= m_ammount;
    }

    public bool Spend(int cost)
    {
        Assert.IsTrue(CanAfford(cost));

        m_ammount -= cost;

        return m_ammount >= 0;
    }
}
