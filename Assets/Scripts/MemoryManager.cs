using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance { get; private set; }

    private List<MemoryData> collectedMemories = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMemory(MemoryData memory)
    {
        if (!collectedMemories.Contains(memory))
            collectedMemories.Add(memory);
    }

    public List<MemoryData> GetCollectedMemories() => collectedMemories;
}
