using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
