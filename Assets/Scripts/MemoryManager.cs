using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance { get; private set; }

    private List<MemoryData> collectedMemories = new();
    private List<MemoryData> allMemories = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        MemoryData[] loaded = Resources.LoadAll<MemoryData>("MemoryDataFolder"); // ���ۂ̃p�X�ɍ��킹�Ă�������
        allMemories.AddRange(loaded);
    }

    public void AddMemory(MemoryData memory)
    {
        if (!collectedMemories.Contains(memory))
            collectedMemories.Add(memory);
    }

    public List<MemoryData> GetCollectedMemories() => collectedMemories;

    public MemoryData FindMemoryById(string id)
    {
        return allMemories.Find(m => m.id == id);
    }
}
