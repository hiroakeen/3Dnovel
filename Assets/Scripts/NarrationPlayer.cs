using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NarrationPlayer : MonoBehaviour
{
    [SerializeField] private NarrationData narrationData;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    private int currentIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        dialogueCanvas.SetActive(true);
        nextButton.onClick.AddListener(ShowNextLine);
        ShowLine(currentIndex);
    }

    void ShowNextLine()
    {
        if (isTyping) return; // タイピング中は無視

        currentIndex++;

        if (currentIndex < narrationData.lines.Count)
        {
            ShowLine(currentIndex);
        }
        else
        {
            EndNarration();
        }
    }

    void ShowLine(int index)
    {
        StartCoroutine(TypeText(narrationData.lines[index].text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }

    void EndNarration()
    {
        dialogueCanvas.SetActive(false);
        nextButton.onClick.RemoveAllListeners();

        // 本編開始
        GameManager.Instance.StartGameplay();
    }

}
