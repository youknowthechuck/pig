/* ----------------------------------------------------------------------------
  Connor wrote this a long time ago.
---------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// </summary>
public class SingletonClass<T> where T : class, new()
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
                    // Construct object, instance is automatically assigned, and initialize is called
                    SingletonClass<T> singletonInstance = new T() as SingletonClass<T>;
                }

                return m_instance;
            }
        }
    }

    public SingletonClass()
    {
        m_instance = this as T;
        Initialize();
    }

    public static void StaticConstruct()
    {
        if(m_instance == null)
        {
            m_instance = new T();
        }
    }

    protected virtual void Initialize()
    {

    }
}