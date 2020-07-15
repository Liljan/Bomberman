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
    }

    public override void Exit()
    {
        Debug.Log("Exiting Setup state.");
    }
}
