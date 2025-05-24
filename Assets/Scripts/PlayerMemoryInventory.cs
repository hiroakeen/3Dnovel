using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryInventory : MonoBehaviour
{
    [Header("現在プレイヤーが所持している記憶")]
    public List<MemoryData> currentMemories = new List<MemoryData>();

    public void AddMemory(MemoryData memory)
    {
        if (!currentMemories.Contains(memory))
        {
            currentMemories.Add(memory);
            Debug.Log($"記憶追加: {memory.memoryText}");
        }
    }

    public void RemoveMemory(MemoryData memory)
    {
        if (currentMemories.Contains(memory))
        {
            currentMemories.Remove(memory);
            Debug.Log($"記憶削除: {memory.memoryText}");
        }
    }

    public List<MemoryData> GetAllMemories()
    {
        return new List<MemoryData>(currentMemories);
    }
}
