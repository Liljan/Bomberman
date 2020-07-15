using System.Collections;
using UnityEngine;

public class InstructionsState : State
{
    [Range(0.0f, 10.0f)]
    public float m_Time = 0.0f;

    protected override void Awake()
    {
        base.Awake();
    }


    public override void Enter()
    {
        StartCoroutine(ShowInstructions(m_Time));
    }

    public override void Exit()
    {
    }

    private IEnumerator ShowInstructions(float time)
    {
        yield return new WaitForSeconds(time);

        AdvanceToNextState();
    }

}
