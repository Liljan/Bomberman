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


    public abstract void Enter();
    public abstract void Exit();
}
