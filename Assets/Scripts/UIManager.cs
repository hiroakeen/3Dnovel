using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject memorySelectionPanel;
    [SerializeField] private GameObject targetSelectionPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private GameObject grayOverlayPanel;
    [SerializeField] private GameObject memoryUseButton;
    [SerializeField] private GameObject memoryListPanel;
    [SerializeField] private Transform memoryButtonParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private Transform characterButtonParent;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Ending UI")]
    [SerializeField] private TextMeshProUGUI endingTitleText;
    [SerializeField] private TextMeshProUGUI endingDescriptionText;

    [SerializeField] private Canvas mainGameplayCanvas;
    [SerializeField] private Canvas dialogueCanvas;


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

        // 最初は移動状態なので、移動用Canvasを表示
        if (mainGameplayCanvas != null)
        {
            mainGameplayCanvas.gameObject.SetActive(true);
        }

        // 会話Canvasは非表示
        if (dialogueCanvas != null)
        {
            dialogueCanvas.gameObject.SetActive(false);
        }

        if (grayOverlayPanel != null)
        {
            grayOverlayPanel.SetActive(false);
        }
        if (memoryUseButton != null)
        {
            memoryUseButton.SetActive(false);
        }
        if (memoryListPanel != null)
        {
            memoryListPanel.SetActive(false);
        }
    }


    public void ShowCharacterSelection(List<CharacterMemoryData> characters, Action<CharacterMemoryData> onSelect)
    {
        ClearCharacterButtons();
        characterSelectionPanel.SetActive(true);

        foreach (var character in characters)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                characterSelectionPanel.SetActive(false);
                onSelect(character);
            });
        }
    }

    public void ShowDialogue(string dialogueLine)
    {
        if (playerController != null) playerController.PauseControl();
        mainGameplayCanvas.gameObject.SetActive(false);
        dialogueCanvas.gameObject.SetActive(true);

        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLine;
    }

    public void HideDialogue()
    {
        if (playerController != null) playerController.ResumeControl();
        mainGameplayCanvas.gameObject.SetActive(true);
        dialogueCanvas.gameObject.SetActive(false);

        dialoguePanel.SetActive(false);
    }



    public void ShowDialogueWithMemoryOption(string npcName, string dialogueLine, TalkTrigger trigger)
    {
        currentTalkTrigger = trigger;
        ShowDialogue($"{npcName}：{dialogueLine}");

        if (memoryUseButton != null)
        {
            memoryUseButton.SetActive(true);
            memoryUseButton.GetComponent<Button>().onClick.RemoveAllListeners();
            memoryUseButton.GetComponent<Button>().onClick.AddListener(ShowMemoryList);
        }
    }

    public void ShowMemoryList()
    {
        if (memoryListPanel != null && playerMemoryInventory != null)
        {
            memoryListPanel.SetActive(true);
            ClearMemoryButtons();

            List<MemoryData> memories = playerMemoryInventory.GetAllMemories();

            foreach (var memory in memories)
            {
                GameObject buttonObj = Instantiate(characterButtonPrefab, memoryButtonParent);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = memory.memoryText;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                    memoryListPanel.SetActive(false);
                    memoryUseButton.SetActive(false);
                    currentTalkTrigger?.UseMemory(memory.memoryText);
                    playerMemoryInventory.RemoveMemory(memory);
                });
            }
        }
    }

    public void ShowMemorySelection(List<CharacterMemoryData> memorySources, Action<CharacterMemoryData> onMemoryChosen)
    {
        memorySelectionPanel.SetActive(true);
        ClearCharacterButtons();

        foreach (var character in memorySources)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName + " の記憶";
            buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                memorySelectionPanel.SetActive(false);
                onMemoryChosen(character);
            });
        }
    }

    public void ShowTargetSelection(List<CharacterMemoryData> targets, Action<CharacterMemoryData> onTargetChosen)
    {
        targetSelectionPanel.SetActive(true);
        ClearCharacterButtons();

        foreach (var character in targets)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName + " に使わせる";
            buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                targetSelectionPanel.SetActive(false);
                onTargetChosen(character);
            });
        }
    }

    public void ShowEnding(string title, string description)
    {
        endingPanel.SetActive(true);
        endingTitleText.text = title;
        endingDescriptionText.text = description;
    }

    private void ClearCharacterButtons()
    {
        foreach (Transform child in characterButtonParent)
        {
            Destroy(child.gameObject);
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
