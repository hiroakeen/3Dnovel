using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTrigger : MonoBehaviour
{
    [Header("共通のアクションボタン（話す／記憶を渡す）")]
    [SerializeField] private GameObject talkActionButton; // 1つに統合
    [SerializeField] private TMP_Text actionButtonText;   // Text: "話す" or "記憶を渡す"

    [Header("このNPCのキャラデータ")]
    [SerializeField] private CharacterMemoryData characterData;

    [Header("記憶渡しUIの制御スクリプト")]
    [SerializeField] private MemoryGiveUIController memoryGiveUI;

    private bool isPlayerNear = false;

    void Start()
    {
        if (talkActionButton != null)
        {
            talkActionButton.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetButtonDown("Submit"))
        {
            HandleInteraction();
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = true;

        if (talkActionButton == null)
        {
            Debug.LogError("talkActionButton が null です");
            return;
        }

        if (actionButtonText == null)
        {
            Debug.LogError("actionButtonText が null です");
            return;
        }

        talkActionButton.SetActive(true);

        Button btn = talkActionButton.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("talkActionButton に Button コンポーネントが付いていません！");
            return;
        }

        btn.onClick.RemoveAllListeners();

        if (GameTurnStateManager.Instance.CurrentState == GameTurnState.TalkPhase)
        {
            actionButtonText.text = "話す";
            btn.onClick.AddListener(TalkToNPC);
        }
        else if (GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            actionButtonText.text = "記憶を渡す";
            btn.onClick.AddListener(() => GiveMemoryToNPC(characterData));
        }
    }

private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = false;
        talkActionButton?.SetActive(false);
    }

    // Submitキーでも呼び出せる共通処理
    private void HandleInteraction()
    {
        if (GameTurnStateManager.Instance.CurrentState == GameTurnState.TalkPhase)
        {
            TalkToNPC();
        }
        else if (GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            GiveMemoryToNPC(characterData);
        }
    }

    public void TalkToNPC()
    {
        talkActionButton?.SetActive(false);

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

                NotifyTalked();
                return;
            }
        }

        // 記憶使用対象なら、選択UIつきで表示
        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}");
        }

        NotifyTalked();
    }

    public void GiveMemoryToNPC(CharacterMemoryData target)
    {
        talkActionButton?.SetActive(false);
        memoryGiveUI.Open(target);
    }

    public void UseMemory(string memoryContent)
    {
        string npcName = characterData != null ? characterData.characterName : "NPC";
        Debug.Log($"{npcName} に記憶を使用：{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} に「{memoryContent}」を使った。");

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
