using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : PigScript
{
    [SerializeField]
    Text m_Display = null;
    
    [SerializeField]
    int m_startingBalance = 0;

    Currency m_currency = null;

    public Currency Vault
    {
        get { return m_currency; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //banks hold their own currency
        m_currency = gameObject.AddComponent<Currency>();

        m_currency.Award(m_startingBalance);

        DebugRegistry.Invoke("AddCurrency 20");
    }

    // Update is called once per frame
    void Update()
    {
        m_Display.text = m_currency?.Ammount.ToString();
    }

    [DebugCommand]
    public void AddCurrency(int ammount)
    {
        m_currency.Award(ammount);
    }
}
