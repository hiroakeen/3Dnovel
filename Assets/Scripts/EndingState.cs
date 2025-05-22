using UnityEngine;
public class EndingState : IGameState
{
    private GameStateManager manager;
    private string endingId;

    public EndingState(GameStateManager manager, string endingId)
    {
        this.manager = manager;
        this.endingId = endingId;
    }

    public void Enter()
    {
        Debug.Log($"EndingState: 結末 {endingId} を表示中");
        // EndingDataを参照して UI に表示
    }

    public void Exit()
    {
        Debug.Log("EndingState 終了");
    }
}
