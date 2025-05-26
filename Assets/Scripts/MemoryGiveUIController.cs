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

    private CharacterMemoryData targetCharacter;

    public void Open(CharacterMemoryData characterData)
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

        var npc = FindMatchingNPC(targetCharacter);
        if (npc != null)
        {
            npc.ReceiveMemory(selectedMemory);
        }
        else
        {
            Debug.LogWarning("該当NPCが見つかりませんでした");
        }
    }

    private NPC FindMatchingNPC(CharacterMemoryData data)
    {
        var allNPCs = Object.FindObjectsByType<NPC>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
        {
            if (npc.name == data.name || npc.GetCharacterData() == data)
            {
                return npc;
            }
        }
        return null;
    }
}
