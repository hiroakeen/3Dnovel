using UnityEngine;
using UnityEngine.UI;

public class TalkTrigger : MonoBehaviour
{
    [Header("���ʂ�Talk�{�^��")]
    [SerializeField] private GameObject talkButton;
    [Header("����NPC�̃L�����f�[�^")]
    [SerializeField] private CharacterMemoryData characterData;

    private bool isPlayerNear = false;
    private Transform player;

    void Start()
    {
        if (talkButton != null)
        {
            talkButton.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetButtonDown("Submit"))
        {
            TalkToNPC();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isPlayerNear = true;

            if (talkButton != null)
            {
                talkButton.SetActive(true);
                Button btn = talkButton.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(TalkToNPC);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (talkButton != null) talkButton.SetActive(false);
        }
    }

    public void TalkToNPC()
    {
        if (talkButton != null) talkButton.SetActive(false);

        if (characterData == null)
        {
            Debug.LogWarning("characterData ���ݒ肳��Ă��܂���B");
            return;
        }

        string npcName = characterData.characterName;
        string dialogueLine = characterData.GetDialogueForCurrentTurn();
        bool isMemoryUseTarget = characterData.isMemoryUseTarget;
        MemoryData memoryToGrant = characterData.autoGrantedMemory;

        // �L���̎����擾
        if (memoryToGrant != null)
        {
            var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}\n�i{memoryToGrant.memoryText} ���v���o�����j");

                NotifyTalked(); // �� TurnState�ɒʒm��ǉ�
                return;
            }
        }

        // �L�����g���ΏۂȂ�
        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}");
        }

        NotifyTalked(); // �� TurnState�ɒʒm
    }

    public void UseMemory(string memoryContent)
    {
        string npcName = characterData != null ? characterData.characterName : "NPC";
        Debug.Log($"{npcName} �ɋL�����g�p�F{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} �Ɂu{memoryContent}�v���g�����B");

        // �ΏۃL�����Ǝg�����L���� TurnState �ɒʒm
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        var memory = FindAnyObjectByType<PlayerMemoryInventory>()?.FindMemoryByText(memoryContent);

        if (activeState != null && characterData != null && memory != null)
        {
            activeState.NotifyMemoryUsed(memory.ownerCharacter, characterData);
        }
    }

    private void NotifyTalked()
    {
        var activeState = FindAnyObjectByType<GameStateManager>()?.GetCurrentState() as TurnState;
        if (activeState != null && characterData != null)
        {
            activeState.NotifyCharacterTalked(characterData);
        }
    }
}
