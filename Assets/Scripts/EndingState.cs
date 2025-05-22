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
        Debug.Log($"EndingState: ���� {endingId} ��\����");
        // EndingData���Q�Ƃ��� UI �ɕ\��
    }

    public void Exit()
    {
        Debug.Log("EndingState �I��");
    }
}
