using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string id;

    [TextArea] public string memoryText;
    public Sprite memoryImage;

    [Header("この記憶の持ち主（キャラID）")]
    public string ownerCharacterId;
    public CharacterDataJson ownerCharacter;  // キャラ参照


}
