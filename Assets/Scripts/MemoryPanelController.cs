using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Buffers;

public class MemoryPanelController : MonoBehaviour
{
    [SerializeField] private GameObject memoryPanel;
    [SerializeField] private GameObject descriptionBox;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button memoryButton;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private Button closeDescriptionButton;
    [SerializeField] private Transform memoryGrid;
    [SerializeField] private GameObject memoryItemPrefab;
    [SerializeField] private TextMeshProUGUI noMemoryText;

    private bool isPaused = false;

    private void Start()
    {
        memoryPanel.SetActive(false);
        descriptionBox.SetActive(false);
        memoryButton.onClick.AddListener(OpenMemoryPanel);
        closePanelButton.onClick.AddListener(CloseMemoryPanel);
        closeDescriptionButton.onClick.AddListener(() => descriptionBox.SetActive(false));

        PopulateMemoryItems();
    }

    private void OpenMemoryPanel()
    {
        memoryPanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    private void PopulateMemoryItems()
    {
        var allSlots = memoryGrid.GetComponentsInChildren<Button>(true); // 非アクティブ含む
        var memories = MemoryManager.Instance.GetCollectedMemories();

        bool hasMemory = memories != null && memories.Count > 0;
        noMemoryText.gameObject.SetActive(!hasMemory);

        for (int i = 0; i < allSlots.Length; i++)
        {
            var image = allSlots[i].GetComponentInChildren<Image>();
            if (i < memories.Count)
            {
                var memory = memories[i];
                image.sprite = memory.image;
                allSlots[i].gameObject.SetActive(true);
                int index = i; // クロージャ対策
                allSlots[i].onClick.RemoveAllListeners();
                allSlots[i].onClick.AddListener(() => ShowDescription(memories[index].description));
            }
            else
            {
                image.sprite = null; // 透明などにして空スロット感
                allSlots[i].onClick.RemoveAllListeners();
            }
        }
    }


    private void CloseMemoryPanel()
    {
        memoryPanel.SetActive(false);
        descriptionBox.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }



    private void ShowDescription(string desc)
    {
        descriptionText.text = desc;
        descriptionBox.SetActive(true);
    }

}
