using UnityEngine;

/// <summary>
/// 記憶フェーズ：全員に記憶を渡し終えたら次のターンへ進行（最終ターンならエンディングへ）
/// </summary>
public class TurnState_MemoryPhase : ITurnState
{
    private int memoryUsedCount = 0;
    private int totalNPCs;

    public void OnStateEnter()
    {
        memoryUsedCount = 0;

        // 現在ターン取得
        int currentTurn = GameManager.Instance.GetTurn();

        // 対象NPC数取得
        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        Debug.Log($"【MemoryPhase】開始：記憶を {totalNPCs} 人に渡す");

        UIManager.Instance.ShowNarration(
            "謎の声：集めた記憶を誰に渡す？すべてが繋がれば希望は現れる。",
            onComplete: null
        );

        UIManager.Instance.ShowTurnMessage($"ターン {currentTurn}：記憶を渡す（{totalNPCs}回まで）");
    }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to)
    {
        memoryUsedCount++;
        Debug.Log($"[MemoryPhase] 記憶使用: {from.name} → {to.name}（{memoryUsedCount}/{totalNPCs}）");

        GameManager.Instance.AddDecisionLog(new TurnDecision(GameManager.Instance.GetTurn(), from, to));

        // 全員に記憶を渡したら次へ
        if (memoryUsedCount >= totalNPCs)
        {
            int currentTurn = GameManager.Instance.GetTurn();

            if (currentTurn >= 3)
            {
                UIManager.Instance.ShowNarration("神の声：最後の記憶が渡された。結末を迎える時だ。", () =>
                {
                    GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
                });
            }
            else
            {
                GameManager.Instance.IncrementTurn(); // ターンを進める
                GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase); // 次のターンへ
            }
        }
    }

    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterDataJson character) { }

    public void NotifyTalkFinished(CharacterDataJson character) { }
}
