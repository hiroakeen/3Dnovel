using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData : ScriptableObject
{
    public string memoryId; // ��ӂ�ID�i�ۑ��ȂǂɎg���z��j
    [TextArea]
    public string memoryText; // �\���p�̃e�L�X�g
    public Sprite memoryImage; // UI�\���p�̉摜

    [Header("���̋L���̎�����")]
    public CharacterMemoryData ownerCharacter; // �N�Ɋ֘A�Â��L����
}
