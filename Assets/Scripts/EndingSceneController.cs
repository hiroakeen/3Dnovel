using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource audioSource;

    [Header("データパス")]
    [SerializeField] private string resourceFolderPath = "EndingPattern"; // Resources/EndingPattern/

    private void Start()
    {
        LoadAndApplyEndingData();
    }

    private void LoadAndApplyEndingData()
    {
        string endingId = GameManager.Instance.GetEndingType(); // 例: "TRUE_END"
        if (string.IsNullOrEmpty(endingId))
        {
            Debug.LogError("Ending ID が取得できませんでした。");
            return;
        }

        // Resources/EndingPattern/TRUE_END.asset などを読み込む
        EndingData data = Resources.Load<EndingData>($"{resourceFolderPath}/{endingId}");

        if (data == null)
        {
            Debug.LogError($"EndingData が見つかりません: {resourceFolderPath}/{endingId}");
            return;
        }

        // UI反映
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
