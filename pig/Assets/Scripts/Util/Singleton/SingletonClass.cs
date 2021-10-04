/* ----------------------------------------------------------------------------
  Copyright (c) Chuck Martin and Connor Hollis. All Rights Reserved.
---------------------------------------------------------------------------- */

using System;

/// <summary>
/// This is a singleton class. It can be created via a call to the Instance property. 
/// It can also be created via the "Static Construct" method.
/// Lastly you can manually construct an instance with new T().
/// Either way it will remain globally accessible for the lifetime of the project unless it is manually destroyed.
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
            }

            return m_instance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            return m_instance != null;
        }
    }

    public SingletonClass()
    {
        if(m_instance != null)
        {
            throw new InvalidOperationException(string.Format("SingletonClass constructor: A singleton instance of type '{0}' has already been initialized.", typeof(T)));
        }

        m_instance = this as T;
        Initialize();
    }

    public static void StaticConstruct()
    {
        lock (m_instanceLock)
        {
            if (m_instance != null)
            {
                throw new InvalidOperationException(string.Format("SingletonClass constructor: A singleton instance of type '{0}' has already been initialized.", typeof(T)));
            }
            else
            {
                // Construct object, instance is automatically assigned, and initialize is called
                SingletonClass<T> singletonInstance = new T() as SingletonClass<T>;
            }
        }
    }

    public static void StaticDestroy()
    {
        lock (m_instanceLock)
        {
            if (m_instance == null)
            {
                throw new InvalidOperationException(string.Format("SingletonClass.StaticDestroy: A singleton instance of type '{0}' was not available to destroy.", typeof(T)));
            }

            (m_instance as SingletonClass<T>).Teardown();
            m_instance = null;
        }
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void Teardown()
    {

    }
}