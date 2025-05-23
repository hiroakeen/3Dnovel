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

    [Header("Prefabs")]
    [SerializeField] private GameObject characterButtonPrefab;
    [SerializeField] private Transform characterButtonParent;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Ending UI")]
    [SerializeField] private TextMeshProUGUI endingTitleText;
    [SerializeField] private TextMeshProUGUI endingDescriptionText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowCharacterSelection(List<CharacterMemoryData> characters, Action<CharacterMemoryData> onSelect)
    {
        ClearCharacterButtons();
        characterSelectionPanel.SetActive(true);

        foreach (var character in characters)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
            buttonObj.GetComponentInChildren<Text>().text = character.characterName;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => {
                characterSelectionPanel.SetActive(false);
                onSelect(character);
            });
        }
    }

    public void ShowDialogue(string dialogueLine)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLine;
    }


    public void ShowMemorySelection(List<CharacterMemoryData> memorySources, Action<CharacterMemoryData> onMemoryChosen)
    {
        memorySelectionPanel.SetActive(true);
        ClearCharacterButtons();

        foreach (var character in memorySources)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
            buttonObj.GetComponentInChildren<Text>().text = character.characterName + " ‚Ì‹L‰¯";
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
            buttonObj.GetComponentInChildren<Text>().text = character.characterName + " ‚ÉŽg‚í‚¹‚é";
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
}
