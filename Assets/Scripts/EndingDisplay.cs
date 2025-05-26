using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource bgmSource;

    private void Start()
    {
        string id = EndingManager.LastEndingId;
        EndingData data = Resources.Load<EndingData>($"EndingData/{id}");

        if (data != null)
        {
            titleText.text = data.title;
            descriptionText.text = data.description;

            if (backgroundImage != null && data.backgroundImage != null)
                backgroundImage.sprite = data.backgroundImage;

            if (bgmSource != null && data.endingBGM != null)
            {
                bgmSource.clip = data.endingBGM;
                bgmSource.Play();
            }
        }
        else
        {
            Debug.LogError($"EndingData not found for ID: {id}");
        }
    }
}
