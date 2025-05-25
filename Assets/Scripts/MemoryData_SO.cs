using UnityEngine;

[CreateAssetMenu(menuName = "Memory/MemoryData")]
public class MemoryData_SO : ScriptableObject
{
    public string memoryId;
    public Sprite image;
    [TextArea] public string description;
}
