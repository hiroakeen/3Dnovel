using UnityEngine;

public class GameTurnStateManager : MonoBehaviour
{
    public static GameTurnStateManager Instance { get; private set; }

    private ITurnState currentState;
    private GameTurnState currentPhase;

    private bool hasUsedMemoryThisTurn = false; //このターンで記憶を使ったかどうか

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetState(GameTurnState.TalkPhase);
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
                hasUsedMemoryThisTurn = false; //記憶使用フラグをリセット
                break;
            case GameTurnState.EndingPhase:
                currentState = new TurnState_EndingPhase();
                break;
        }

        currentState.OnStateEnter();
    }

    public ITurnState GetCurrentState() => currentState;

    public GameTurnState CurrentState => currentPhase;

    // 使用可否を問う
    public bool CanUseMemoryThisTurn() => !hasUsedMemoryThisTurn;

    // 使用後にフラグを立てる
    public void SetMemoryUsedThisTurn()
    {
        hasUsedMemoryThisTurn = true;
    }

    public void ResetTalkPhaseState()
    {
        if (currentState is TurnState_TalkPhase talkPhase)
        {
            talkPhase.ResetTalkLog();
        }
    }

}
