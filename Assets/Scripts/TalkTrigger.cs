using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTrigger : MonoBehaviour
{
    [Header("共通のアクションボタン（話す／記憶を渡す）")]
    [SerializeField] private GameObject talkActionButton;
    [SerializeField] private TMP_Text actionButtonText;

    [Header("このNPCのキャラデータ")]
    [SerializeField] private CharacterDataJson characterData;

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

        string npcName = characterData.name;

        var lang = LocalizationManager.Instance.GetCurrentLanguage();
        string dialogueLine = characterData.GetDialogueForCurrentTurn(lang);

        bool isMemoryUseTarget = characterData.isMemoryUseTarget;

        // ✅ 現在のターンに応じた記憶IDを取得
        int currentTurn = GameManager.Instance.GetTurn();
        string memoryIdToGrant = characterData.grantedMemoriesPerTurn
            ?.Find(entry => entry.turn == currentTurn)?.memoryId;

        MemoryData memoryToGrant = null;

        if (!string.IsNullOrEmpty(memoryIdToGrant))
        {
            memoryToGrant = MemoryManager.Instance?.FindMemoryById(memoryIdToGrant);
        }

        var walker = GetComponent<SimpleNPCWalker>();
        walker?.SetTalking(true);

        if (memoryToGrant != null)
        {
            var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                MemoryManager.Instance?.AddMemory(memoryToGrant);

                UIManager.Instance.ShowDialogue(
                    $"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）"
                );

                NotifyTalked();
                return;
            }
        }

        if (isMemoryUseTarget && GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}");
        }

        NotifyTalked();
    }



    public void GiveMemoryToNPC(CharacterDataJson target)
    {
        talkActionButton?.SetActive(false);
        memoryGiveUI.Open(target);
    }

    public void UseMemory(string memoryContent)
    {
        string npcName = characterData != null ? characterData.name : "NPC";
        Debug.Log($"{npcName} に記憶を使用：{memoryContent}");

        var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
        var memory = inventory?.FindMemoryByText(memoryContent);
        var currentState = GameTurnStateManager.Instance.GetCurrentState();

        if (currentState != null && characterData != null && memory != null)
        {
            currentState.NotifyMemoryUsed(memory.GetOwnerCharacter(), characterData);
            inventory.RemoveMemory(memory);

            // ✅ リアクションセリフ選択
            string reactionLine = "……？";

            switch (characterData.memoryReactionType)
            {
                case MemoryReactionType.True:
                    reactionLine = characterData.reactionTrueJP;
                    break;
                case MemoryReactionType.Good:
                    reactionLine = characterData.reactionSuccessJP;
                    break;
                case MemoryReactionType.Bad:
                    reactionLine = characterData.reactionFailJP;
                    break;
                case MemoryReactionType.None:
                default:
                    reactionLine = "……？";
                    break;
            }

            // ✅ NPCのセリフとして表示
            UIManager.Instance.ShowDialogue(
                $"{npcName} に「{memoryContent}」を使った。\n{npcName}：{reactionLine}"
            );

            GameTurnStateManager.Instance.SetMemoryUsedThisTurn();
            TurnFlowController.Instance.AdvanceToNextTurn();
        }
    }


    public CharacterDataJson GetCharacterData()
    {
        return characterData;
    }



    private void NotifyTalked()
    {
        var currentState = GameTurnStateManager.Instance.GetCurrentState();
        if (currentState != null && characterData != null)
        {
            currentState.NotifyCharacterTalked(characterData);
        }
    }

    public void EndTalk()
    {
        var walker = GetComponent<SimpleNPCWalker>();
        walker?.SetTalking(false);

        var currentState = GameTurnStateManager.Instance.GetCurrentState();
        currentState?.NotifyTalkFinished(characterData);
    }
}
