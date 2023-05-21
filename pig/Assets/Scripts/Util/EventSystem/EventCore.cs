using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base event core class. Singleton.
/// </summary>
public class EventCore : SingletonClass<EventCore>
{
    /// <summary>
    /// The base Delegate type.
    /// </summary>
    /// <typeparam name="T">Derived type from EventBase.</typeparam>
    /// <param name="evn">The event being passed during the callback.</param>
    public delegate void EventDelegate<in T>(T evn) where T : EventBase;

    /// <summary>
    /// Internal container of event listeners the event type as the key.
    /// </summary>
    private Dictionary<System.Type, EventTypeListenerContainer> eventContainers;

    /// <summary>
    /// Container holding all of the reflected type to auto registering events data.
    /// </summary>
    private AutoRegisteringEventsPerType autoRegisteringEventsPerType;

    /// <summary>
    /// Adds a new listener for a specific event type.
    /// </summary>
    /// <typeparam name="T">Type derived from EventBase.</typeparam>
    /// <param name="callback">The callback being added.</param>
    public static void AddListener<T>(EventDelegate<T> callback) where T : EventBase
    {
        Instance.AddListenerInternal(typeof(T), callback);
    }

    /// <summary>
    /// Adds a new listener based on the event type and a generic callback definition.
    /// </summary>
    /// <param name="type">The event type.</param>
    /// <param name="callback">The callback.</param>
    public static void AddListener(System.Type type, System.Delegate callback)
    {
        Instance.AddListenerInternal(type, callback);
    }

    /// <summary>
    /// Removes a specific callback/listener.
    /// </summary>
    /// <typeparam name="T">Type derived from EventBase</typeparam>
    /// <param name="callback">The callback to remove.</param>
    public static void RemoveListener<T>(EventDelegate<T> callback) where T : EventBase
    {
        Instance.RemoveListenerInternal(typeof(T), callback);
    }

    /// <summary>
    /// Removes a listener given the event type and the generic callback.
    /// </summary>
    /// <param name="type">The event type.</param>
    /// <param name="callback">Callback to remove.</param>
    public static void RemoveListener(System.Type type, System.Delegate callback)
    {
        Instance.RemoveListenerInternal(type, callback);
    }

    /// <summary>
    /// Sends an event to every single listener.
    /// </summary>
    /// <typeparam name="T">Event type derived from EventBase</typeparam>
    /// <param name="from">The component broadcasting the event.</param>
    /// <param name="eventInstance">The event instance.</param>
    public static void BroadcastEvent<T>(Component from, T eventInstance) where T : EventBase
    {
        Instance.BroadcastEventInternal(from, eventInstance);
    }

    /// <summary>
    /// Sends an event to a specific GameObject. If they are listening for it they will receive it.
    /// </summary>
    /// <typeparam name="T">Event type derived from EventBase.</typeparam>
    /// <param name="source">The component sending the event.</param>
    /// <param name="target">The target gameobject listening for the event.</param>
    /// <param name="eventInstance">The event instance.</param>
    public static void SendTo<T>(Component source, GameObject target, T eventInstance) where T : EventBase
    {
        Instance.SendToInternal<T>(source, target, eventInstance);
    }

    /// <summary>
    /// Gets every auto registering event on the specified type.
    /// </summary>
    /// <param name="type">The behaviour type to search.</param>
    /// <returns>The auto registering events on the specified type.</returns>
    public static List<AutoRegisteringEventData> GetAutoRegisteringEventsOnType(System.Type type)
    {
        return Instance.autoRegisteringEventsPerType.GetAutoRegisteringEventsOnType(type);
    }

    protected override void Initialize()
    {
        // Container construction
        eventContainers = new Dictionary<System.Type, EventTypeListenerContainer>();

        // Auto registration
        autoRegisteringEventsPerType = new AutoRegisteringEventsPerType();
        autoRegisteringEventsPerType.GenerateAllAutoRegistrationEvents();
    }

    private void AddListenerInternal(System.Type eventType, System.Delegate callback)
    {
        EventTypeListenerContainer eventContainer;

        if (eventContainers.TryGetValue(eventType, out eventContainer))
        {
            eventContainer.AddListener(callback);
        }
        else
        {
            eventContainer = new EventTypeListenerContainer();
            eventContainer.AddListener(callback);
            eventContainers.Add(eventType, eventContainer);
        }
    }

    private void RemoveListenerInternal(System.Type eventType, System.Delegate callback)
    {
        EventTypeListenerContainer eventContainer;

        if (eventContainers.TryGetValue(eventType, out eventContainer))
        {
            eventContainer.RemoveListener(callback);
        }
    }

    private void BroadcastEventInternal<T>(Component from, T eventInstance) where T : EventBase
    {
        System.Type eventType = typeof(T);
        EventTypeListenerContainer eventContainer;

        if (!eventContainers.TryGetValue(eventType, out eventContainer))
        {
            return;
        }

        foreach (EventDelegate<T> callback in eventContainer)
        {
            Component componentTarget = callback.Target as Component;
            eventInstance.SetData(from, componentTarget.gameObject);

            eventInstance.PreSend();
            callback.Invoke(eventInstance);
            eventInstance.PostSend();
        }
    }

    private void SendToInternal<T>(Component from, GameObject to, T eventInstance) where T : EventBase
    {
        System.Type eventType = typeof(T);
        EventTypeListenerContainer eventContainer;

        if (!eventContainers.TryGetValue(eventType, out eventContainer))
        {
            return;
        }

        GameObject next = to;
        while (next != null)
        {
            foreach (EventDelegate<T> callback in eventContainer)
            {
                Component originalTarget = callback.Target as Component;
                if (originalTarget.gameObject.Equals(next))
                {
                    eventInstance.SetData(from, next);

                    eventInstance.PreSend();
                    callback.Invoke(eventInstance);
                    eventInstance.PostSend();

                    break;
                }
            }

            //dispatch event all the way up the target owner chain
            //ObjectOwner ownerComponent = next.GetComponent<ObjectOwner>();
            //next = ownerComponent?.Owner;
            next = next.transform.parent?.gameObject;
        }
    }
}
