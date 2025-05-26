using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMemoryData", menuName = "Story/Character Memory Data")]
public class CharacterMemoryData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    [TextArea] public string personalityDescription;

    [Header("記憶と嘘設定")]
    public LocalizedString memoryFragmentLocalized;
    public bool isMemoryTrue;
    public bool isLying;
    public CharacterRoleType roleType;
    public MemoryData autoGrantedMemory;
    public MemoryData expectedMemory;

    [Header("記憶使用の対象かどうか")]
    public bool isMemoryUseTarget;

    [Header("記憶を渡されたときのリアクション")]
    public MemoryReactionType memoryReactionType;
    public LocalizedString memoryReactionTrueLocalized;
    public LocalizedString memoryReactionSuccessLocalized;
    public LocalizedString memoryReactionFailLocalized;

    [Header("ターン別セリフ（最大3ターン）")]
    public List<LocalizedTurnDialogue> turnDialoguesLocalized = new();

    public string GetDialogueForTurn(int turn, Language lang)
    {
        foreach (var dialogue in turnDialoguesLocalized)
        {
            if (dialogue.turn == turn)
                return dialogue.dialogueLine.GetLocalized(lang);
        }
        return lang == Language.Japanese ? "……（無言）" : "...(silent)";
    }

    public string GetDialogueForCurrentTurn(Language lang)
    {
        int turn = GameManager.CurrentTurn;
        return GetDialogueForTurn(turn, lang);
    }

    public string GetReactionForMemoryResult(MemoryReactionType resultType, Language lang)
    {
        return resultType switch
        {
            MemoryReactionType.True => memoryReactionTrueLocalized.GetLocalized(lang),
            MemoryReactionType.Good => memoryReactionSuccessLocalized.GetLocalized(lang),
            MemoryReactionType.Bad => memoryReactionFailLocalized.GetLocalized(lang),
            _ => lang == Language.Japanese ? "……" : "..."
        };
    }
}

public enum CharacterRoleType
{
    Investigate,
    Persuade,
    Trigger,
    Unknown
}

public enum MemoryReactionType
{
    None,
    Bad,
    Good,
    True
}
