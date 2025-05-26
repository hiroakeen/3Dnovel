public enum GameTurnState
{
    TalkPhase,
    MemoryPhase,
    EndingPhase
}

public interface ITurnState
{
    void OnStateEnter();
    void OnStateExit();
    void NotifyCharacterTalked(CharacterMemoryData character);
    void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to);
    void NotifyTalkFinished(CharacterMemoryData character);
}
