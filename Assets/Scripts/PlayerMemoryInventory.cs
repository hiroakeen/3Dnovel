using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMemoryInventory : MonoBehaviour
{
    [Header("���݃v���C���[���������Ă���L��")]
    public List<MemoryData> currentMemories = new List<MemoryData>();

    [Header("���̃v���C���[�̃L����ID")]
    public string playerCharacterId;

    private List<MemoryData> usedMemories = new List<MemoryData>();

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
            usedMemories.Add(memory);
            Debug.Log($"�L���폜���L�^: {memory.memoryText}");
        }
    }

    public List<MemoryData> GetUsedMemories()
    {
        return usedMemories;
    }

    public List<MemoryData> GetAllMemories()
    {
        return currentMemories;
    }

    public MemoryData FindMemoryByText(string text)
    {
        return currentMemories.Find(m => m.memoryText == text);
    }

    public List<MemoryData> GetMemoriesForCurrentTurn()
    {
        int currentTurn = GameManager.Instance.GetTurn();
        return currentMemories.Where(m => m.autoGrantedTurn == currentTurn).ToList();
    }


}
