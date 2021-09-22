using System;
using System.Collections.Generic;

public class StateLink
{
    public delegate bool TransitionCondition();

    private TransitionCondition m_conditional;
    private State m_nextState;

    public StateLink(State nextState, TransitionCondition conditional)
    {
        m_conditional = conditional;
        m_nextState = nextState;
    }

    public State NextState
    {
        get { return m_nextState; }
    }

    public bool IsSatisfied
    {
        get { return m_conditional(); }
    }
}

