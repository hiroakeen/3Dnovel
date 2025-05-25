using UnityEngine;
using UnityEngine.UI;

public class TalkTrigger : MonoBehaviour
{
    [Header("共通のTalkボタン")]
    [SerializeField] private GameObject talkButton;
    [Header("このNPCのキャラデータ")]
    [SerializeField] private CharacterMemoryData characterData;

    private bool isPlayerNear = false;
    private Transform player;

    void Start()
    {
        if (talkButton != null)
        {
            talkButton.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetButtonDown("Submit"))
        {
            TalkToNPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isPlayerNear = true;

            if (talkButton != null)
            {
                talkButton.SetActive(true);
                Button btn = talkButton.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(TalkToNPC);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (talkButton != null) talkButton.SetActive(false);
        }
    }

    public void TalkToNPC()
    {
        if (talkButton != null) talkButton.SetActive(false);

        if (characterData == null)
        {
            Debug.LogWarning("characterData が設定されていません。");
            return;
        }

        string npcName = characterData.characterName;
        string dialogueLine = characterData.GetDialogueForCurrentTurn();
        bool isMemoryUseTarget = characterData.isMemoryUseTarget;
        MemoryData memoryToGrant = characterData.autoGrantedMemory;

        // 記憶の自動取得
        if (memoryToGrant != null)
        {
            var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）");

                NotifyTalked(); // ← TurnStateに通知を追加
                return;
            }
        }

        // 記憶を使う対象なら
        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}");
        }

        NotifyTalked(); // ← TurnStateに通知
    }

    public void UseMemory(string memoryContent)
    {
        string npcName = characterData != null ? characterData.characterName : "NPC";
        Debug.Log($"{npcName} に記憶を使用：{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} に「{memoryContent}」を使った。");

        // 対象キャラと使った記憶を TurnState に通知
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        var memory = FindAnyObjectByType<PlayerMemoryInventory>()?.FindMemoryByText(memoryContent);

        if (activeState != null && characterData != null && memory != null)
        {
            activeState.NotifyMemoryUsed(memory.ownerCharacter, characterData);
        }
    }

    private void NotifyTalked()
    {
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        if (activeState != null && characterData != null)
        {
            activeState.NotifyCharacterTalked(characterData);
        }
    }
}
