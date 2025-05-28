using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI�Q��")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource audioSource;

    [Header("�f�[�^�p�X")]
    [SerializeField] private string resourceFolderPath = "EndingPattern"; // Resources/EndingPattern/

    private void Start()
    {
        LoadAndApplyEndingData();
    }

    private void LoadAndApplyEndingData()
    {
        string endingId = GameManager.Instance.GetEndingType(); // ��: "TRUE_END"
        if (string.IsNullOrEmpty(endingId))
        {
            Debug.LogError("Ending ID ���擾�ł��܂���ł����B");
            return;
        }

        // Resources/EndingPattern/TRUE_END.asset �Ȃǂ�ǂݍ���
        EndingData data = Resources.Load<EndingData>($"{resourceFolderPath}/{endingId}");

        if (data == null)
        {
            Debug.LogError($"EndingData ��������܂���: {resourceFolderPath}/{endingId}");
            return;
        }

        // UI���f
        if (titleText != null) titleText.text = data.title;
        if (descriptionText != null) descriptionText.text = data.description;
        if (backgroundImage != null && data.backgroundImage != null)
            backgroundImage.sprite = data.backgroundImage;
        if (audioSource != null && data.endingBGM != null)
        {
            audioSource.clip = data.endingBGM;
            audioSource.Play();
        }
    }
}
