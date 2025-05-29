using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private GameTurnState? lastState = null;
    public string CharacterId => characterData?.id;

    void Start() => talkActionButton?.SetActive(false);

    void Update()
    {
        if (isPlayerNear && Input.GetButtonDown("Submit"))
            HandleInteraction();

        var currentState = GameTurnStateManager.Instance.CurrentState;
        if (isPlayerNear && currentState != lastState)
        {
            lastState = currentState;
            SetupActionButton();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        isPlayerNear = true;
        lastState = null;
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
        if (talkActionButton == null || actionButtonText == null) return;

        talkActionButton.SetActive(true);
        Button btn = talkActionButton.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("talkActionButton に Button がついてません");
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
        if (state == GameTurnState.TalkPhase) TalkToNPC();
        else if (state == GameTurnState.MemoryPhase) GiveMemoryToNPC(characterData);
    }

    public void TalkToNPC()
    {
        talkActionButton?.SetActive(false);
        if (characterData == null) return;

        string npcName = characterData.name;
        int currentTurn = GameManager.Instance.GetTurn();
        Language lang = Language.Japanese; // ※固定 or GameManager等から取得してください

        string dialogueLine = characterData.GetDialogueForCurrentTurn(currentTurn, lang);

        var memoryToGrant = MemoryManager.Instance?.FindAutoGrantedMemory(characterData.id, currentTurn);

        var walker = GetComponent<SimpleNPCWalker>();
        walker?.SetTalking(true);

        if (memoryToGrant != null)
        {
            var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                UIManager.Instance.ShowDialogue(
                    $"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）",
                    () =>
                    {
                        NotifyTalked();
                        EndTalk();
                    });
                return;
            }
        }

        UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}", () =>
        {
            NotifyTalked();
            EndTalk();
        });
    }


    public void GiveMemoryToNPC(CharacterDataJson target)
    {
        talkActionButton?.SetActive(false);
        memoryGiveUI.Open(target);
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

    public CharacterDataJson GetCharacterData() => characterData;

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

        bool isCorrect = memory.IsCorrectReceiver(characterData.id);
        string reactionLine = isCorrect
            ? characterData.reactionCorrectJP
            : characterData.reactionIncorrectJP;

        UIManager.Instance.ShowDialogue(
            $"{npcName} に「{memory.memoryText}」を使った。\n{npcName}：{reactionLine}",
            () =>
            {
                GameTurnStateManager.Instance.RegisterMemoryGiven(characterData.id);

                var decision = new TurnDecision(
                    GameManager.Instance.GetTurn(),
                    memory.originalOwner,
                    characterData,
                    memory
                );
                GameManager.Instance.AddDecisionLog(decision);

                GameTurnStateManager.Instance.GetCurrentState()?.NotifyMemoryUsed(
                    memory.originalOwner,
                    characterData,
                    memory
                );
            });
    }
}
