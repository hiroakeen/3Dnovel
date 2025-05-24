using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMemoryData", menuName = "Story/Character Memory Data")]
public class CharacterMemoryData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    [TextArea] public string personalityDescription;

    [Header("記憶と嘘設定")]
    [TextArea] public string memoryFragment;
    public bool isMemoryTrue;
    public bool isLying;
    public CharacterRoleType roleType;
    public MemoryData autoGrantedMemory;

    [Header("記憶使用の対象かどうか")]
    public bool isMemoryUseTarget;

    [Header("ターン別セリフ（最大3ターン）")]
    public List<TurnDialogue> turnDialogues = new();

    public string GetDialogueForTurn(int turn)
    {
        foreach (var dialogue in turnDialogues)
        {
            if (dialogue.turn == turn)
                return dialogue.dialogueLine;
        }
        return "……（無言）";
    }

    public string GetDialogueForCurrentTurn()
    {
        int turn = GameManager.CurrentTurn;
        return GetDialogueForTurn(turn);
    }
}

[System.Serializable]
public class TurnDialogue
{
    public int turn; // 1〜3
    [TextArea] public string dialogueLine;
}

public enum CharacterRoleType
{
    Investigate,
    Persuade,
    Trigger,
    Unknown
}
