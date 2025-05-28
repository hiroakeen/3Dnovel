using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGiveUIController : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform memoryGrid;
    [SerializeField] private Button cancelButton;
    [SerializeField] private GameObject memoryItemPrefab;

    private CharacterDataJson targetCharacter;

    public void Open(CharacterDataJson characterData)
    {
        targetCharacter = characterData;
        panel.SetActive(true);
        PopulateGrid();
        Time.timeScale = 0;
    }

    public void Close()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    private void Start()
    {
        cancelButton.onClick.AddListener(Close);
    }

    private void PopulateGrid()
    {
        var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
        if (inventory == null) return;

        var memories = inventory.GetAllMemories();

        var slots = memoryGrid.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < slots.Length; i++)
        {
            var button = slots[i];
            var image = button.GetComponentInChildren<Image>();
            button.onClick.RemoveAllListeners();

            if (i < memories.Count)
            {
                var memory = memories[i];
                image.sprite = memory.memoryImage;
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => GiveMemory(memory));
            }
            else
            {
                image.sprite = null;
                button.gameObject.SetActive(false);
            }
        }
    }

    private void GiveMemory(MemoryData selectedMemory)
    {
        Close();

        // ✅ 正解判定と記録
        MemoryManager.Instance.RecordMemoryUsage(selectedMemory, targetCharacter.id);

        // ✅ 該当キャラが見つかればTalk演出を呼ぶ
        var allTalkTriggers = Object.FindObjectsByType<TalkTrigger>(FindObjectsSortMode.None);
        foreach (var trigger in allTalkTriggers)
        {
            if (trigger.GetCharacterData()?.id == targetCharacter.id)
            {
                trigger.UseMemory(selectedMemory.memoryText);
                return;
            }
        }

        // ✅ TalkTriggerがない場合でも反応はさせる（不正解演出）
        UIManager.Instance.ShowDialogue($"{targetCharacter.name} に「{selectedMemory.memoryText}」を使った。\n{targetCharacter.name}：……？");

        // ✅ ターン進行（TalkTrigger経由でも呼ばれているなら重複防止してもOK）
        TurnFlowController.Instance.AdvanceToNextTurn();
    }

}