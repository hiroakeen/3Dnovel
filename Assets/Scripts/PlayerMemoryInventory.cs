using System.Collections.Generic;
using UnityEngine;

public class PlayerMemoryInventory : MonoBehaviour
{
    [Header("現在プレイヤーが所持している記憶")]
    public List<MemoryData> currentMemories = new List<MemoryData>();
    [Header("このプレイヤーのキャラデータ")]
    public CharacterMemoryData PlayerCharacterData;
    public void AddMemory(MemoryData memory)
    {
        if (!currentMemories.Contains(memory))
        {
            currentMemories.Add(memory);
            Debug.Log($"記憶追加: {memory.memoryText}");
        }

    }

    private List<MemoryData> usedMemories = new List<MemoryData>();

    public void RemoveMemory(MemoryData memory)
    {
        if (currentMemories.Contains(memory))
        {
            currentMemories.Remove(memory);
            usedMemories.Add(memory); // ← 使用履歴に追加
            Debug.Log($"記憶削除＆記録: {memory.memoryText}");
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


}
