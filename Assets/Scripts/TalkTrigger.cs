using UnityEngine;
using UnityEngine.UI;

public class TalkTrigger : MonoBehaviour
{
    [Header("共通のTalkボタン")]
    [SerializeField] private GameObject talkButton;
    [Header("会話情報")]
    [SerializeField] private string npcName = "NPC";
    [TextArea]
    [SerializeField] private string dialogueLine = "こんにちは。何か用ですか？";
    [Header("記憶使用対応")]
    [SerializeField] private bool isMemoryUseTarget = false;
    [Header("話しかけた時に取得する記憶（任意）")]
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
                UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}\n（{memoryToGrant.memoryText} を思い出した）");
                return;
            }
        }

        if (isMemoryUseTarget)
        {
            UIManager.Instance.ShowDialogueWithMemoryOption(npcName, dialogueLine, this);
        }
        else
        {
            UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}");
        }
    }

    public void UseMemory(string memoryContent)
    {
        Debug.Log($"{npcName} に記憶を使用：{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} に「{memoryContent}」を使った。");
    }
}
