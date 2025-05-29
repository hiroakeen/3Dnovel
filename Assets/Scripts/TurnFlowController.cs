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

        GameTurnStateManager.Instance.ResetTalkPhaseState();
        NarrationPlayer.Instance.ResetMemoryNarrationFlag();

        if (nextTurn > maxTurn)
        {
            string endingType = MemoryManager.Instance.GetEndingResultType();
            GameManager.Instance.SetEndingType(endingType);

            GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
            return;
        }

        GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
    }



    public void ForceToEnding()
    {
        GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
    }
}
