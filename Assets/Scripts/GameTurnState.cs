using System.Collections.Generic;

/// <summary>
/// ゲームの各ターンフェーズを表す列挙体。
/// </summary>
public enum GameTurnState
{
    TalkPhase,     // 会話フェーズ（記憶収集）
    MemoryPhase,   // 記憶使用フェーズ（5人に渡す）
    EndingPhase    // エンディング判定・演出
}

/// <summary>
/// 各ターン状態の共通インターフェース。
/// 状態ごとに処理内容を分離するために使用。
/// </summary>
public interface ITurnState
{
    /// <summary>状態に入ったときに呼ばれる</summary>
    void OnStateEnter();

    /// <summary>状態を離れるときに呼ばれる</summary>
    void OnStateExit();

    /// <summary>会話したキャラの通知（TalkPhase用）</summary>
    void NotifyCharacterTalked(CharacterDataJson character);

    /// <summary>記憶を渡した通知（MemoryPhase用）</summary>
    void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory);


    /// <summary>会話終了通知（ターン内で全員と話したかを管理）</summary>
    void NotifyTalkFinished(CharacterDataJson character);
}
