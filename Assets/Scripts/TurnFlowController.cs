using UnityEngine;

public class TurnFlowController : MonoBehaviour
{
    public static TurnFlowController Instance { get; private set; }

    [SerializeField] private int maxTurn = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 記憶使用後に呼ばれ、次のターンまたは終了へ遷移する
    /// </summary>
    public void AdvanceToNextTurn()
    {
        int nextTurn = GameManager.Instance.GetTurn() + 1;
        GameManager.Instance.SetTurn(nextTurn);

        Debug.Log($"[ターン進行] → ターン {nextTurn}");

        GameTurnStateManager.Instance.ResetTalkPhaseState();
        UIManager.Instance.ResetMemoryNarrationFlag();

        if (nextTurn > maxTurn)
        {
            Debug.Log("[ターン進行] 最終ターンを超えたためエンディングへ");

            // ✅ 正答数に応じたエンディングタイプをログ出力 or 保存
            string endingType = MemoryManager.Instance.GetEndingResultType();
            Debug.Log($"[エンディング判定] 結果: {endingType}");

            // 必要に応じて GameManager に渡すなどもOK
            GameManager.Instance.SetEndingType(endingType);

            GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
            return;
        }

        UIManager.Instance.ShowNarration(
            $"謎の声：{nextTurn}ターン目が始まる。",
            () => GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase)
        );
    }

    public void ForceToEnding()
    {
        GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
    }
}
