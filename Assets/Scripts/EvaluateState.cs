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
        Debug.Log("EvaluateState: �I�����O����G���f�B���O��]����");

        // DecisionLog�Ȃǂ����ɕ��򔻒�
        // EndingState�ɃG���hID��n���đJ��
        manager.ChangeState(new EndingState(manager, "END_A"));
    }

    public void Exit()
    {
        Debug.Log("EvaluateState �I��");
    }
}
