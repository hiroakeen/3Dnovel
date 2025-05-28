using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話フェーズ：全員と会話し終えたら記憶フェーズへ移行
/// </summary>
public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterIds = new();
    private bool narrationShown = false;
    private int totalNPCs;

    public void OnStateEnter()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;

        int currentTurn = GameManager.Instance.GetTurn();
        Debug.Log($"【TalkPhase】開始：第{currentTurn}ターン、プレイヤーは全員と話すことができます。");

        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        NarrationPlayer.Instance.PlayNarration(
            $"謎の声：第{currentTurn}ターン、記憶を集める時間だ。",
            null
        );
    }

    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        if (talkedCharacterIds.Add(character.id))
        {
            Debug.Log($"[TalkPhase] 話しかけた: {character.name}（{talkedCharacterIds.Count}/{totalNPCs}）");
        }
    }

    public void NotifyTalkFinished(CharacterDataJson character)
    {
        Debug.Log($"[TalkFinished] {character.name} との会話終了");

        // 全員と話したら記憶フェーズへ移行
        Debug.Log($"現在の会話人数: {talkedCharacterIds.Count}/{totalNPCs}");

        if (talkedCharacterIds.Count >= totalNPCs && !narrationShown)
        {
            narrationShown = true;
            Debug.Log("[TalkPhase] ナレーション表示＆記憶フェーズへ");

            NarrationPlayer.Instance.PlayNarration(
                "謎の声：記憶は十分に集まった。次は誰に渡すか、決める時だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }
    }


    public void ResetTalkLog()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;
    }

    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
