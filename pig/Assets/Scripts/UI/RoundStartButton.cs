using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundStartButton : MonoBehaviour
{
    public Button m_input;

    public Director m_director;

    // Start is called before the first frame update
    void Start()
    {
        m_input.onClick.AddListener(ButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ButtonPressed()
    {
        m_director?.BeginNextRound(false);
    }
}
