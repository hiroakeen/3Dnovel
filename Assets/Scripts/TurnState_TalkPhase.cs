using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��b�t�F�[�Y�F�S���Ɖ�b���I������L���t�F�[�Y�ֈڍs
/// </summary>
public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterIds = new();
    private bool narrationShown = false;
    private int totalNPCs;

    public void OnStateEnter()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;

        int currentTurn = GameManager.Instance.GetTurn();
        Debug.Log($"�yTalkPhase�z�J�n�F��{currentTurn}�^�[���A�v���C���[�͑S���Ƙb�����Ƃ��ł��܂��B");

        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        NarrationPlayer.Instance.PlayNarration(
            $"��̐��F��{currentTurn}�^�[���A�L�����W�߂鎞�Ԃ��B",
            null
        );
    }

    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        if (talkedCharacterIds.Add(character.id))
        {
            Debug.Log($"[TalkPhase] �b��������: {character.name}�i{talkedCharacterIds.Count}/{totalNPCs}�j");
        }
    }

    public void NotifyTalkFinished(CharacterDataJson character)
    {
        Debug.Log($"[TalkFinished] {character.name} �Ƃ̉�b�I��");

        // �S���Ƙb������L���t�F�[�Y�ֈڍs
        Debug.Log($"���݂̉�b�l��: {talkedCharacterIds.Count}/{totalNPCs}");

        if (talkedCharacterIds.Count >= totalNPCs && !narrationShown)
        {
            narrationShown = true;
            Debug.Log("[TalkPhase] �i���[�V�����\�����L���t�F�[�Y��");

            NarrationPlayer.Instance.PlayNarration(
                "��̐��F�L���͏\���ɏW�܂����B���͒N�ɓn�����A���߂鎞���B",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }


    public void ResetTalkLog()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;
    }

    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
