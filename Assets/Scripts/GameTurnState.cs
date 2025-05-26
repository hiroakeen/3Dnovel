using System.Collections.Generic;

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
    void NotifyCharacterTalked(CharacterDataJson character);
    void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to);
    void NotifyTalkFinished(CharacterDataJson character);
}
