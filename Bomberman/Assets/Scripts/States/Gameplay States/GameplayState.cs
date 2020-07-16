using System.Collections;
using UnityEngine;

public class GameplayState : State
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
        Debug.Log("Playing the game.");

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Game över.");
        AdvanceToNextState();
    }

    public override void Exit()
    {
    }
}
