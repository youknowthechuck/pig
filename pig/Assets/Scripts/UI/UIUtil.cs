using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIUtil
{
    public static void InstantiateUI()
    {
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (!canvas)
        {
            GameObject UIRoot = new GameObject("UI");
            GameObject canvasGO = new GameObject("Canvas");
            canvasGO.transform.parent = UIRoot.transform;
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject eventSystem = Object.Instantiate(new GameObject("EventSystem"), UIRoot.transform);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}
