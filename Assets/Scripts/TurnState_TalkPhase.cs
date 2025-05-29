using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��b�t�F�[�Y�F�S���Ƙb���I������L���t�F�[�Y�ֈڍs
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

        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        int currentTurn = GameManager.Instance.GetTurn();
        Debug.Log($"�yTalkPhase�z�J�n�F��{currentTurn}�^�[���A�v���C���[�͑S���Ƙb�����Ƃ��ł��܂��B");

        NarrationPlayer.Instance.PlayNarration(
            $"�L�����W�߂鎞�Ԃ��B",
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

        // �O�̂��߂����ł��ǉ�
        talkedCharacterIds.Add(character.id);

        if (talkedCharacterIds.Count >= totalNPCs && !narrationShown)
        {
            narrationShown = true;
            Debug.Log("[TalkPhase] �i���[�V�����\�����L���t�F�[�Y��");

            NarrationPlayer.Instance.PlayNarration(
                "�L���͏\���ɏW�܂����B���͒N�ɓn�����A���߂鎞���B",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }

        if (talkedCharacterIds.Count < totalNPCs)
        {
            var all = GameManager.Instance.GetAllCharacters();
            foreach (var c in all)
            {
                if (!talkedCharacterIds.Contains(c.id))
                    Debug.LogWarning($"����b�L����: {c.name}�iID: {c.id}�j");
            }
        }

    }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory) { }

    public void OnStateExit() { }

    public void ResetTalkLog()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;
    }
}
