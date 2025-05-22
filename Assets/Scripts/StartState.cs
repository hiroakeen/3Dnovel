using UnityEngine;
public class StartState : IGameState
{
    private GameStateManager manager;

    public StartState(GameStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("StartState: ゲーム開始準備");
        // タイトル演出やイントロが終わったら次へ
        manager.ChangeState(new TurnState(manager, 1));
    }

    public void Exit()
    {
        Debug.Log("StartState終了");
    }
}
