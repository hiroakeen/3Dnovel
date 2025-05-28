using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Button dialogueNextButton;
    [SerializeField] private TextMeshProUGUI turnMessageText;
    [SerializeField] private GameObject narrationPanel;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private Button narrationNextButton;

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

    public void ShowDialogue(string dialogueLine, Action onComplete)
    {
        if (playerController != null) playerController.PauseControl();

        gameplayPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);
        dialogueText.text = dialogueLine;

        dialogueNextButton.gameObject.SetActive(true);
        dialogueNextButton.onClick.RemoveAllListeners();
        dialogueNextButton.onClick.AddListener(() =>
        {
            HideDialogue();
            dialogueNextButton.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void ShowDialogue(string dialogueLine)
    {
        ShowDialogue(dialogueLine, null);
    }

    public void HideDialogue()
    {
        if (playerController != null) playerController.ResumeControl();

        gameplayPanel?.SetActive(true);
        dialoguePanel?.SetActive(false);

        if (currentTalkTrigger != null)
        {
            currentTalkTrigger.EndTalk();
            currentTalkTrigger = null;
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
                        GameTurnStateManager.Instance.RegisterMemoryGiven(currentTalkTrigger.CharacterId); // 修正ポイント
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
        if (narrationPanel == null || narrationText == null || narrationNextButton == null)
        {
            Debug.LogError("UIManager: ナレーションUIの参照が不足しています");
            return;
        }

        if (playerController != null)
            playerController.PauseControl();

        narrationPanel.SetActive(true);
        narrationText.text = message;

        StartCoroutine(ShowNarrationRoutine(onComplete));
    }

    private IEnumerator ShowNarrationRoutine(Action onComplete)
    {
        yield return new WaitForSecondsRealtime(0.1f); // ★ 修正：1フレーム以上確実に待つ

        Time.timeScale = 0;

        narrationNextButton.onClick.RemoveAllListeners();
        narrationNextButton.onClick.AddListener(() =>
        {
            narrationPanel.SetActive(false);
            Time.timeScale = 1;

            if (playerController != null)
                playerController.ResumeControl();

            onComplete?.Invoke();
        });
    }


    public void ShowTurnStartMessage(int turn)
    {
        ShowNarration($"謎の声：{turn}ターン目が始まった。", null);
    }

    public void SetTurnMessage(string message)
    {
        if (turnMessageText != null)
        {
            turnMessageText.text = message;
            turnMessageText.gameObject.SetActive(true);
        }
    }

    public void ResetMemoryNarrationFlag()
    {
        hasShownMemoryNarration = false;
    }

}
