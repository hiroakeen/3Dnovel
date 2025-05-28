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
    public string CharacterId => characterData?.id;

    void Start()
    {
        talkActionButton?.SetActive(false);
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
        SetupActionButton();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = false;
        talkActionButton?.SetActive(false);
    }

    private void SetupActionButton()
    {
        if (talkActionButton == null || actionButtonText == null)
        {
            Debug.LogError("トークUIの設定が不足しています");
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

        var state = GameTurnStateManager.Instance.CurrentState;
        if (state == GameTurnState.TalkPhase)
        {
            actionButtonText.text = "話す";
            btn.onClick.AddListener(TalkToNPC);
        }
        else if (state == GameTurnState.MemoryPhase)
        {
            actionButtonText.text = "記憶を渡す";
            btn.onClick.AddListener(() => GiveMemoryToNPC(characterData));
        }
    }

    private void HandleInteraction()
    {
        var state = GameTurnStateManager.Instance.CurrentState;
        if (state == GameTurnState.TalkPhase)
        {
            TalkToNPC();
        }
        else if (state == GameTurnState.MemoryPhase)
        {
            GiveMemoryToNPC(characterData);
        }
    }

    public void TalkToNPC()
    {
        talkActionButton?.SetActive(false);

        if (characterData == null) return;

        string npcName = characterData.name;
        string dialogueLine = characterData.GetDialogueForCurrentTurn(LocalizationManager.Instance.GetCurrentLanguage());
        bool isMemoryUseTarget = characterData.isMemoryUseTarget;

        MemoryData memoryToGrant = null;
        int currentTurn = GameManager.Instance.GetTurn();
        string memoryIdToGrant = characterData.grantedMemoriesPerTurn?.Find(entry => entry.turn == currentTurn)?.memoryId;

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
                    $"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）",
                    () =>
                    {
                        NotifyTalked();  // ✅ ShowDialogue完了後にカウント
                        EndTalk();       // ✅ 明示的に会話終了を呼ぶ
                    });

                return;
            }
        }

        if (isMemoryUseTarget && GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}", () =>
            {
                NotifyTalked();
                EndTalk();
            });
        }

        NotifyTalked();
    }

    public void GiveMemoryToNPC(CharacterDataJson target)
    {
        talkActionButton?.SetActive(false);
        memoryGiveUI.Open(target);
    }

    public void UseMemory(MemoryData memory)
    {
        if (characterData == null || memory == null) return;

        string npcName = characterData.name;
        Debug.Log($"{npcName} に記憶を使用：{memory.memoryText}");

        var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
        if (inventory == null || !inventory.GetAllMemories().Contains(memory))
        {
            Debug.LogWarning("記憶が手元に存在しません");
            return;
        }

        inventory.RemoveMemory(memory);
        MemoryManager.Instance.RecordMemoryUsage(memory, characterData.id);

        string reactionLine = characterData.memoryReactionType switch
        {
            MemoryReactionType.True => characterData.reactionTrueJP,
            MemoryReactionType.Good => characterData.reactionSuccessJP,
            MemoryReactionType.Bad => characterData.reactionFailJP,
            _ => "……？",
        };

        UIManager.Instance.ShowDialogue(
            $"{npcName} に「{memory.memoryText}」を使った。\n{npcName}：{reactionLine}",
            () =>
            {
                GameTurnStateManager.Instance.RegisterMemoryGiven(characterData.id);
            });
    }

    public CharacterDataJson GetCharacterData()
    {
        return characterData;
    }

    private void NotifyTalked()
    {
        var currentState = GameTurnStateManager.Instance.GetCurrentState();
        currentState?.NotifyCharacterTalked(characterData);
    }

    public void EndTalk()
    {
        GetComponent<SimpleNPCWalker>()?.SetTalking(false);
        GameTurnStateManager.Instance.GetCurrentState()?.NotifyTalkFinished(this.characterData);

    }
}
