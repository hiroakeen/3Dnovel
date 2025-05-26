public enum GameTurnState
{
    TalkPhase,
    MemoryPhase,
    EndingPhase
}

public interface ITurnState
{
    void OnStateEnter();
    public void OnStateExit() { }
    public void NotifyCharacterTalked(CharacterMemoryData character) { }
    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }
    public void NotifyTalkFinished(CharacterMemoryData character) { } 

}
