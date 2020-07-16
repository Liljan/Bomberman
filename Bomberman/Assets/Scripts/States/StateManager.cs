using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private State m_CurrentState = null;

    [SerializeField] private State m_StartState;

    void Start()
    {
        DeactivateAllStates();
        GotoStartState();
    }

    private void DeactivateAllStates()
    {
       foreach(State state in this.gameObject.GetComponents<State>())
            state.enabled = false;
    }

    private void GotoStartState()
    {
        m_CurrentState = m_StartState;
        EnterCurrentState();
    }

    private void ExitCurrentState()
    {
        m_CurrentState.Exit();
        m_CurrentState.enabled = false;
    }

    private void EnterCurrentState()
    {
        m_CurrentState.enabled = true;
        m_CurrentState.Enter();
    }

    public bool GotoState(State state)
    {
        Debug.Assert(state);

        ExitCurrentState();
        m_CurrentState = state;
        EnterCurrentState();

        return true;
    }
}
