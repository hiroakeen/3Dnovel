using UnityEngine;

public class NPC : MonoBehaviour
{
    public void ReceiveMemory(MemoryData_SO memory)
    {
        // 記憶が条件を満たしているか判定して分岐など
        if (memory.memoryId == expectedMemoryId)
        {
            Debug.Log("正しい記憶を受け取った！");
            // 好感度アップ・エンディングフラグ etc.
        }
        else
        {
            Debug.Log("合ってない記憶だった…");
            // 無反応・誤反応など
        }
    }

    public string expectedMemoryId; // 条件として使う記憶ID
}
