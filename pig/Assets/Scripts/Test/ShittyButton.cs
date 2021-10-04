using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShittyButton : MonoBehaviour
{
    public Button bigBoy;
    public GameObject towerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        bigBoy.onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed()
    {
        TowerBuilderMode builder = FindObjectOfType<TowerBuilderMode>();
        builder.AssignTowerPrefab(towerPrefab);

        FindObjectOfType<TowerBuilderMode>().EnableBuilderMode();
    }
}
