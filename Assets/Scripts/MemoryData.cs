using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string memoryId;
    [TextArea] public string memoryText;
    public Sprite memoryImage;

    [Header("‚±‚Ì‹L‰¯‚Ì‚¿å")]
    public CharacterMemoryData ownerCharacter;
}
