using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameplayPanel;    
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject memorySelectPanel;
    [SerializeField] private GameObject grayOverlayPanel;
    [SerializeField] private GameObject useMemoryButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject memoryButtonPrefab;
    [SerializeField] private Transform memoryButtonParent;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI turnMessageText;
    [SerializeField] private GameObject narrationPanel;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private Button narrationNextButton;
    [SerializeField] private TurnMessageTable messageTable;

    private PlayerControllerManager playerController;
    private TalkTrigger currentTalkTrigger;
    private PlayerMemoryInventory playerMemoryInventory;

    private bool hasShownMemoryNarration = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerControllerManager>();
        playerMemoryInventory = FindAnyObjectByType<PlayerMemoryInventory>();

        gameplayPanel?.SetActive(true);
        dialoguePanel?.SetActive(false);
        memorySelectPanel?.SetActive(false);
        grayOverlayPanel?.SetActive(false);
        useMemoryButton?.SetActive(false);
    }

    public void ShowDialogue(string dialogueLine)
    {
        if (playerController != null) playerController.PauseControl();

        gameplayPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);

        dialogueText.text = dialogueLine;
    }

    public void HideDialogue()
    {
        if (playerController != null) playerController.ResumeControl();

        gameplayPanel?.SetActive(true);
        dialoguePanel?.SetActive(false);

        // 会話相手がいた場合、TalkTrigger に通知
        if (currentTalkTrigger != null)
        {
            currentTalkTrigger.EndTalk();
            currentTalkTrigger = null;
        }

        // 記憶が3つになったあと、まだナレーションを出していなければここで表示
        var inventory = GameObject.FindFirstObjectByType<PlayerMemoryInventory>();
        if (!hasShownMemoryNarration && inventory != null && inventory.GetAllMemories().Count >= 3)
        {
            hasShownMemoryNarration = true;

            ShowNarration(
                "謎の声：手に入れた記憶がそろった……渡す時間だ。",
                () =>
                {
                    UIManager.Instance.SetTurnMessage("ひとりを選んで、手に入れた記憶を渡そう！");
                    GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase);
                });
        }
    }

    public void ShowDialogueWithMemoryOption(string npcName, string dialogueLine, TalkTrigger trigger)
    {
        currentTalkTrigger = trigger;
        ShowDialogue($"{npcName}：{dialogueLine}");

        if (useMemoryButton != null)
        {
            useMemoryButton.SetActive(true);
            useMemoryButton.GetComponent<Button>().onClick.RemoveAllListeners();
            useMemoryButton.GetComponent<Button>().onClick.AddListener(ShowMemoryList);
        }
    }

    public void ShowMemoryList()
    {
        if (memorySelectPanel != null && playerMemoryInventory != null)
        {
            memorySelectPanel.SetActive(true);
            ClearMemoryButtons();

            List<MemoryData> memories = playerMemoryInventory.GetAllMemories();
            for (int i = memories.Count - 1; i >= 0; i--)
            {
                var memory = memories[i];

                GameObject buttonObj = Instantiate(memoryButtonPrefab, memoryButtonParent);
                var label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null)
                    label.text = memory.memoryText;

                buttonObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    memorySelectPanel.SetActive(false);
                    useMemoryButton.SetActive(false);
                    playerMemoryInventory.RemoveMemory(memory);

                    if (currentTalkTrigger != null)
                    {
                        currentTalkTrigger.UseMemory(memory.memoryText);
                        currentTalkTrigger = null;
                    }

                    HideDialogue();
                });
            }
        }
    }

    private void ClearMemoryButtons()
    {
        foreach (Transform child in memoryButtonParent)
        {
            Destroy(child.gameObject);
        }
    }


    public void ShowTurnMessage(string message)
    {
        if (turnMessageText != null)
        {
            turnMessageText.text = message;
            turnMessageText.gameObject.SetActive(true);
        }
    }
    public void ShowNarration(string message, Action onComplete)
    {
        if (playerController != null) playerController.PauseControl(); // プレイヤー停止
        Time.timeScale = 0; // 一時停止（演出用）

        narrationPanel?.SetActive(true);
        narrationText.text = message;

        narrationNextButton.onClick.RemoveAllListeners();
        narrationNextButton.onClick.AddListener(() =>
        {
            narrationPanel?.SetActive(false);
            if (playerController != null) playerController.ResumeControl();
            Time.timeScale = 1; // 再開
            onComplete?.Invoke();
        });
    }
    public void ResetMemoryNarrationFlag()
    {
        hasShownMemoryNarration = false;
    }
    public void SetTurnMessage(string message)
    {
        if (turnMessageText != null)
        {
            turnMessageText.text = message;
            turnMessageText.gameObject.SetActive(true);
        }
    }

    public void SetTurnMessageByKeyWithTurn(TurnMessageKey key, int turn)
    {
        string msg = messageTable?.GetMessage(key);
        if (!string.IsNullOrEmpty(msg))
            SetTurnMessage(msg.Replace("{N}", turn.ToString()));
    }

}
