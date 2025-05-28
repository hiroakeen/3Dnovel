using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L���̕]���ɉ����� TRUE / GOOD / BAD �G���f�B���O�𔻒�i�����Łj
/// </summary>
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
            switch (log.targetCharacter.memoryReactionType)
            {
                case MemoryReactionType.True:
                    trueCount++;
                    break;
                case MemoryReactionType.Good:
                    goodCount++;
                    break;
            }
        }

        int totalCorrect = trueCount + goodCount;

        string endingId;
        if (trueCount == 3)
        {
            endingId = "TRUE_END"; // ���S�~�o�i�^�G���h�j
        }
        else if (totalCorrect >= 2)
        {
            endingId = "GOOD_END"; 
        }
        else
        {
            endingId = "BAD_END"; // �S��
        }

        Debug.Log($"[�]������] Ending ID: {endingId} (True: {trueCount}, Good: {goodCount}, Total: {totalCorrect})");

        NarrationPlayer.Instance.PlayNarration(
            GetNarrationForEnding(endingId),
            onComplete: () => EndingManager.Instance.LoadEndingScene(endingId)
        );
    }

    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterDataJson character) { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }

    public void NotifyTalkFinished(CharacterDataJson character) { }

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
