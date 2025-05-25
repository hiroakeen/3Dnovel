using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkTrigger : MonoBehaviour
{
    [Header("���ʂ̃A�N�V�����{�^���i�b���^�L����n���j")]
    [SerializeField] private GameObject talkActionButton; // 1�ɓ���
    [SerializeField] private TMP_Text actionButtonText;   // Text: "�b��" or "�L����n��"

    [Header("����NPC�̃L�����f�[�^")]
    [SerializeField] private CharacterMemoryData characterData;

    [Header("�L���n��UI�̐���X�N���v�g")]
    [SerializeField] private MemoryGiveUIController memoryGiveUI;

    private bool isPlayerNear = false;

    void Start()
    {
        if (talkActionButton != null)
        {
            talkActionButton.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetButtonDown("Submit"))
        {
            HandleInteraction();
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = true;

        if (talkActionButton == null)
        {
            Debug.LogError("talkActionButton �� null �ł�");
            return;
        }

        if (actionButtonText == null)
        {
            Debug.LogError("actionButtonText �� null �ł�");
            return;
        }

        talkActionButton.SetActive(true);

        Button btn = talkActionButton.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("talkActionButton �� Button �R���|�[�l���g���t���Ă��܂���I");
            return;
        }

        btn.onClick.RemoveAllListeners();

        if (GameTurnStateManager.Instance.CurrentState == GameTurnState.TalkPhase)
        {
            actionButtonText.text = "�b��";
            btn.onClick.AddListener(TalkToNPC);
        }
        else if (GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            actionButtonText.text = "�L����n��";
            btn.onClick.AddListener(() => GiveMemoryToNPC(characterData));
        }
    }

private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerNear = false;
        talkActionButton?.SetActive(false);
    }

    // Submit�L�[�ł��Ăяo���鋤�ʏ���
    private void HandleInteraction()
    {
        if (GameTurnStateManager.Instance.CurrentState == GameTurnState.TalkPhase)
        {
            TalkToNPC();
        }
        else if (GameTurnStateManager.Instance.CurrentState == GameTurnState.MemoryPhase)
        {
            GiveMemoryToNPC(characterData);
        }
    }

    public void TalkToNPC()
    {
        talkActionButton?.SetActive(false);

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

                NotifyTalked();
                return;
            }
        }

        // �L���g�p�ΏۂȂ�A�I��UI���ŕ\��
        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}");
        }

        NotifyTalked();
    }

    public void GiveMemoryToNPC(CharacterMemoryData target)
    {
        talkActionButton?.SetActive(false);
        memoryGiveUI.Open(target);
    }

    public void UseMemory(string memoryContent)
    {
        string npcName = characterData != null ? characterData.characterName : "NPC";
        Debug.Log($"{npcName} �ɋL�����g�p�F{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} �Ɂu{memoryContent}�v���g�����B");

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
