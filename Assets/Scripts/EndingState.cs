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

        // �G���f�B���O����ێ�
        EndingResultHolder.endingId = endingId;

        // �G���f�B���OID�ɉ����ăV�[����؂�ւ�
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
        Debug.Log("EndingState �I��");
    }
}
