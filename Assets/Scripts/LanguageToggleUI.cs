using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageToggleUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;

    private void Start()
    {
        if (languageDropdown == null)
        {
            Debug.LogError("LanguageDropdown が設定されていません！");
            return;
        }

        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new System.Collections.Generic.List<string> { "日本語", "English" });

        languageDropdown.value = LocalizationManager.Instance.GetCurrentLanguage() == Language.Japanese ? 0 : 1;
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }


    private void OnLanguageChanged(int index)
    {
        Language selectedLanguage = index == 0 ? Language.Japanese : Language.English;
        LocalizationManager.Instance.SetLanguage(selectedLanguage);

        // UI更新イベントを発行（登録されたUIが再描画）
        LocalizationEventBus.RaiseLanguageChanged(selectedLanguage);
    }
}