public class TurnState_MemoryPhase : ITurnState
{
    private int useCount = 0;

    public void OnStateEnter()
    {
        useCount = 0; // ← 念のためリセット
        UIManager.Instance.ShowTurnMessage($"ターン {GameManager.CurrentTurn}：記憶を渡そう（3回まで）");
    }

    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to)
    {
        useCount++;
        UnityEngine.Debug.Log($"[MemoryPhase] 記憶使用: {from.characterName} → {to.characterName}（{useCount}/3）");

        GameManager.Instance.AddDecisionLog(new TurnDecision(GameManager.CurrentTurn, from, to));

        if (useCount >= 3)
        {
            if (GameManager.CurrentTurn >= 3)
            {
                UIManager.Instance.ShowNarration("神の声：すべての記憶が使われた。", () =>
                {
                    GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase);
                });
            }
            else
            {
                GameManager.Instance.SetTurn(GameManager.CurrentTurn + 1);
                GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
            }
        }
    }

    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterMemoryData character) { }
    public void NotifyTalkFinished(CharacterMemoryData character) { }

}
