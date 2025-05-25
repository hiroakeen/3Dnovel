using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryPanelController : MonoBehaviour
{
    [SerializeField] private GameObject closeDescriptionButtonObject;
    [SerializeField] private GameObject descriptionBox;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button closeDescriptionButton;

    [SerializeField] private Button memoryButton;
    [SerializeField] private GameObject memoryPanel;
    [SerializeField] private Transform memoryGrid;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private GameObject closePanelButtonObject;
    [SerializeField] private GameObject memoryItemPrefab;

    private bool isPaused = false;

    private void Start()
    {
        // 初期状態で全UIを非表示にして安全スタート
        memoryPanel.SetActive(false);
        descriptionBox.SetActive(false);
        closePanelButtonObject.SetActive(false);
        closeDescriptionButtonObject.SetActive(false);

        closePanelButton.onClick.AddListener(CloseMemoryPanel);
        closeDescriptionButton.onClick.AddListener(CloseDescriptionBox);
        memoryButton.onClick.RemoveAllListeners();
        memoryButton.onClick.AddListener(OpenMemoryPanel);
    }

    public void OpenMemoryPanel()
    {
        memoryPanel.SetActive(true);
        closePanelButtonObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        PopulateMemoryItems();
    }

    private void PopulateMemoryItems()
    {
        var allSlots = memoryGrid.GetComponentsInChildren<Button>(true);
        var memories = MemoryManager.Instance.GetCollectedMemories();

        for (int i = 0; i < allSlots.Length; i++)
        {
            var image = allSlots[i].GetComponentInChildren<Image>();
            if (i < memories.Count)
            {
                var memory = memories[i];
                image.sprite = memory.image;
                allSlots[i].gameObject.SetActive(true);
                int index = i;
                allSlots[i].onClick.RemoveAllListeners();
                allSlots[i].onClick.AddListener(() => ShowDescription(memories[index].description));
            }
            else
            {
                image.sprite = null;
                allSlots[i].gameObject.SetActive(false);
                allSlots[i].onClick.RemoveAllListeners();
            }
        }
    }

    public void CloseMemoryPanel()
    {
        Debug.Log("CloseMemoryPanel: memoryPanel false にする");
        memoryPanel.SetActive(false);
        closePanelButtonObject.SetActive(false);
        descriptionBox.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }


    private void ShowDescription(string desc)
    {
        descriptionText.text = desc;
        descriptionBox.SetActive(true);
    }

    public void CloseDescriptionBox()
    {
        descriptionBox.SetActive(false);
        closeDescriptionButtonObject.SetActive(false);
    }
}
