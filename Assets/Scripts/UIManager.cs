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
    [SerializeField] private MemoryCharacterDisplay characterDisplay;

    private PlayerControllerManager playerController;
    private TalkTrigger currentTalkTrigger;
    private PlayerMemoryInventory playerMemoryInventory;

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

    public void ShowDialogue(string characterName, Sprite characterSprite, string dialogueLine, Action onComplete)
    {
        if (playerController != null) playerController.PauseControl();

        gameplayPanel?.SetActive(false);
        dialoguePanel?.SetActive(true);

        // キャライラスト表示
        characterDisplay?.Show(characterSprite, characterName);

        // テキスト表示
        dialogueText.text = "";
        StartCoroutine(TypeText(dialogueLine, onComplete));
    }

    private IEnumerator TypeText(string dialogueLine, Action onComplete)
    {
        dialogueNextButton.gameObject.SetActive(false);

        foreach (char c in dialogueLine)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f); // 文字送り速度
        }

        dialogueNextButton.gameObject.SetActive(true);
        dialogueNextButton.onClick.RemoveAllListeners();
        dialogueNextButton.onClick.AddListener(() =>
        {
            HideDialogue();
            onComplete?.Invoke();
        });
    }

    public void ShowDialogue(string dialogueLine)
    {
        // キャライラストや名前は不要なときに使う簡易版
        ShowDialogue("???", null, dialogueLine, null);
    }

    public void HideDialogue()
    {
        playerController?.ResumeControl();
        gameplayPanel?.SetActive(true);
        dialoguePanel?.SetActive(false);
        characterDisplay?.Hide();

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

                    if (currentTalkTrigger != null)
                    {
                        currentTalkTrigger.UseMemory(memory);
                        GameTurnStateManager.Instance.RegisterMemoryGiven(currentTalkTrigger.CharacterId);
                        currentTalkTrigger = null;
                    }

                    // 最後にダイアログを閉じる
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

    public void SetTurnMessage(string message)
    {
        if (turnMessageText != null)
        {
            turnMessageText.text = message;
            turnMessageText.gameObject.SetActive(true);
        }
    }
}
