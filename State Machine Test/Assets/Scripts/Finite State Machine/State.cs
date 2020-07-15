using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateManager m_StateManager;

    [SerializeField]
    protected State m_NextState;

    protected virtual void Awake()
    {
        m_StateManager = this.gameObject.GetComponent<StateManager>();
    }

    protected virtual void AdvanceToNextState()
    {
        m_StateManager.GotoState(m_NextState);
    }

    public abstract void Enter();
    public abstract void Exit();
}
