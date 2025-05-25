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

        // エンディング情報を渡してシーン遷移
        EndingResultHolder.endingId = endingId;
        SceneManager.LoadScene("EndingScene");
    }

    public void Exit()
    {
        Debug.Log("EndingState 終了");
    }
}
