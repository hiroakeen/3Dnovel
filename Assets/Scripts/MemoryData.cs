using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string memoryId; // 一意なID（保存などに使う想定）
    [TextArea]
    public string memoryText; // 表示用のテキスト
    public Sprite memoryImage; // UI表示用の画像

    [Header("この記憶の持ち主")]
    public CharacterMemoryData ownerCharacter; // 誰に関連づく記憶か
}
