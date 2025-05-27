using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話フェーズ：3人と会話し終えたら記憶フェーズへ移行
/// </summary>
public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterNames = new();
    private bool narrationShown = false;

    public void OnStateEnter()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;

        Debug.Log("【TalkPhase】開始：プレイヤーは3人と話すことができます。");
        UIManager.Instance.SetTurnMessage($"第{GameManager.Instance.GetTurn()}ターン：誰に話しかける？");
    }

    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        // 話しかけたキャラのIDを記録（重複防止）
        if (!talkedCharacterNames.Contains(character.id))
        {
            talkedCharacterNames.Add(character.id);
            Debug.Log($"[TalkPhase] 話しかけた: {character.name}（合計: {talkedCharacterNames.Count}人）");
        }
    }

    public void NotifyTalkFinished(CharacterDataJson character)
    {
        // 会話終了後、3人目ならナレーションを表示して記憶フェーズへ遷移
        if (talkedCharacterNames.Count >= 3 && !narrationShown)
        {
            narrationShown = true;

            UIManager.Instance.ShowNarration(
                "謎の声：手に入れた記憶を渡す時間だ。",
                () =>
                {
                    UIManager.Instance.SetTurnMessage("ひとりを選んで、記憶を渡そう！");
                    GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase);
                });
        }
    }

    public void ResetTalkLog()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;
    }

    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
