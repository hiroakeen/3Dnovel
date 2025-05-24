using UnityEngine;
using UnityEngine.UI;

public class TalkTrigger : MonoBehaviour
{
    [Header("���ʂ�Talk�{�^��")]
    [SerializeField] private GameObject talkButton;
    [Header("��b���")]
    [SerializeField] private string npcName = "NPC";
    [TextArea]
    [SerializeField] private string dialogueLine = "����ɂ��́B�����p�ł����H";
    [Header("�L���g�p�Ή�")]
    [SerializeField] private bool isMemoryUseTarget = false;
    [Header("�b�����������Ɏ擾����L���i�C�Ӂj")]
    [SerializeField] private MemoryData memoryToGrant;

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

        if (memoryToGrant != null)
        {
            var inventory = FindAnyObjectByType<PlayerMemoryInventory>();
            if (inventory != null && !inventory.GetAllMemories().Contains(memoryToGrant))
            {
                inventory.AddMemory(memoryToGrant);
                UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}\n�i{memoryToGrant.memoryText} ���v���o�����j");
                return;
            }
        }

        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}");
        }
    }

    public void UseMemory(string memoryContent)
    {
        Debug.Log($"{npcName} �ɋL�����g�p�F{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} �Ɂu{memoryContent}�v���g�����B");
    }
}
