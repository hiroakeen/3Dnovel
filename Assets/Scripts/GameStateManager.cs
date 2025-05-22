using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private IGameState currentState;

    public void ChangeState(IGameState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
            currentState.Enter();
    }

    private void Start()
    {
        ChangeState(new StartState(this)); // Å‰‚Ìó‘Ô‚É‘JˆÚ
    }
}
