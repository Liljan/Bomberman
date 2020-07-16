using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField]
    protected StateManager m_StateManager;

    [SerializeField]
    protected State m_NextState;

    protected virtual void Awake()
    {
        Debug.Assert(m_StateManager);

        DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void AdvanceToNextState()
    {
        m_StateManager.GotoState(m_NextState);
    }

    public abstract void Enter();
    public abstract void Exit();
}
