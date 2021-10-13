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

    private TowerBuilderMode m_builder = null;

    // Start is called before the first frame update
    void Start()
    {
        bigBoy.onClick.AddListener(ButtonPressed);

        m_builder = FindObjectOfType<TowerBuilderMode>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotkey))
        {
            ButtonPressed();
        }
    }

    void ButtonPressed()
    {
        TowerBuilderMode builder = FindObjectOfType<TowerBuilderMode>();
        builder.AssignTowerPrefab(towerPrefab);

        FindObjectOfType<TowerBuilderMode>().EnableBuilderMode();
    }
}
