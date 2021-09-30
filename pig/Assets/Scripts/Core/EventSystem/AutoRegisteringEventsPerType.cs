using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public struct AutoRegisteringEventData
{
    public Type eventType;
    public MethodInfo methodInfo;
    public Type delegateType;

    public Delegate GenerateDelegate(Component target)
    {
        return Delegate.CreateDelegate(delegateType, target, methodInfo);
    }
}

/// <summary>
/// Base container type so we can keep a non templated container instance in the event core dictionary.
/// </summary>
public class AutoRegisteringEventsPerType
{
    /// <summary>
    /// Dictionary where the key is the behaviour type, and the value is a list of event types and methods that point to that.
    /// </summary>
    private Dictionary<Type, List<AutoRegisteringEventData>> behaviourEventMappings = new Dictionary<Type, List<AutoRegisteringEventData>>();

    /// <summary>
    /// Gets all of the auto registering events on the specified type. Will add the events if they are not already determined.
    /// </summary>
    /// <param name="behaviourType">The behaviour to search.</param>
    /// <returns></returns>
    public List<AutoRegisteringEventData> GetAutoRegisteringEventsOnType(Type behaviourType)
    {
        return GenerateBehaviourMapping(behaviourType);
    }

    /// <summary>
    /// Gets all of the auto registering events on all monobehaviours in the game. Will register events with the behaviour mappings.
    /// </summary>
    /// <returns></returns>
    public void GenerateAllAutoRegistrationEvents()
    {
        foreach (Type type in Assembly.GetAssembly(typeof(EventListenerBehaviour)).GetTypes())
        {
            GenerateBehaviourMapping(type);
        }
    }

    /// <summary>
    /// Generates the actual event callback information from reflection.
    /// </summary>
    /// <param name="behaviourType">The behaviour type to search for auto registering functions.</param>
    /// <returns>List of all auto registering methods and event data.</returns>
    private List<AutoRegisteringEventData> GenerateBehaviourMapping(Type behaviourType)
    {
        List<AutoRegisteringEventData> eventList;
        if (!behaviourEventMappings.TryGetValue(behaviourType, out eventList))
        {
            // Add a null list but mark that this type was considered
            behaviourEventMappings.Add(behaviourType, null);

            MethodInfo[] methods = behaviourType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (MethodInfo mi in methods)
            {
                if (!mi.GetCustomAttributes(true).OfType<AutoRegisterEvent>().Any())
                {
                    continue;
                }

                ParameterInfo[] parameters = mi.GetParameters();

                // Only one param allowed
                if (parameters.Length != 1)
                {
                    continue;
                }

                // It must be a method that takes eventbase
                Type paramType = parameters[0].ParameterType;
                if (!paramType.IsSubclassOf(typeof(EventBase)))
                {
                    continue;
                }

                // Get the generic event type
                Type genericEventDelegateType = typeof(EventCore.EventDelegate<>).GetGenericTypeDefinition().MakeGenericType(paramType);

                AutoRegisteringEventData eventToMethod;
                eventToMethod.eventType = paramType;
                eventToMethod.methodInfo = mi;
                eventToMethod.delegateType = genericEventDelegateType;

                // Only construct an event list if we have an event on this behaviour
                if(eventList == null)
                {
                    eventList = new List<AutoRegisteringEventData>();
                    behaviourEventMappings[behaviourType] = eventList;
                }

                eventList.Add(eventToMethod);
            }
        }

        return eventList;
    }
}