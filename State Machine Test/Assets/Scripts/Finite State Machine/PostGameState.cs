using System.Collections;
using UnityEngine;

public class PostGameState : State
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        StartCoroutine(SimulatePostGame());
    }

    private IEnumerator SimulatePostGame()
    {
        Debug.Log("Game ends in:");
        for (int i = 2; i > 0; i--)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(5.0f);
        AdvanceToNextState();
    }

    public override void Exit()
    {
    }
}
