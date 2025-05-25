using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGiveUIController : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform memoryGrid; // 9マスのGridにMemoryItemプレハブを配置
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
        var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
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
                image.sprite = null; // または透明スプライト
                button.gameObject.SetActive(false); // 空スロットは非表示
            }
        }
    }

    private void GiveMemory(MemoryData selectedMemory)
    {
        // 渡した記憶が期待されたものかを確認
        if (targetCharacter.expectedMemory != null &&
            selectedMemory == targetCharacter.expectedMemory)
        {
            UIManager.Instance.ShowDialogue($"{targetCharacter.characterName} に正しい記憶を渡した！");
            // TODO: 正しい反応やフラグ処理をここに
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{targetCharacter.characterName} に記憶を渡したが、反応はなかった…");
        }

        Close();

        // 状態更新
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        if (activeState != null)
        {
            activeState.NotifyMemoryUsed(selectedMemory.ownerCharacter, targetCharacter);
        }
    }
}
