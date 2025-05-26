using UnityEngine;

public class TurnState_MemoryPhase : ITurnState
{
    private int useCount = 0;

    public void OnStateEnter()
    {
        useCount = 0; // �O�̂��߃��Z�b�g
        UIManager.Instance.ShowTurnMessage($"�^�[�� {GameManager.CurrentTurn}�F�L����n�����i3��܂Łj");
    }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to)
    {
        useCount++;
        Debug.Log($"[MemoryPhase] �L���g�p: {from.name} �� {to.name}�i{useCount}/3�j");

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

    public void NotifyCharacterTalked(CharacterDataJson character) { }

    public void NotifyTalkFinished(CharacterDataJson character) { }
}
