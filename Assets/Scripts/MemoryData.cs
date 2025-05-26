using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string id;

    [TextArea] public string memoryText;
    public Sprite memoryImage;

    [Header("���̋L���̎�����i�L����ID�j")]
    public string ownerCharacterId;
    public CharacterDataJson ownerCharacter;  // �L�����Q��


}
