using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private CharacterDataJson characterData;
    [SerializeField] private TalkTrigger talkTrigger;

    public void ReceiveMemory(MemoryData memory)
    {
        if (characterData == null || memory == null)
        {
            Debug.LogWarning("NPCまたは記憶データが null です");
            return;
        }

        string npcName = characterData.name;
        string expectedId = characterData.expectedMemoryId;

        // プレイヤーの記憶を取得
        var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
        var turnState = GameTurnStateManager.Instance.GetCurrentState();

        // ✅ 記憶は常に削除（正解不正解問わず）
        inventory?.RemoveMemory(memory);

        // ✅ 正解判定：expectedMemoryId による比較
        if (!string.IsNullOrEmpty(expectedId) && memory.id == expectedId)
        {
            // 正解リアクション
            UIManager.Instance.ShowDialogue($"{npcName} に正しい記憶を渡した！");

            var ownerCharacter = memory.originalOwner;
            if (ownerCharacter != null && turnState != null)
            {
                turnState.NotifyMemoryUsed(ownerCharacter, characterData, memory);
            }
        }
        else
        {
            // 不正解リアクション
            UIManager.Instance.ShowDialogue($"{npcName} は記憶を受け取ったが、反応がない……");
        }
    }

    public CharacterDataJson GetCharacterData()
    {
        return talkTrigger != null ? talkTrigger.GetCharacterData() : null;
    }
}
