using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All scripts in pig should derive from this script so we can perform auto event registration.
/// Plus we can toss any other cross game functionality we want into this class.
/// </summary>
public class PigScript : EventListenerBehaviour
{
    protected override sealed void OnEnable()
    {
        DebugRegistry.RegisterListener(this);

        base.OnEnable();
        OnEnabled();
    }

    protected override sealed void OnDisable()
    {
        DebugRegistry.UnregisterListener(this);

        OnDisabled();
        base.OnDisable();
    }

    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
}
