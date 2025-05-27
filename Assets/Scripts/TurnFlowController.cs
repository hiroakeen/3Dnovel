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

        Debug.Log($"ターンが進行しました: {nextTurn}");

        if (nextTurn > maxTurn)
        {
            GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
        }
        else
        {
            GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
        }
    }

    public void ForceToEnding()
    {
        GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
    }
}
