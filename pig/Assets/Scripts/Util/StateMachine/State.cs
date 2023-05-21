using System;
using System.Collections.Generic;
using UnityEngine;

public class State : PigScript
{
    public State()
    {
        StateLinks = new List<StateLink>();
    }

    public List<StateLink> StateLinks
    {
        get;
        private set;
    }

    public void AddStateLink(StateLink link)
    {
        StateLinks.Add(link);
    }

    public virtual void Enter(object[] input) { }

    public virtual void Tick() { }

    public virtual object[] Exit() { return null; }
}

