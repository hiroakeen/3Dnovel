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
        Debug.Log($"Turn {turnNumber} 開始：3人と会話→1人に記憶使用");
        // UIやGameManagerでこのターンの選択・会話を制御
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} 終了");
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
