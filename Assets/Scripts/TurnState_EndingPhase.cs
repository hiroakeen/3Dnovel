using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("�yEndingPhase�z�]���J�n");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        // �����W�v
        bool usedYuukaMemory = false;
        bool usedOnCorrectTarget = false;
        int trueMemoryCount = 0;

        foreach (var log in logs)
        {
            if (log.selectedMemoryOwner.characterName == "�J�{ ���E�J")
            {
                usedYuukaMemory = true;
            }

            if (log.selectedMemoryOwner.isMemoryTrue && log.selectedMemoryOwner != log.targetCharacter)
            {
                trueMemoryCount++;
            }

            if (log.selectedMemoryOwner.characterName == "�e�I" && log.targetCharacter.characterName == "���V �J�Y�}")
            {
                usedOnCorrectTarget = true;
            }
        }

        // ���򔻒�iEvaluateState����ڐA�j
        string endingId;
        if (usedYuukaMemory && usedOnCorrectTarget && trueMemoryCount >= 2)
        {
            endingId = "TRUE_END";
        }
        else if (trueMemoryCount >= 2)
        {
            endingId = "GOOD_END";
        }
        else
        {
            endingId = "BAD_END";
        }

        Debug.Log($"[�]������] Ending ID: {endingId}");

        // �i���[�V����������ŃG���f�B���O��
        UIManager.Instance.ShowNarration(
            GetNarrationForEnding(endingId),
            onComplete: () =>
            {
                EndingManager.Instance.LoadEndingScene(endingId);
            }
        );
    }

    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterMemoryData character) { }

    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }

    private string GetNarrationForEnding(string id)
    {
        return id switch
        {
            "TRUE_END" => "�_�̐��F���ׂĂ̐^�����A����������ɓ͂����B",
            "GOOD_END" => "�_�̐��F�������̐^�����A���E�����������~�����B",
            "BAD_END" => "�_�̐��F�^���͓`��炸�A�N���C�Â��Ȃ��܂܎��͗��ꂽ�B",
            _ => "�_�̐��F����͐Â��ɖ�������B"
        };
    }
    public void NotifyTalkFinished(CharacterMemoryData character) { }

}
