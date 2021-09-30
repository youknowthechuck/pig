﻿/* ----------------------------------------------------------------------------
  Connor wrote this a long time ago.
---------------------------------------------------------------------------- */

using System;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is the monobehaviour instance
/// </summary>
public class SingletonBehaviour<T>
    : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// The actual singleton instance for this class.
    /// </summary>
    private static T m_instance;

    /// <summary>
    /// Object used to prevent multi-threading mistakes when instantiating singletons.
    /// </summary>
    private static object m_instanceLock = new object();

    /// <summary>
    /// Boolean to determine if Unity is shutting down.
    /// </summary>
    private static bool m_hasInitialized = false;

    /// <summary>
    /// The property accessor for this singleton's instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (m_instanceLock)
            {
                if (m_instance == null)
                {
                    // Look for an instance
                    T found = FindObjectOfType<T>();
                    if (found == null)
                    {
                        // Create gameobject that represents our singleton
                        GameObject singleton = new GameObject();
                        m_instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                }

                if (!m_hasInitialized)
                {
                    // Call init
                    SingletonBehaviour<T> singletonInstance = m_instance as SingletonBehaviour<T>;
                    singletonInstance.Initialize();
                }

                return m_instance;
            }
        }
    }

    public static bool HasInstance
    {
        get { return m_instance != null; }
    }

    /// <summary>
    /// Initialization function derived classes can override.
    /// </summary>
    public void Initialize()
    {
        if (m_hasInitialized)
        {
            throw new InvalidOperationException(string.Format("SingletonBehavior.Initialize: A singleton instance of type '{0}' has already been initialized", typeof(T)));
        }

        m_instance = this as T;
        m_hasInitialized = true;
        OnInitialize();
    }

    /// <summary>
    /// Initialization function derived classes can override.
    /// </summary>
    protected virtual void OnInitialize()
    {

    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        if(ReferenceEquals(this, m_instance))
        {
            m_instance = null;
            m_hasInitialized = false;
        }
    }
}