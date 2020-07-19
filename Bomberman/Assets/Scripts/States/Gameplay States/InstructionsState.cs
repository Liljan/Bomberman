using System.Collections;
using UnityEngine;

public class InstructionsState : State
{
    [Header("UI")]
    [SerializeField] private GameObject m_InstructionsUI;

    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(m_InstructionsUI);
    }

    public override void Enter()
    {
        m_InstructionsUI.SetActive(true);
    }

    public override void Exit()
    {
        m_InstructionsUI.SetActive(false);
    }

    public void CompleteState()
    {
        AdvanceToNextState();
    }
}
