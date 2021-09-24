using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base behaviour class for event listeners and broadcasters.
/// </summary>
public class EventListenerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Boolean for determining if this behaviour has done auto event registration.
    /// </summary>
    private bool hasAutoRegisteredEvents = false;

    /// <summary>
    /// The auto registered event data
    /// </summary>
    private List<KeyValuePair<Type, Delegate>> autoRegisteredEvents;

    /// <summary>
    /// Adds a new event listener to the event core.
    /// </summary>
    /// <typeparam name="T">Event type.</typeparam>
    /// <param name="callback">The callback to add.</param>
    public void AddListener<T>(EventCore.EventDelegate<T> callback) where T : EventBase
    {
        EventCore.AddListener<T>(callback);
    }

    /// <summary>
    /// Removes an event listener from the event core.
    /// </summary>
    /// <typeparam name="T">Event type.</typeparam>
    /// <param name="callback">The callback to remove.</param>
    public void UnregisterListener<T>(EventCore.EventDelegate<T> callback) where T : EventBase
    {
        EventCore.RemoveListener<T>(callback);
    }

    /// <summary>
    /// Sends an event to the target GameObject.
    /// </summary>
    /// <typeparam name="T">The event type.</typeparam>
    /// <param name="evn">The event instance.</param>
    /// <param name="target">The target gameobject.</param>
    public void SendEvent<T>(T evn, GameObject target) where T : EventBase
    {
        EventCore.SendTo<T>(this, target, evn);
    }

    /// <summary>
    /// Broadcasts an event to every listener.
    /// </summary>
    /// <typeparam name="T">The event type.</typeparam>
    /// <param name="evn">The event instance.</param>
    public void BroadcastEvent<T>(T evn) where T : EventBase
    {
        EventCore.BroadcastEvent<T>(this, evn);
    }

    /// <summary>
    /// Call to add any auto registering events to the event system.
    /// </summary>
    public void AutoRegisterEvents()
    {
        // Only generate auto registered events the first time
        if (!hasAutoRegisteredEvents)
        {
            List<AutoRegisteringEventData> autoRegisteringEvents = EventCore.GetAutoRegisteringEventsOnType(GetType());
            if ((autoRegisteringEvents != null) && (autoRegisteringEvents.Count > 0))
            {
                autoRegisteredEvents = new List<KeyValuePair<Type, Delegate>>();
                foreach (AutoRegisteringEventData eventData in autoRegisteringEvents)
                {
                    autoRegisteredEvents.Add(new KeyValuePair<Type, Delegate>(eventData.eventType, eventData.GenerateDelegate(this)));
                }
            }

            hasAutoRegisteredEvents = true;
        }

        if (autoRegisteredEvents != null)
        {
            foreach (KeyValuePair<Type, Delegate> typeAndDelegate in autoRegisteredEvents)
            {
                EventCore.AddListener(typeAndDelegate.Key, typeAndDelegate.Value);
            }
        }
    }

    /// <summary>
    /// Call to unregister any auto registering events.
    /// </summary>
    public void AutoUnregisterEvents()
    {
        if (autoRegisteredEvents == null)
        {
            return;
        }

        foreach (KeyValuePair<Type, Delegate> typeAndDelegate in autoRegisteredEvents)
        {
            EventCore.RemoveListener(typeAndDelegate.Key, typeAndDelegate.Value);
        }
    }

    protected virtual void OnEnable()
    {
        AutoRegisterEvents();
    }

    protected virtual void OnDisable()
    {
        AutoUnregisterEvents();
    }
}