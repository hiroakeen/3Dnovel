using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string memoryId;
    [TextArea] public string memoryText;
    public Sprite memoryImage;

    [Header("���̋L���̎�����")]
    public CharacterMemoryData ownerCharacter;
}
