using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string id;
    [TextArea] public string memoryText;
    public Sprite memoryImage;

    [Header("この記憶の持ち主（キャラID）")]
    public string ownerCharacterId;

    [HideInInspector]
    public CharacterDataJson ownerCharacter; // ← 残すならここは使ってもOK

    // 必要なときにキャラデータを取得する関数
    public CharacterDataJson GetOwnerCharacter()
    {
        return GameManager.Instance?.FindCharacterById(ownerCharacterId);
    }
}
