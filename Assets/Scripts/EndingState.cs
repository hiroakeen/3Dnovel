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
        Debug.Log($"EndingState: エンディング {endingId} を表示中");

        // 仮データで表示（本番では ScriptableObject から参照）
        string title = "";
        string description = "";

        switch (endingId)
        {
            case "TRUE_END":
                title = "真実の脱出";
                description = "あなたは全ての嘘を見抜き、皆と共に部屋を出た。";
                break;
            case "GOOD_END":
                title = "一部脱出";
                description = "あなたは助かったが、他の誰かが犠牲になった。";
                break;
            case "BAD_END":
            default:
                title = "閉ざされた終焉";
                description = "何も知らないまま、あなたたちは閉じ込められた。";
                break;
        }

        UIManager.Instance.ShowEnding(title, description);
    }

    public void Exit()
    {
        Debug.Log("EndingState 終了");
    }
}