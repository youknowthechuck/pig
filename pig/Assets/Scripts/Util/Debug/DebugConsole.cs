using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : PigScript
{
    bool m_enabled;

    InputField m_inputField;

    [SerializeField]
    VerticalLayoutGroup m_autoCompleteContentLayout;

    [SerializeField]
    Text m_autoCompleteDefaultLabel;

    List<Text> m_autoCompleteLabels = new List<Text>();

    List<string> m_autoCompleteOptions = new List<string>();

    private void Awake()
    {
        //we always want this around so keep it loaded.
        DontDestroyOnLoad(gameObject);

        m_inputField = gameObject.GetComponentInChildren<InputField>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    // Update is called once per frame`
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Toggle();
        }
        else if (m_enabled && Input.anyKey)
        {
            string command = m_inputField.text;
            GenerateAutoCompletes(command.Trim().ToLower());
        }
    }

    void GenerateAutoCompletes(string input)
    {
        m_autoCompleteOptions.Clear();

        List<UnityEngine.Object> autoCompleteObjects = DebugRegistry.GetMatchingListeners(input);
        List<DebugCommandData> autoCompleteMethods = DebugRegistry.GetMatchingMethods(input);

        foreach (UnityEngine.Object listener in autoCompleteObjects)
        {
            m_autoCompleteOptions.Add(listener.name);
        }
        foreach (DebugCommandData method in autoCompleteMethods)
        {
            m_autoCompleteOptions.Add(method.methodInfo.Name);
        }

        foreach (Text label in m_autoCompleteLabels)
        {
            GameObject.Destroy(label.gameObject);
        }

        m_autoCompleteLabels.Clear();

        foreach (string option in m_autoCompleteOptions)
        {
            Text label = Instantiate<Text>(m_autoCompleteDefaultLabel, m_autoCompleteContentLayout.transform);
            label.text = option;
            label.gameObject.SetActive(true);
            m_autoCompleteLabels.Add(label);
        }
    }

    void Toggle()
    {
        if (m_enabled)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Hide()
    {
        m_inputField?.DeactivateInputField();

        gameObject.GetComponentInChildren<Canvas>().enabled = false;

        m_enabled = false;
    }

    void Show()
    {
        gameObject.GetComponentInChildren<Canvas>().enabled = true;

        m_inputField?.ActivateInputField();

        m_enabled = true;
    }
}
