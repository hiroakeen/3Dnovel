using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string id;
    [TextArea] public string memoryText;
    public string ownerCharacterId;
    public string correctReceiverCharacterId;

    public Sprite memoryImage;

    public CharacterDataJson ownerCharacter;

    public bool IsCorrectReceiver(string targetCharacterId)
    {
        return targetCharacterId == correctReceiverCharacterId;
    }

    public CharacterDataJson GetOwnerCharacter()
    {
        return GameManager.Instance?.FindCharacterById(ownerCharacterId);
    }
}
