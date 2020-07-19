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
        m_StateManager.SetGameInputActive(true);

        StartCoroutine(SimulatePreGame());
    }

    private IEnumerator SimulatePreGame()
    {
        Debug.Log("Playing the game.");

        yield return new WaitForSeconds(3.0f);

        Debug.Log("Game över.");
        AdvanceToNextState();
    }

    public override void Exit()
    {
        m_StateManager.SetGameInputActive(false);
    }
}
