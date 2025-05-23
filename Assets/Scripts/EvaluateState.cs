using System.Collections.Generic;
using UnityEngine;

public class EvaluateState : IGameState
{
    private GameStateManager manager;

    public EvaluateState(GameStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("EvaluateState: ログを評価してエンディングを決定");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        // 仮の分岐条件例（あとでSO化可能）
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

        // 分岐判定
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

        manager.ChangeState(new EndingState(manager, endingId));
    }

    public void Exit()
    {
        Debug.Log("EvaluateState 終了");
    }
}
