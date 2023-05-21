using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKillListener : PigScript
{
    [SerializeField]
    Text m_killFeed = null;

    int m_killCount;
    // Start is called before the first frame update
    void Start()
    {
        if (m_killFeed != null)
        {
            m_killFeed.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    [AutoRegisterEvent]
    void HandleUnitKillEvent(UnitKillEvent e)
    {
        ++m_killCount;

        if (m_killFeed != null)
        {
            m_killFeed.text = m_killCount.ToString();
        }
    }
}
