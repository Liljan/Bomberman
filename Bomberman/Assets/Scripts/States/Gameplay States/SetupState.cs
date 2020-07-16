using System.Collections;
using UnityEngine;

public class SetupState : State
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        Debug.Log("Entering Setup state.");

        StartCoroutine(SimulateSetup(0.2f));
    }

    private IEnumerator SimulateSetup(float time)
    {
        yield return new WaitForSeconds(time);

        AdvanceToNextState();
    }

    public override void Exit()
    {
    }
}
