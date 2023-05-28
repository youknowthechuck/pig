using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : PigScript
{
    //this is some shit, we can only have one of these in the world
    //and check this shit to block inputs to buttons or whatever
    public static bool Active = false;

    string m_lastGeneratedString = "";

    InputField m_inputField;

    [SerializeField]
    VerticalLayoutGroup m_autoCompleteContentLayout;

    [SerializeField]
    AutoCompleteEntry m_autoCompleteEntryTemplate;

    List<AutoCompleteEntry> m_autoCompleteEntries = new List<AutoCompleteEntry>();

    List<string> m_commandHistory = new List<string>();

    private int m_selectedEntry = 0;
    private int m_selectedHistory = 0;

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
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            FillAutoComplete();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectNext();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectPrevious();
        }
        else if (Active && Input.anyKey)
        {
            string command = m_inputField.text.Trim().ToLower();
            if (command != m_lastGeneratedString)
            {
                m_lastGeneratedString = command;
                GenerateAutoCompletes(m_lastGeneratedString);
            }
        }
    }

    void FillAutoComplete()
    {
        m_inputField.text = m_autoCompleteEntries[m_selectedEntry].EntryText;
        m_inputField.MoveTextEnd(false);
    }

    void ExecuteCommand()
    {
        DebugRegistry.Invoke(m_inputField.text);

        m_commandHistory.Add(m_inputField.text);

        m_inputField.text = "";
        m_lastGeneratedString = "";
        GenerateAutoCompletes(m_lastGeneratedString);
        m_selectedHistory = 0;
    }

    void SelectNext()
    {
        if (m_autoCompleteEntries.Count > 0)
        {
            m_autoCompleteEntries[m_selectedEntry].SetHighlighted(false);
            m_selectedEntry = (m_selectedEntry + 1) % m_autoCompleteEntries.Count;
            m_autoCompleteEntries[m_selectedEntry].SetHighlighted(true);
        }
    }

    void SelectPrevious()
    {
        if (m_autoCompleteEntries.Count > 0)
        {
            m_autoCompleteEntries[m_selectedEntry].SetHighlighted(false);
            m_selectedEntry = m_selectedEntry - 1;
            if (m_selectedEntry < 0)
            {
                m_selectedEntry = m_autoCompleteEntries.Count - 1;
            }
            m_autoCompleteEntries[m_selectedEntry].SetHighlighted(true);
        }
    }

    void GenerateAutoCompletes(string input)
    {
        m_selectedEntry = 0;

        List<UnityEngine.Object> autoCompleteObjects = DebugRegistry.GetMatchingListeners(input);
        List<DebugCommandData> autoCompleteMethods = DebugRegistry.GetMatchingMethods(input);

        List<string> options = new List<string>();

        foreach (UnityEngine.Object listener in autoCompleteObjects)
        {
            options.Add(listener.name);
        }
        foreach (DebugCommandData method in autoCompleteMethods)
        {
            options.Add(method.methodInfo.Name);
        }

        foreach (var entry in m_autoCompleteEntries)
        {
            GameObject.Destroy(entry.gameObject);
        }

        m_autoCompleteEntries.Clear();

        foreach (string option in options)
        {
            AutoCompleteEntry entry = Instantiate<AutoCompleteEntry>(m_autoCompleteEntryTemplate, m_autoCompleteContentLayout.transform);
            entry.gameObject.SetActive(true);
            entry.SetText(option);
            entry.SetHighlighted(false);
            m_autoCompleteEntries.Add(entry);
        }

        if (m_autoCompleteEntries.Count > 0)
        {
            m_autoCompleteEntries[m_selectedEntry].SetHighlighted(true);
        }
    }

    void Toggle()
    {
        if (Active)
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
        m_inputField.text.TrimEnd('`');
        m_inputField?.DeactivateInputField();

        gameObject.GetComponentInChildren<Canvas>().enabled = false;

        Active = false;
    }

    void Show()
    {
        gameObject.GetComponentInChildren<Canvas>().enabled = true;

        m_inputField?.ActivateInputField();

        m_lastGeneratedString = m_inputField.text;
        GenerateAutoCompletes(m_lastGeneratedString);

        Active = true;
    }
}
