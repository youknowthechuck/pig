using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// It's game instance.
/// Exists the entire lifecycle of the game. Only one instance exists per executable.
/// Listens for level/scene change events and loads or tearsdown level specific scripts.
/// </summary>
public class GameCore : SingletonBehaviour<GameCore>
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += ActiveSceneChanged;

        Debug.Log("GAME STARTED");
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SCENE LOADED: " + scene.name + "  -  MODE:" + mode);

        // Add scene statics
        GameObject sceneStatics = new GameObject("SceneStatics");
        sceneStatics.AddComponent<SceneStatics>();
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("SCENE TEARDOWN: " + scene.name);
    }

    void ActiveSceneChanged(Scene lastScene, Scene newScene)
    {
        Debug.Log("SCENE CHANGED  - OLD: " + ((lastScene != null) ? lastScene.name : "NO SCENE") + "  -  NEW:" + ((newScene != null) ? newScene.name : "NO SCENE"));
    }
}
