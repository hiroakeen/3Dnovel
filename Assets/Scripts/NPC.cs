using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private CharacterDataJson characterData;

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

        // 状態を取得（現在のITurnState）
        var turnState = GameTurnStateManager.Instance.GetCurrentState();

        if (!string.IsNullOrEmpty(expectedId) && memory.id == expectedId)
        {
            // 正解！リアクション
            UIManager.Instance.ShowDialogue($"{npcName} に正しい記憶を渡した！");

            inventory?.RemoveMemory(memory);

            // ownerCharacterId ではなく ownerCharacter を使う
            var ownerCharacter = memory.ownerCharacter;

            if (ownerCharacter != null)
            {
                turnState?.NotifyMemoryUsed(ownerCharacter, characterData);
            }
            else
            {
                Debug.LogError("MemoryData の ownerCharacter が null です");
            }

        }
        else
        {
            // 不正解（記録なし、リアクションのみ）
            UIManager.Instance.ShowDialogue($"{npcName} は記憶を受け取ったが、反応がない……");

            // 任意でログ記録も可能
            // GameManager.Instance.AddDecisionLog(new TurnDecision(GameManager.CurrentTurn, memory.ownerCharacter, characterData));
        }
    }

    public CharacterDataJson GetCharacterData()
    {
        return characterData;
    }
}
