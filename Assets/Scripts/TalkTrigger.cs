using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    [Header("�ΏۂƂȂ�UI�{�^��")]
    [SerializeField] private GameObject talkButton;
    [Header("��b���")]
    [SerializeField] private string npcName = "NPC";
    [TextArea]
    [SerializeField] private string dialogueLine = "����ɂ��́B�����p�ł����H";

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
        UIManager.Instance.ShowDialogue($"{npcName}�F{dialogueLine}");
    }

    public void UseMemory(string memoryContent)
    {
        Debug.Log($"{npcName} �ɋL�����g�p�F{memoryContent}");
        UIManager.Instance.ShowDialogue($"{npcName} �Ɂu{memoryContent}�v���g�����B");
    }

}
