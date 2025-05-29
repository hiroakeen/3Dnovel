using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string id;
    [TextArea] public string memoryText;
    public string ownerCharacterId;
    public string correctReceiverCharacterId;
    public Sprite memoryImage;

    public CharacterDataJson originalOwner;

    public bool autoGrantedMemory = false;
    public int autoGrantedTurn = 0;


    public bool IsCorrectReceiver(string targetCharacterId) =>
        targetCharacterId == correctReceiverCharacterId;

    public CharacterDataJson GetOwnerCharacter()
    {
        return GameManager.Instance?.FindCharacterById(ownerCharacterId);
    }


}

