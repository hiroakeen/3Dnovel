using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource audioSource;

    [Header("エンディングデータ")]
    [SerializeField] private EndingData trueEndingData;
    [SerializeField] private EndingData normalEndingData;
    [SerializeField] private EndingData badEndingData;

    void Start()
    {
        string endingId = EndingManager.LastEndingId;
        var data = GetEndingDataById(endingId);

        if (data != null)
        {
            if (titleText) titleText.text = data.title;
            if (descriptionText) descriptionText.text = data.description;
            if (backgroundImage && data.backgroundImage != null) backgroundImage.sprite = data.backgroundImage;
            if (audioSource && data.endingBGM != null)
            {
                audioSource.clip = data.endingBGM;
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Ending data not found for ID: " + endingId);
        }
    }

    private EndingData GetEndingDataById(string id)
    {
        return id switch
        {
            "TRUE_END" => trueEndingData,
            "GOOD_END" => normalEndingData,
            _ => badEndingData
        };
    }
}
