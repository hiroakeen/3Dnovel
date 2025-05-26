using UnityEngine;

public class GameTurnStateManager : MonoBehaviour
{
    public static GameTurnStateManager Instance { get; private set; }

    private ITurnState currentState;
    private GameTurnState currentPhase; // ★ 追加：現在のフェーズを保持

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

    // 現在の列挙フェーズを返すプロパティ（TalkTrigger から使える）
    public GameTurnState CurrentState => currentPhase;
}
