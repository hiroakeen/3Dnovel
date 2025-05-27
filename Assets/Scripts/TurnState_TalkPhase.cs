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

        Debug.Log($"[TalkPhase] �b���������l��: {talkedCharacters.Count}");

        if (talkedCharacters.Count >= 3)
        {
            // �i���[�V�����o�R�őJ��
            UIManager.Instance.ShowNarration(
                "��̐��F�L����n�����Ԃ��B",
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
                "��̐��F��ɓ��ꂽ�L����n�����Ԃ��B",
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
