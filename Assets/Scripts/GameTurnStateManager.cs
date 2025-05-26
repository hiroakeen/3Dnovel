using UnityEngine;

public class GameTurnStateManager : MonoBehaviour
{
    public static GameTurnStateManager Instance { get; private set; }

    private ITurnState currentState;
    private GameTurnState currentPhase; // �� �ǉ��F���݂̃t�F�[�Y��ێ�

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
    }

    public void SetState(GameTurnState newState)
    {
        currentState?.OnStateExit();

        currentPhase = newState; 

        switch (newState)
        {
            case GameTurnState.TalkPhase:
                currentState = new TurnState_TalkPhase();
                break;
            case GameTurnState.MemoryPhase:
                currentState = new TurnState_MemoryPhase();
                break;
            case GameTurnState.EndingPhase:
                currentState = new TurnState_EndingPhase();
                break;
        }

        currentState.OnStateEnter();
    }

    public ITurnState GetCurrentState() => currentState;

    // ���݂̗񋓃t�F�[�Y��Ԃ��v���p�e�B�iTalkTrigger ����g����j
    public GameTurnState CurrentState => currentPhase;
}
