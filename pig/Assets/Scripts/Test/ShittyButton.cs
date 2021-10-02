using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShittyButton : MonoBehaviour
{
    public Button bigBoy;

    // Start is called before the first frame update
    void Start()
    {
        bigBoy.onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed()
    {
        SceneManager.LoadScene("Connor_PolyTest", LoadSceneMode.Single);
    }
}
