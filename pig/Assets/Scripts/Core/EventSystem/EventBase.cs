using UnityEngine;

/// <summary>
/// Base event class for communicating between GameObjects and components.
/// </summary>
public class EventBase
{
    private Component eventSource;
    private GameObject eventTarget;

    /// <summary>
    /// Read-only event source property.
    /// </summary>
    public Component EventSource
    {
        get { return eventSource; }
    }

    /// <summary>
    /// Read-only event target property.
    /// </summary>
    public GameObject EventTarget
    {
        get { return eventTarget; }
    }

    /// <summary>
    ///  Sets the internal data for this event.
    /// </summary>
    /// <param name="source">The event source.</param>
    /// <param name="target">The event target.</param>
    public void SetData(Component source, GameObject target)
    {
        eventSource = source;
        eventTarget = target;
    }

    /// <summary>
    /// Called before we send the event.
    /// </summary>
    public virtual void PreSend()
    {

    }

    /// <summary>
    /// Called after we send the event.
    /// </summary>
    public virtual void PostSend()
    {

    }
}
