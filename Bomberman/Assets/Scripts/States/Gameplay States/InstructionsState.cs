using System.Collections;
using UnityEngine;

public class InstructionsState : State
{
    [Range(0.0f, 10.0f)]
    [SerializeField] private float m_Time = 0.0f;

    [Header("UI")]
    [SerializeField] private GameObject m_InstructionsUI;

    protected override void Awake()
    {
        base.Awake();
    }


    public override void Enter()
    {
        m_InstructionsUI.SetActive(true);
        StartCoroutine(ShowInstructions(m_Time));
    }

    public override void Exit()
    {
        m_InstructionsUI.SetActive(true);
    }

    private IEnumerator ShowInstructions(float time)
    {
        yield return new WaitForSeconds(time);

        AdvanceToNextState();
    }

}
