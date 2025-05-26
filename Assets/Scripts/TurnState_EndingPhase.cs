// TurnState_EndingPhase：記憶の評価に応じてTRUE／GOOD／BADエンディングを判定

using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("【EndingPhase】評価開始");

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
            endingId = "TRUE_END"; // 全員脱出
        }
        else if (goodCount >= 1 || trueCount >= 1)
        {
            endingId = "GOOD_END"; // プレイヤー or 誰か1人と脱出
        }
        else
        {
            endingId = "BAD_END"; // 全滅
        }

        Debug.Log($"[評価結果] Ending ID: {endingId} (True: {trueCount}, Good: {goodCount})");

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
            "TRUE_END" => "神の声：すべての記憶が正しい者へ届いた。希望は共有された。扉が開かれる…",
            "GOOD_END" => "神の声：記憶は一部に伝わった。救いは完全ではなかったが、わずかに道が残された。",
            "BAD_END" => "神の声：記憶は届かず、闇はそのまま続いた。誰も真実を知らないまま、終わりが訪れた。",
            _ => "神の声：物語は静かに幕を閉じた。"
        };
    }
}