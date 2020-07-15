using System.Collections;
using UnityEngine;

public class PreGameState : State
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        StartCoroutine(SimulatePreGame());
    }

    private IEnumerator SimulatePreGame()
    {
        Debug.Log("Game begins in:");
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1.0f);
        }

        AdvanceToNextState();
    }

    public override void Exit()
    {
    }
}
