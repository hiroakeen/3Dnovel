using TMPro;
using UnityEngine;

public class LocalizedTextUpdater : MonoBehaviour
{
    [SerializeField] private LocalizedString localizedText;
    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
        UpdateText(LocalizationManager.Instance.GetCurrentLanguage());
        LocalizationEventBus.OnLanguageChanged += UpdateText;
    }

    private void OnDestroy()
    {
        LocalizationEventBus.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText(Language lang)
    {
        if (label != null && localizedText != null)
        {
            label.text = localizedText.GetLocalized(lang);
        }
    }
}
