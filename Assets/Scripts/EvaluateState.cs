using UnityEngine;
public class EvaluateState : IGameState
{
    private GameStateManager manager;

    public EvaluateState(GameStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("EvaluateState: 選択ログからエンディングを評価中");

        // DecisionLogなどを元に分岐判定
        // EndingStateにエンドIDを渡して遷移
        manager.ChangeState(new EndingState(manager, "END_A"));
    }

    public void Exit()
    {
        Debug.Log("EvaluateState 終了");
    }
}
