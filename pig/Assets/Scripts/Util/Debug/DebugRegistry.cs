using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;

public struct DebugCommandData
{
    public Type type;
    public MethodInfo methodInfo;
}

class DebugRegistry : SingletonClass<DebugRegistry>
{
    private Dictionary<string, DebugCommandData> commandMethods = new Dictionary<string, DebugCommandData>();

    private Dictionary<Type, List<object>> commandListeners = new Dictionary<Type, List<object>>();


    public void GenerateFullDebugCommandRegistry()
    {
        foreach (Type type in Assembly.GetAssembly(typeof(PigScript)).GetTypes())
        {
            GatherCommandData(type);
        }
    }

    public static void RegisterListener(object listener)
    {
        Type listenerType = listener.GetType();

        List<object> registerd;
        if (!Instance.commandListeners.TryGetValue(listenerType, out registerd))
        {
            registerd = new List<object>();
        }

        registerd.Add(listener);
        Instance.commandListeners[listenerType] = registerd;
    }
    public static void UnregisterListener(object listener)
    {
        Type listenerType = listener.GetType();

        List<object> registerd;
        if (!Instance.commandListeners.TryGetValue(listenerType, out registerd))
        {
            Debug.LogError(String.Format("Object of type {0} trying to unregister but none were found.", listenerType.ToString()));
        }
        else if (!registerd.Remove(listener))
        {
            Debug.LogError(String.Format("Object {0} trying to unregister but was not in list.", listener.ToString()));
        }
        Instance.commandListeners[listenerType] = registerd;
    }

    public static void Invoke(string commandLine)
    {
        Instance.InvokeImpl(commandLine);
    }

    private void InvokeImpl(string commandLine)
    {
        string[] args = commandLine.Split(' ');
        DebugCommandData data;
        if (commandMethods.TryGetValue(args[0], out data))
        {
            List<object> listeners;
            if (commandListeners.TryGetValue(data.type, out listeners))
            {
                object[] methodParams;
                if (ParamsFromString(args.Skip(1).ToArray(), data.methodInfo.GetParameters(), out methodParams))
                {
                    foreach (object listener in listeners)
                    {
                        data.methodInfo.Invoke(listener, methodParams);
                    }
                }
                else
                {
                    Debug.LogError(String.Format("Debug command \"{0}\" was malformed", commandLine));
                }
            }
        }
        else
        {
            Debug.LogError(String.Format("No debug command \"{0}\" found", args[0]));
        }
    }

    private bool ParamsFromString(string[] args, ParameterInfo[] descriptions, out object[] formattedParams)
    {
        Debug.Assert(args.Length == descriptions.Length);

        formattedParams = new object[args.Length];

        for (int i = 0; i < args.Length; ++i)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(descriptions[i].ParameterType);
            Debug.Assert(tc != null);

            if (!tc.IsValid(args[i]))
            {
                //if any args fail to convert, bail
                return false;
            }

            formattedParams[i] = tc.ConvertFrom(null, CultureInfo.InvariantCulture, args[i]);
        }

        return true;
    }

    private void GatherCommandData(Type behaviourType)
    {
        MethodInfo[] methods = behaviourType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        foreach (MethodInfo mi in methods)
        {
            if (!mi.GetCustomAttributes(true).OfType<DebugCommand>().Any())
            {
                continue;
            }

            DebugCommandData commandData;
            if (commandMethods.TryGetValue(mi.Name, out commandData))
            {
                string commandError = String.Format("Method {0} for type {1} already defined for type {2}",
                    mi.Name,
                    behaviourType.Name,
                    commandData.type.Name);

                Debug.LogError(commandError);
            }
            else
            {
                commandData = new DebugCommandData();
                commandData.type = behaviourType;
                commandData.methodInfo = mi;

                commandMethods.Add(mi.Name, commandData);
            }
        }
    }

    protected override void Initialize()
    {
        GenerateFullDebugCommandRegistry();
    }
}
