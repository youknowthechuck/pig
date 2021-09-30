using System;
using System.Collections.Generic;

public class StateMachine
{
    public State CurrentState
    {
        get;
        private set;
    }

    public State PreviousState
    {
        get;
        private set;
    }

    private Dictionary<Type, State> m_states;

    public StateMachine()
    {
        m_states = new Dictionary<Type, State>();
    }

    public void AddState<StateType>(StateType state) where StateType : State
    {
        m_states.Add(typeof(StateType), state);
    }

    public StateType GetState<StateType>() where StateType : State
    {
        State state = m_states[typeof(StateType)];
        return (StateType)state;
    }

    public void RemoveState<StateType>() where StateType : State
    {
        StateType state = GetState<StateType>();
        m_states.Remove(typeof(StateType));
    }

    public void ForceState<StateType>() where StateType : State
    {
        ChangeState(GetState<StateType>());
    }

    public void Start<StateType>() where StateType : State
    {
        if(CurrentState == null)
        {
            ChangeState(GetState<StateType>());
        }
    }

    public void Tick()
    {
        if(CurrentState == null)
        {
            return;
        }

        CurrentState.Tick();

        foreach(StateLink link in CurrentState.StateLinks)
        {
            if(link.IsSatisfied)
            {
                ChangeState(link.NextState);
                break;
            }
        }
    }

    private void ChangeState(State nextState)
    {
        if(nextState == CurrentState)
        {
            // why u do?
            return;
        }

        if(CurrentState != null)
        {
            CurrentState.Exit();
        }

        PreviousState = CurrentState;
        CurrentState = nextState;

        CurrentState.Enter();
    }
}