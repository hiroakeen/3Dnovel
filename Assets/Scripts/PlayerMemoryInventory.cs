using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryInventory : MonoBehaviour
{
    [Header("���݃v���C���[���������Ă���L��")]
    public List<MemoryData> currentMemories = new List<MemoryData>();

    public void AddMemory(MemoryData memory)
    {
        if (!currentMemories.Contains(memory))
        {
            currentMemories.Add(memory);
            Debug.Log($"�L���ǉ�: {memory.memoryText}");
        }
    }

    public void RemoveMemory(MemoryData memory)
    {
        if (currentMemories.Contains(memory))
        {
            currentMemories.Remove(memory);
            Debug.Log($"�L���폜: {memory.memoryText}");
        }
    }

    public List<MemoryData> GetAllMemories()
    {
        return new List<MemoryData>(currentMemories);
    }
}
