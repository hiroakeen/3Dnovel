using UnityEngine;

public enum GameTurnState { TalkPhase, MemoryPhase }

public class GameTurnStateManager : MonoBehaviour
{
    public static GameTurnStateManager Instance { get; private set; }
    public GameTurnState CurrentState { get; private set; } = GameTurnState.TalkPhase;

    private int talkCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterTalk()
    {
        talkCount++;
        if (talkCount >= 3)
            CurrentState = GameTurnState.MemoryPhase;
    }
}
