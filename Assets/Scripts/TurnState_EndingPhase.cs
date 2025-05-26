// TurnState_EndingPhase�F�L���̕]���ɉ�����TRUE�^GOOD�^BAD�G���f�B���O�𔻒�

using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("�yEndingPhase�z�]���J�n");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        int trueCount = 0;
        int goodCount = 0;

        foreach (var log in logs)
        {
            if (log.targetCharacter.memoryReactionType == MemoryReactionType.True)
                trueCount++;
            else if (log.targetCharacter.memoryReactionType == MemoryReactionType.Good)
                goodCount++;
        }

        string endingId;
        if (trueCount >= 3)
        {
            endingId = "TRUE_END"; // �S���E�o
        }
        else if (goodCount >= 1 || trueCount >= 1)
        {
            endingId = "GOOD_END"; // �v���C���[ or �N��1�l�ƒE�o
        }
        else
        {
            endingId = "BAD_END"; // �S��
        }

        Debug.Log($"[�]������] Ending ID: {endingId} (True: {trueCount}, Good: {goodCount})");

        UIManager.Instance.ShowNarration(
            GetNarrationForEnding(endingId),
            onComplete: () => EndingManager.Instance.LoadEndingScene(endingId)
        );
    }

    public void OnStateExit() { }
    public void NotifyCharacterTalked(CharacterMemoryData character) { }
    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }
    public void NotifyTalkFinished(CharacterMemoryData character) { } 

    private string GetNarrationForEnding(string id)
    {
        return id switch
        {
            "TRUE_END" => "�_�̐��F���ׂĂ̋L�����������҂֓͂����B��]�͋��L���ꂽ�B�����J�����c",
            "GOOD_END" => "�_�̐��F�L���͈ꕔ�ɓ`������B�~���͊��S�ł͂Ȃ��������A�킸���ɓ����c���ꂽ�B",
            "BAD_END" => "�_�̐��F�L���͓͂����A�ł͂��̂܂ܑ������B�N���^����m��Ȃ��܂܁A�I��肪�K�ꂽ�B",
            _ => "�_�̐��F����͐Â��ɖ�������B"
        };
    }
}