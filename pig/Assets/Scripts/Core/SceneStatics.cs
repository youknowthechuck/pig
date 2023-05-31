using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneStatics : MonoBehaviour
{
    void Awake()
    {
        // Add all of the required scene statics in this function
        FindOrCreateStatic<Bank>();
        FindOrCreateBankText();
        FindOrCreateDebugConsole();
    }

    void FindOrCreateStatic<T>() where T : MonoBehaviour 
    {
        if (!FindObjectOfType<T>())
        {
            gameObject.AddComponent<T>();
        }
    }

    // One off dummy shit
    void FindOrCreateBankText()
    {
        if (!FindObjectOfType<BankText>())
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if(!canvas)
            {
                UIUtil.InstantiateUI();
            }

            BankConfig config = Resources.Load<BankConfig>(ConfigPaths.BankConfigPath);

            GameObject canvasGO = FindObjectOfType<Canvas>().gameObject;
            GameObject textGO = Instantiate(config.bankTextPrefab, canvasGO.transform);
            RectTransform transform = textGO.GetComponent<RectTransform>();
            transform.anchorMin = new Vector2(0, 1);
            transform.anchorMax = new Vector2(0, 1);
            transform.anchoredPosition = new Vector3(100, -100, 0);
        }
    }

    // One off dummy shit
    void FindOrCreateDebugConsole()
    {
        if (!FindObjectOfType<DebugConsole>())
        {
            DebugConsoleConfig config = Resources.Load<DebugConsoleConfig>(ConfigPaths.DebugConsoleConfigPath);
            Instantiate(config.consolePrefab);
        }
    }
}
