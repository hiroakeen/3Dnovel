using System.Collections.Generic;
using UnityEngine;
public class TurnState_TalkPhase : ITurnState
{
    private int talkedCount = 0;
    private CharacterMemoryData lastTalkedCharacter;

    public void OnStateEnter()
    {
        talkedCount = 0;
        UIManager.Instance.ShowTurnMessage($"ターン {GameManager.CurrentTurn}：3人に話しかけよう");
    }

    public void NotifyCharacterTalked(CharacterMemoryData character)
    {
        talkedCount++;
        lastTalkedCharacter = character;
        Debug.Log($"[TalkPhase] 話しかけた人数: {talkedCount}");
    }

    public void NotifyTalkFinished(CharacterMemoryData character)
    {
        // 最後に話しかけたキャラと同じなら、神の声を表示（ダブり防止）
        if (talkedCount >= 3 && character == lastTalkedCharacter)
        {
            UIManager.Instance.ShowNarration(
                "神の声：手に入れた記憶を渡す時間だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }

    public void OnStateExit() { }
    public void NotifyMemoryUsed(CharacterMemoryData from, CharacterMemoryData to) { }
}

