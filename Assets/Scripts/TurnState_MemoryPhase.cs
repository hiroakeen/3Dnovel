public class TurnState_MemoryPhase : ITurnState
{
    private int useCount = 0;

    public void OnStateEnter()
    {
        useCount = 0; // �� �O�̂��߃��Z�b�g
        UIManager.Instance.ShowTurnMessage($"�^�[�� {GameManager.CurrentTurn}�F�L����n�����i3��܂Łj");
    }

    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to)
    {
        useCount++;
        UnityEngine.Debug.Log($"[MemoryPhase] �L���g�p: {from.characterName} �� {to.characterName}�i{useCount}/3�j");

        GameManager.Instance.AddDecisionLog(new TurnDecision(GameManager.CurrentTurn, from, to));

        if (useCount >= 3)
        {
            if (GameManager.CurrentTurn >= 3)
            {
                UIManager.Instance.ShowNarration("�_�̐��F���ׂĂ̋L�����g��ꂽ�B", () =>
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
