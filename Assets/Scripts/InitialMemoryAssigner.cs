using System.Collections.Generic;
using UnityEngine;

public class InitialMemoryAssigner : MonoBehaviour
{
    [Header("初期記憶の数")]
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
        var playerCharacter = inventory.PlayerCharacterData;
        if (playerCharacter == null)
        {
            Debug.LogWarning("❗ PlayerCharacterData が設定されていません。");
            return;
        }

        // Resources や Addressables でも可（すべての MemoryData を取得）
        MemoryData[] allMemories = Resources.LoadAll<MemoryData>("ScriptableObjects/MemoryData");

        // プレイヤーの記憶だけを抽出
        List<MemoryData> playerMemories = new List<MemoryData>();
        foreach (var memory in allMemories)
        {
            if (memory.ownerCharacter == playerCharacter)
            {
                playerMemories.Add(memory);
            }
        }

        if (playerMemories.Count == 0)
        {
            Debug.LogWarning("⚠ プレイヤーの記憶が1つも見つかりませんでした。");
            return;
        }

        // ランダムに抽出
        int count = Mathf.Min(numberOfMemoriesToAssign, playerMemories.Count);
        List<MemoryData> selected = new List<MemoryData>();
        List<MemoryData> tempPool = new List<MemoryData>(playerMemories);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, tempPool.Count);
            selected.Add(tempPool[index]);
            tempPool.RemoveAt(index);
        }

        // 登録
        foreach (var memory in selected)
        {
            inventory.AddMemory(memory);
        }

        Debug.Log($"プレイヤーの記憶を {selected.Count} 個追加しました。");
    }
}
