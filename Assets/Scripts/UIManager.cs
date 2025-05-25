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
    }

    public void ShowDialogueWithMemoryOption(string npcName, string dialogueLine, TalkTrigger trigger)
    {
        currentTalkTrigger = trigger;
        ShowDialogue($"{npcName}ÅF{dialogueLine}");

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
}
