using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTrigger : MonoBehaviour
{
    [Header("共通のアクションボタン（話す／記憶を渡す）")]
    [SerializeField] private GameObject talkActionButton; // プレハブにして使いまわし
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

        // NPC移動停止（SimpleNPCWalkerがアタッチされていれば）
        var walker = GetComponent<SimpleNPCWalker>();
        walker?.SetTalking(true);

        // 記憶の自動取得
        if (memoryToGrant != null)
        {
            var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                // UI更新用にMemoryManagerにも登録（←必要！）
                MemoryManager.Instance?.AddMemory(memoryToGrant);
                UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）");

                NotifyTalked();
                return;
            }
        }

        if (isMemoryUseTarget && GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            // フェーズが記憶フェーズのときだけ、記憶使用UIつきで表示
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            // 通常の会話として表示
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

        var memory = Object.FindFirstObjectByType<PlayerMemoryInventory>()?.FindMemoryByText(memoryContent);
        var currentState = GameTurnStateManager.Instance.GetCurrentState();

        if (currentState != null && characterData != null && memory != null)
        {
            currentState.NotifyMemoryUsed(memory.ownerCharacter, characterData);
        }
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
        // NPCの移動再開
        var walker = GetComponent<SimpleNPCWalker>();
        walker?.SetTalking(false);

        // 最後に話し終わった通知（セリフ表示後）
        var currentState = GameTurnStateManager.Instance.GetCurrentState();
        currentState?.NotifyTalkFinished(characterData);
    }


}
