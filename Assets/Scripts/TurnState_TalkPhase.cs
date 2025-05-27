using System.Collections.Generic;
using UnityEngine;

public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterNames = new();
    private bool narrationShown = false;
    private List<CharacterDataJson> talkedCharacters = new();

    public void OnStateEnter()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;
    }


    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        if (!talkedCharacters.Contains(character))
        {
            talkedCharacters.Add(character);
        }

        Debug.Log($"[TalkPhase] 話しかけた人数: {talkedCharacters.Count}");

        if (talkedCharacters.Count >= 3)
        {
            // ナレーション経由で遷移
            UIManager.Instance.ShowNarration(
                "謎の声：記憶を渡す時間だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }


    public void NotifyTalkFinished(CharacterDataJson character)
    {
        if (talkedCharacterNames.Count >= 3 && !narrationShown)
        {
            narrationShown = true;

            UIManager.Instance.ShowNarration(
                "謎の声：手に入れた記憶を渡す時間だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }

    public void ResetTalkLog()
    {
        talkedCharacters.Clear();
    }


    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
