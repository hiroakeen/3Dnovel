using System.Collections.Generic;
using UnityEngine;

public class GameTurnStateManager : MonoBehaviour
{
    public static GameTurnStateManager Instance { get; private set; }

    private ITurnState currentState;
    private GameTurnState currentPhase;

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
                ResetMemoryGivenTracking(); // 🧠 新仕様：記憶渡し履歴をリセット
                break;
            case GameTurnState.EndingPhase:
                currentState = new TurnState_EndingPhase();
                break;
        }

        currentState.OnStateEnter();
    }

    public ITurnState GetCurrentState() => currentState;
    public GameTurnState CurrentState => currentPhase;

    public void ResetTalkPhaseState()
    {
        if (currentState is TurnState_TalkPhase talkPhase)
        {
            talkPhase.ResetTalkLog();
        }
    }

    // ========================================
    // 🧠 新仕様：1ターンに5人へ記憶を渡すと自動進行
    // ========================================

    private HashSet<string> givenCharacterIdsThisTurn = new();

    public void RegisterMemoryGiven(string characterId)
    {
        if (givenCharacterIdsThisTurn.Contains(characterId)) return;

        givenCharacterIdsThisTurn.Add(characterId);

        Debug.Log($"[記憶使用] {characterId} に記憶を渡した（{givenCharacterIdsThisTurn.Count}/5）");

        if (givenCharacterIdsThisTurn.Count >= 5)
        {
            UIManager.Instance.ShowNarration(
                "謎の声：すべての者に記憶を渡し終えたようだ…次のターンに進もう。",
                () =>
                {
                    TurnFlowController.Instance.AdvanceToNextTurn();
                });
        }
    }

    public bool HasAlreadyReceivedMemory(string characterId)
    {
        return givenCharacterIdsThisTurn.Contains(characterId);
    }

    public void ResetMemoryGivenTracking()
    {
        givenCharacterIdsThisTurn.Clear();
    }
}
