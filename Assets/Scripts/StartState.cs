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
        Debug.Log("StartState: �Q�[���J�n����");
        // �^�C�g�����o��C���g�����I������玟��
        manager.ChangeState(new TurnState(manager, 1));
    }

    public void Exit()
    {
        Debug.Log("StartState�I��");
    }
}
