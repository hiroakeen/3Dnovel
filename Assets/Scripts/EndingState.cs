using UnityEngine;
using UnityEngine.SceneManagement;

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

        // エンディング情報を保持
        EndingResultHolder.endingId = endingId;

        // エンディングIDに応じてシーンを切り替え
        string sceneName = endingId switch
        {
            "TRUE_END" => "TrueEndingScene",
            "GOOD_END" => "NormalEndingScene",
            _ => "BadEndingScene"
        };

        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Debug.Log("EndingState 終了");
    }
}
