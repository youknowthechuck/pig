using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base container type so we can keep a non templated container instance in the event core dictionary.
/// </summary>
public class EventTypeListenerContainer 
    : IEnumerable
{
    /// <summary>
    /// Container of callbacks.
    /// </summary>
    private List<System.Delegate> listeners = new List<System.Delegate>();

    /// <summary>
    /// Adds a callback to the container.
    /// </summary>
    /// <param name="callback">Callback to add</param>
    public virtual void AddListener(System.Delegate callback)
    {
        listeners.Add(callback);
    }

    /// <summary>
    /// Removes a specific callback from the container.
    /// </summary>
    /// <param name="callback">The callback to remove.</param>
    public virtual void RemoveListener(System.Delegate callback)
    {
        listeners.Remove(callback);
    }

    /// <summary>
    /// Enumerable inherited function.
    /// </summary>
    /// <returns>The enumerator from the listeners list.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return listeners.GetEnumerator();
    }
}