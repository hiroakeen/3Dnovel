using System.Collections.Generic;
using UnityEngine;

public class InitialMemoryAssigner : MonoBehaviour
{
    [Header("初期記憶候補リスト")]
    [SerializeField] private List<MemoryData> memoryPool;
    [SerializeField] private int numberOfMemoriesToAssign = 2;

    private void Start()
    {
        var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
        if (inventory != null)
        {
            AssignInitialMemories(inventory);
        }
    }

    private void AssignInitialMemories(PlayerMemoryInventory inventory)
    {
        List<MemoryData> selected = new List<MemoryData>();

        int count = Mathf.Min(numberOfMemoriesToAssign, memoryPool.Count);
        List<MemoryData> tempPool = new List<MemoryData>(memoryPool);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            selected.Add(tempPool[index]);
            tempPool.RemoveAt(index);
        }

        foreach (var memory in selected)
        {
            inventory.AddMemory(memory);
        }

        Debug.Log($"初期記憶を {selected.Count} 個追加しました。");
    }
}
