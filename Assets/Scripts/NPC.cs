using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private CharacterMemoryData characterData;

    public void ReceiveMemory(MemoryData memory)
    {
        if (characterData == null || memory == null)
        {
            Debug.LogWarning("NPCまたは記憶データが null です");
            return;
        }

        string npcName = characterData.characterName;
        var expected = characterData.expectedMemory;

        if (expected != null && memory == expected)
        {
            // 正解！
            UIManager.Instance.ShowDialogue($"{npcName} に正しい記憶を渡した！");

            // 記憶を削除
            var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
            inventory?.RemoveMemory(memory);

            // ターン終了を通知
            var turnState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
            turnState?.NotifyMemoryUsed(memory.ownerCharacter, characterData);
        }
        else
        {
            // 不正解：何も起きない（再挑戦可能）
            UIManager.Instance.ShowDialogue($"{npcName} は記憶を受け取ったが、反応がない……");

            // UIは自動的に閉じられてOK（MemoryGiveUIControllerがClose()を呼ぶ）
            // → ターンは継続、何もしない
        }
    }
}
