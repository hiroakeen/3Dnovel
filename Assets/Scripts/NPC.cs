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

        // プレイヤーの記憶を取得
        var inventory = FindAnyObjectByType<PlayerMemoryInventory>();

        // 状態を取得（現在のITurnState）
        var turnState = GameTurnStateManager.Instance.GetCurrentState();

        if (expected != null && memory == expected)
        {
            // 正解！リアクション
            UIManager.Instance.ShowDialogue($"{npcName} に正しい記憶を渡した！");

            inventory?.RemoveMemory(memory);

            turnState?.NotifyMemoryUsed(memory.ownerCharacter, characterData);
        }
        else
        {
            // 不正解（記録なし、リアクションのみ）
            UIManager.Instance.ShowDialogue($"{npcName} は記憶を受け取ったが、反応がない……");

            // 必要に応じてここで false でもログ記録したければ以下を呼ぶ（要件次第）
            // GameManager.Instance.AddDecisionLog(new TurnDecision(GameManager.CurrentTurn, memory.ownerCharacter, characterData));
        }
    }
    public CharacterMemoryData GetCharacterData()
    {
        return characterData;
    }

}
