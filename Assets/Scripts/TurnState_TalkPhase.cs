using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 会話フェーズ：全員と話し終えたら記憶フェーズへ移行
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

        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        int currentTurn = GameManager.Instance.GetTurn();
        Debug.Log($"【TalkPhase】開始：第{currentTurn}ターン、プレイヤーは全員と話すことができます。");

        NarrationPlayer.Instance.PlayNarration(
            $"記憶を集める時間だ。",
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

        // 念のためここでも追加
        talkedCharacterIds.Add(character.id);

        if (talkedCharacterIds.Count >= totalNPCs && !narrationShown)
        {
            narrationShown = true;
            Debug.Log("[TalkPhase] ナレーション表示＆記憶フェーズへ");

            NarrationPlayer.Instance.PlayNarration(
                "記憶は十分に集まった。次は誰に渡すか、決める時だ。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase)
            );
        }

        if (talkedCharacterIds.Count < totalNPCs)
        {
            var all = GameManager.Instance.GetAllCharacters();
            foreach (var c in all)
            {
                if (!talkedCharacterIds.Contains(c.id))
                    Debug.LogWarning($"未会話キャラ: {c.name}（ID: {c.id}）");
            }
        }

    }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory) { }

    public void OnStateExit() { }

    public void ResetTalkLog()
    {
        talkedCharacterIds.Clear();
        narrationShown = false;
    }
}
