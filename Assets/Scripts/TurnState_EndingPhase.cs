using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("【EndingPhase】評価開始");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        // 条件集計
        bool usedYuukaMemory = false;
        bool usedOnCorrectTarget = false;
        int trueMemoryCount = 0;

        foreach (var log in logs)
        {
            if (log.selectedMemoryOwner.characterName == "雨宮 ユウカ")
            {
                usedYuukaMemory = true;
            }

            if (log.selectedMemoryOwner.isMemoryTrue && log.selectedMemoryOwner != log.targetCharacter)
            {
                trueMemoryCount++;
            }

            if (log.selectedMemoryOwner.characterName == "テオ" && log.targetCharacter.characterName == "黒澤 カズマ")
            {
                usedOnCorrectTarget = true;
            }
        }

        // 分岐判定（EvaluateStateから移植）
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

        Debug.Log($"[評価結果] Ending ID: {endingId}");

        // ナレーションを挟んでエンディングへ
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
            "TRUE_END" => "神の声：すべての真実が、正しい相手に届いた。",
            "GOOD_END" => "神の声：いくつかの真実が、世界を少しだけ救った。",
            "BAD_END" => "神の声：真実は伝わらず、誰も気づかないまま時は流れた。",
            _ => "神の声：物語は静かに幕を閉じた。"
        };
    }
    public void NotifyTalkFinished(CharacterMemoryData character) { }

}
