using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    [Header("対象となるUIボタン")]
    [SerializeField] private GameObject talkButton;
    [Header("会話情報")]
    [SerializeField] private string npcName = "NPC";
    [TextArea]
    [SerializeField] private string dialogueLine = "こんにちは。何か用ですか？";

    private bool isPlayerNear = false;
    private Transform player;

    void Start()
    {
        if (talkButton != null)
        {
            talkButton.SetActive(false);
            talkButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(TalkToNPC);
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
            if (talkButton != null) talkButton.SetActive(true);
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
        UIManager.Instance.ShowDialogue($"{npcName}：{dialogueLine}");
    }

    public void UseMemory(string memoryContent)
    {
        Debug.Log($"{npcName} に記憶を使用：{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} に「{memoryContent}」を使った。");
    }

}
