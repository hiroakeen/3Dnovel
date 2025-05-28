using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class NarrationPlayer : MonoBehaviour
{
    public static NarrationPlayer Instance { get; private set; }

    [SerializeField] private NarrationData narrationData; // 最初の導入ナレーション用
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Queue<(string text, Action onComplete)> narrationQueue = new();
    private bool hasShownMemoryNarration = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    void Start()
    {
        if (narrationData != null && narrationData.lines.Count > 0)
        {
            dialogueCanvas.SetActive(true);
            nextButton.onClick.AddListener(ShowNextLine);
            ShowLine(currentIndex);
        }
        else
        {
            dialogueCanvas.SetActive(false);
        }
    }

    public void PlayNarration(string message, Action onComplete = null)
    {
        narrationQueue.Enqueue((message, onComplete));
        if (!dialogueCanvas.activeSelf && !isTyping)
        {
            dialogueCanvas.SetActive(true);
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(ShowQueuedNarration);
            ShowQueuedNarration();
        }
    }

    private void ShowQueuedNarration()
    {
        if (narrationQueue.Count == 0)
        {
            EndNarration();
            return;
        }

        var (text, onComplete) = narrationQueue.Dequeue();
        StartCoroutine(TypeText(text, () =>
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() =>
            {
                onComplete?.Invoke();
                ShowQueuedNarration();
            });
        }));
    }

    void ShowNextLine()
    {
        if (isTyping) return;

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
        string line = narrationData.lines[index].text;
        Debug.Log($"[Narration] Showing line: {line}");
        StartCoroutine(TypeText(line));
    }

    IEnumerator TypeText(string fullText, Action onFinished = null)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
        onFinished?.Invoke();
    }

    void EndNarration()
    {
        dialogueCanvas.SetActive(false);
        nextButton.onClick.RemoveAllListeners();
    }

    public void ResetMemoryNarrationFlag()
    {
        hasShownMemoryNarration = false;
    }

}
