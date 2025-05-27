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

        // 内部状態をリセット
        GameTurnStateManager.Instance.ResetTalkPhaseState();

        // 🔄 UI側の記憶ナレーションフラグもリセット（重要）
        UIManager.Instance.ResetMemoryNarrationFlag();

        // 最大ターンを超えたら終了
        if (nextTurn > maxTurn)
        {
            Debug.Log("[ターン進行] 最終ターンを超えたためエンディングへ");
            GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
            return;
        }

        // ナレーション経由で TalkPhase に戻す
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
