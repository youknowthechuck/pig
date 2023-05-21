using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShittyButton : MonoBehaviour
{
    public Button bigBoy;
    public GameObject towerPrefab;
    public KeyCode hotkey;

    public Color activeColor;
    public Color disabledColor;

    private TowerBuilderMode m_builder = null;

    private Bank m_bank = null;

    private bool m_enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        bigBoy.onClick.AddListener(ButtonPressed);

        m_builder = FindObjectOfType<TowerBuilderMode>();

        m_bank = FindObjectOfType<Bank>();
    }

    private void Update()
    {
        m_enabled = m_bank.Vault.CanAfford(towerPrefab.GetComponent<Currency>().Ammount);

        Image f = GetComponent<Image>();
        f.color = m_enabled ? activeColor : disabledColor;

        if (Input.GetKeyDown(hotkey))
        {
            ButtonPressed();
        }
    }

    void ButtonPressed()
    {
        if (m_enabled)
        {
            m_builder.AssignTowerPrefab(towerPrefab);

            m_builder.EnableBuilderMode();
        }
    }
}
