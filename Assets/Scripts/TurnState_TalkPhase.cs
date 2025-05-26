using System.Collections.Generic;
using UnityEngine;
public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterNames = new();
    private bool narrationShown = false;

    public void OnStateEnter()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;

        UIManager.Instance.SetTurnMessageByKeyWithTurn(TurnMessageKey.TurnStart, GameManager.CurrentTurn);

    }


    public void NotifyCharacterTalked(CharacterMemoryData character)
    {
        if (talkedCharacterNames.Contains(character.characterName)) return;

        talkedCharacterNames.Add(character.characterName);
        Debug.Log($"[TalkPhase] {character.characterName} に話しかけた（{talkedCharacterNames.Count}/3）");
    }

    public void NotifyTalkFinished(CharacterMemoryData character)
    {
        if (talkedCharacterNames.Count >= 3 && !narrationShown)
        {
            narrationShown = true;

            UIManager.Instance.ShowNarration(
                "神の声：手に入れた記憶を渡す時間だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }

    public void OnStateExit() { }
    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }
}
