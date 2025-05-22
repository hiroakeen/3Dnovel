using UnityEngine;
public class TurnState : IGameState
{
    private GameStateManager manager;
    private int turnNumber;

    public TurnState(GameStateManager manager, int turnNumber)
    {
        this.manager = manager;
        this.turnNumber = turnNumber;
    }

    public void Enter()
    {
        Debug.Log($"Turn {turnNumber} �J�n�F3�l�Ɖ�b��1�l�ɋL���g�p");
        // UI��GameManager�ł��̃^�[���̑I���E��b�𐧌�
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} �I��");
    }

    public void OnTurnFinished()
    {
        if (turnNumber >= 3)
        {
            manager.ChangeState(new EvaluateState(manager));
        }
        else
        {
            manager.ChangeState(new TurnState(manager, turnNumber + 1));
        }
    }
}
