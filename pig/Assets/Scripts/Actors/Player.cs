using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Player : PigScript
{
    [SerializeField]
    Text m_HealthUI;

    DamagedBehavior m_baseHealth;

    // Start is called before the first frame update
    void Start()
    {
        m_baseHealth = null;

        //take the first base health component we find
        //it's not technically an error to have multiple health components on the player, but its kind of useless right now
        //shields one day maybe?
        DamagedBehavior[] damageSections = GetComponents<DamagedBehavior>();
        foreach (DamagedBehavior damageSection in damageSections)
        {
            if (damageSection.Type == EHealthPool.HP_Base)
            {
                m_baseHealth = damageSection;
                break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_HealthUI.text = m_baseHealth.CurrentHealth.ToString();
    }


    [AutoRegisterEvent]
    void HandleDamageEvent(DamageEvent e)
    {
        Debug.Assert(m_baseHealth != null, "No base health on player. Please add a DamagedBehavior with HP_Base type.");

        m_baseHealth.TakeDamage(e.damageInstance);

        if (!m_baseHealth.Alive)
        {
            //game over man
        }
    }
}
