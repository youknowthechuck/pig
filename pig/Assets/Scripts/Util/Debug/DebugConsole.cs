using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//commands can be invoked either with "command [args]" or "object.command [args]"
//invoking a command without a target object will send the command to all registered listeners for that command
public class DebugConsole : PigScript
{
    //this is some shit, we can only have one of these in the world
    //and check this shit to block inputs to buttons or whatever
    public static bool Active = false;

    string m_lastGeneratedString = "";

    string m_objectNameParse = "";
    UnityEngine.Object m_objectParse = null;
    string m_commandParse = "";
    List<string> m_argsParse = new List<string>();

    bool m_objectComplete = false;
    bool m_commandComplete = false;

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
            ParseInput(command);

            if (command != m_lastGeneratedString)
            {
                m_lastGeneratedString = command;
                GenerateAutoCompletes(m_lastGeneratedString);
            }
        }
    }

    void ParseInput(string input)
    {
        m_objectNameParse = input;
        m_commandParse = "";
        m_argsParse.Clear();

        string[] splitOnObj = input.Split('.');
        m_objectComplete = splitOnObj.Length > 1;

        string rest = input;

        if (splitOnObj.Length > 1)
        {
            m_objectNameParse = splitOnObj[0];
            rest = splitOnObj[1];
            //ignore the rest we shouldn't have two or more '.'s
        }

        string[] args = rest.Split(' ');
        m_commandComplete = args.Length > 1;

        m_commandParse = args[0];

        m_argsParse = args.Skip(1).ToList();
    }


    void FillAutoComplete()
    {
        string[] splitEntry = m_autoCompleteEntries[m_selectedEntry].EntryText.Split(' ');

        m_inputField.text = m_objectComplete ? m_objectNameParse + '.' : "";
        m_inputField.text += splitEntry[0];
        m_inputField.MoveTextEnd(false);
    }

    void ExecuteCommand()
    {
        DebugRegistry.Invoke(m_commandParse, m_argsParse.ToArray(), m_objectParse);

        m_commandHistory.Add(m_inputField.text);

        ClearInput();
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

        List<UnityEngine.Object> autoCompleteObjects = null;
        autoCompleteObjects = DebugRegistry.GetMatchingListeners(m_objectNameParse);
        m_objectParse = autoCompleteObjects.Count > 0 ? autoCompleteObjects[0] : null;
        List<DebugCommandData> autoCompleteMethods = null;

        if (m_objectComplete)
        {
            autoCompleteMethods = DebugRegistry.GetMatchingMethodsForObject(m_commandParse, m_objectParse);
        }
        else
        {
            autoCompleteMethods = DebugRegistry.GetMatchingMethods(m_commandParse);
        }

        List<string> options = new List<string>();

        if (!m_objectComplete && !m_commandComplete)
        {
            foreach (UnityEngine.Object listener in autoCompleteObjects)
            {
                options.Add(listener.name);
            }
        }
        foreach (DebugCommandData method in autoCompleteMethods)
        {
            options.Add(DescribeCommand(method));
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

    string DescribeCommand(DebugCommandData data)
    {
        string commandString = data.methodInfo.Name;
        foreach (var param in data.methodInfo.GetParameters())
        {
            commandString += string.Format(" <{0} {1}>", param.ParameterType.Name, param.Name);
        }

        return commandString;
    }

    void ClearInput()
    {
        m_inputField.text = "";
        m_lastGeneratedString = "";
        m_selectedHistory = 0;

        ParseInput("");
        GenerateAutoCompletes("");
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
