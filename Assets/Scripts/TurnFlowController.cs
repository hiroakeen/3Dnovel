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
        // ターン数を進める
        int nextTurn = GameManager.Instance.GetTurn() + 1;
        GameManager.Instance.SetTurn(nextTurn);

        Debug.Log($"[ターン進行] → ターン {nextTurn}");

        // 終了条件のチェック
        if (nextTurn > maxTurn)
        {
            // 最終ターンを超えたら EndingPhase へ
            Debug.Log("🔚 最大ターンに達したため、エンディングへ移行します。");
            UIManager.Instance.ShowNarration(
                "謎の声：すべての記憶は語られた……。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase)
            );
            return;
        }

        // 内部状態のリセット（話しかけたキャラなど）
        GameTurnStateManager.Instance.ResetTalkPhaseState();

        // 次のターン開始（ナレーション経由で TalkPhase）
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
