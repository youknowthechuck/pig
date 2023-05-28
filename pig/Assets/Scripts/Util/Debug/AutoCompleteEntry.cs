using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoCompleteEntry : PigScript
{
    public string EntryText = "";

    [SerializeField]
    TextMeshProUGUI m_entryText;

    [SerializeField]
    GameObject m_highlightObject;

    public void SetText(string text)
    {
        EntryText = text;
        m_entryText.text = EntryText;
    }

    public void SetHighlighted(bool highlight)
    {
        m_highlightObject.SetActive(highlight);
    }
}
