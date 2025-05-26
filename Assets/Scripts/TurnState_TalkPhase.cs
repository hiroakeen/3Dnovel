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

    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        if (talkedCharacterNames.Contains(character.name)) return;

        talkedCharacterNames.Add(character.name);
        Debug.Log($"[TalkPhase] {character.name} �ɘb���������i{talkedCharacterNames.Count}/3�j");
    }

    public void NotifyTalkFinished(CharacterDataJson character)
    {
        if (talkedCharacterNames.Count >= 3 && !narrationShown)
        {
            narrationShown = true;

            UIManager.Instance.ShowNarration(
                "��̐��F��ɓ��ꂽ�L����n�����Ԃ��B",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }

    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
