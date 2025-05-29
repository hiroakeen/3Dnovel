using System.Collections.Generic;
using UnityEngine;

public class TurnState_EndingPhase : ITurnState
{
    public void OnStateEnter()
    {
        Debug.Log("【EndingPhase】評価開始");

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
            endingId = "TRUE_END"; // 全問正解
        }
        else if (correctCount >= 10)
        {
            endingId = "GOOD_END"; // 10問以上正解
        }
        else
        {
            endingId = "BAD_END"; // その他
        }

        Debug.Log($"[評価結果] Ending ID: {endingId} (Correct: {correctCount}/15)");

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
            "TRUE_END" => "神の声：すべての記憶が正しい者へ届いた。希望は共有された。扉が開かれる…",
            "GOOD_END" => "神の声：記憶は一部に伝わった。救いは完全ではなかったが、わずかに道が残された。",
            "BAD_END" => "神の声：記憶は届かず、闇はそのまま続いた。誰も真実を知らないまま、終わりが訪れた。",
            _ => "神の声：物語は静かに幕を閉じた。"
        };
    }
}
