using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryGiveUIController : MonoBehaviour
{
    [Header("UI�Q��")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform memoryGrid; // 9�}�X��Grid��MemoryItem�v���n�u��z�u
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
                image.sprite = null; // �܂��͓����X�v���C�g
                button.gameObject.SetActive(false); // ��X���b�g�͔�\��
            }
        }
    }

    private void GiveMemory(MemoryData selectedMemory)
    {
        // �n�����L�������҂��ꂽ���̂����m�F
        if (targetCharacter.expectedMemory != null &&
            selectedMemory == targetCharacter.expectedMemory)
        {
            UIManager.Instance.ShowDialogue($"{targetCharacter.characterName} �ɐ������L����n�����I");
            // TODO: ������������t���O������������
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{targetCharacter.characterName} �ɋL����n�������A�����͂Ȃ������c");
        }

        Close();

        // ��ԍX�V
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        if (activeState != null)
        {
            activeState.NotifyMemoryUsed(selectedMemory.ownerCharacter, targetCharacter);
        }
    }
}
