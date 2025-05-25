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
        Debug.Log($"EndingState: �G���f�B���O {endingId} ��\����");

        // �G���f�B���O����n���ăV�[���J��
        EndingResultHolder.endingId = endingId;
        SceneManager.LoadScene("EndingScene");
    }

    public void Exit()
    {
        Debug.Log("EndingState �I��");
    }
}
