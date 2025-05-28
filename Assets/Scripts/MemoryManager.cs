using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance { get; private set; }

    private List<MemoryData> collectedMemories = new();
    private List<MemoryData> allMemories = new();

    // 新規追加: 渡した記憶とその相手の記録
    private List<MemoryUsageRecord> memoryUsageRecords = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        MemoryData[] loaded = Resources.LoadAll<MemoryData>("MemoryDataFolder"); // 実際のパスに合わせてください
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

    // ✅ 新規追加：記憶を誰に渡したか記録し、正解かどうか判定
    public void RecordMemoryUsage(MemoryData memory, string receiverCharacterId)
    {
        bool isCorrect = memory.IsCorrectReceiver(receiverCharacterId);
        memoryUsageRecords.Add(new MemoryUsageRecord(memory, receiverCharacterId, isCorrect));
        Debug.Log($"記憶 {memory.id} を {receiverCharacterId} に渡した → 正解: {isCorrect}");
    }

    // ✅ 最終評価用：正解数を取得
    public int GetCorrectAnswerCount()
    {
        int count = 0;
        foreach (var record in memoryUsageRecords)
        {
            if (record.isCorrect) count++;
        }
        return count;
    }

    // ✅ 最終評価用：全問のうち正解率チェックにも使える
    public int GetTotalAnsweredCount()
    {
        return memoryUsageRecords.Count;
    }

    // ✅ エンディング分岐ロジックでも呼べるユーティリティ
    public string GetEndingResultType()
    {
        int correct = GetCorrectAnswerCount();
        if (correct == 15) return "TrueEnding";
        if (correct >= 10) return "GoodEnding";
        return "BadEnding";
    }
}

// ✅ 使用履歴を保存する内部クラス
public class MemoryUsageRecord
{
    public MemoryData memory;
    public string receiverCharacterId;
    public bool isCorrect;

    public MemoryUsageRecord(MemoryData memory, string receiverCharacterId, bool isCorrect)
    {
        this.memory = memory;
        this.receiverCharacterId = receiverCharacterId;
        this.isCorrect = isCorrect;
    }
}
