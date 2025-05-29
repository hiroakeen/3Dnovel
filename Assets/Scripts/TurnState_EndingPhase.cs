using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("�yEndingPhase�z�]���J�n");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        int correctCount = 0;

        foreach (var log in logs)
        {
            if (log.usedMemory.IsCorrectReceiver(log.targetCharacter.id))
            {
                correctCount++;
            }
        }

        string endingId;
        if (correctCount == 15)
        {
            endingId = "TRUE_END"; // �S�␳��
        }
        else if (correctCount >= 10)
        {
            endingId = "GOOD_END"; // 10��ȏ㐳��
        }
        else
        {
            endingId = "BAD_END"; // ���̑�
        }

        Debug.Log($"[�]������] Ending ID: {endingId} (Correct: {correctCount}/15)");

        NarrationPlayer.Instance.PlayNarration(
            GetNarrationForEnding(endingId),
            onComplete: () => EndingManager.Instance.LoadEndingScene("EndingScene")
        );
    }

    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterDataJson character) { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory) { }

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
