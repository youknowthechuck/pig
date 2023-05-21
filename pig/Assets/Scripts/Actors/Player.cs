using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class Player : PigScript
{
    [SerializeField]
    Text m_HealthUI = null;

    [SerializeField]
    CanvasRenderer m_TowerSelectionOptions;

    DamagedBehavior m_baseHealth;

    //bleh
    Bank m_bank;

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

        m_TowerSelectionOptions.gameObject.SetActive(false);

        m_bank = FindObjectOfType<Bank>();
    }

    // Update is called once per frame
    void Update()
    {
        m_HealthUI.text = m_baseHealth.CurrentHealth.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool clickedTower = false;
            if (Physics.Raycast(ray, out hit))
            {
                TargetingModeSelect hitUI = hit.transform.gameObject.GetComponentInChildren<TargetingModeSelect>(true);
                TowerBehaviorStateMachine hitTower = hit.transform.gameObject.GetComponent<TowerBehaviorStateMachine>();
                if (hitTower != null)
                {
                    clickedTower = true;

                    TargetingModeSelect modeSelect = m_TowerSelectionOptions.GetComponentInChildren<TargetingModeSelect>(true);

                    modeSelect.SetSelectingTower(hitTower);
                }
                else if (hitUI != null)
                {
                    clickedTower = true;
                }

            }
            m_TowerSelectionOptions.gameObject.SetActive(clickedTower);

        }
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

    [AutoRegisterEvent]
    void HandleUnitKillEvent(UnitKillEvent e)
    {
        //this is dumb?.1
        Currency reward = e.killed.GetComponent<Bounty>()?.GetReward<Currency>();

        if (reward != null)
        {
            m_bank?.Vault?.Award(reward.Ammount);
        }
    }

    [AutoRegisterEvent]
    void HandleTowerPlacedEvent(TowerPlacedEvent e)
    {
        int cost = e.Tower.GetComponent<Currency>().Ammount;

        m_bank.Vault.Spend(cost);
    }
}
