using System.Collections.Generic;
using UnityEngine;
public class TurnState_TalkPhase : ITurnState
{
    private int talkedCount = 0;
    private CharacterMemoryData lastTalkedCharacter;

    public void OnStateEnter()
    {
        talkedCount = 0;
        UIManager.Instance.ShowTurnMessage($"�^�[�� {GameManager.CurrentTurn}�F3�l�ɘb�������悤");
    }

    public void NotifyCharacterTalked(CharacterMemoryData character)
    {
        talkedCount++;
        lastTalkedCharacter = character;
        Debug.Log($"[TalkPhase] �b���������l��: {talkedCount}");
    }

    public void NotifyTalkFinished(CharacterMemoryData character)
    {
        // �Ō�ɘb���������L�����Ɠ����Ȃ�A�_�̐���\���i�_�u��h�~�j
        if (talkedCount >= 3 && character == lastTalkedCharacter)
        {
            UIManager.Instance.ShowNarration(
                "�_�̐��F��ɓ��ꂽ�L����n�����Ԃ��B",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }

    public void OnStateExit() { }
    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }
}

