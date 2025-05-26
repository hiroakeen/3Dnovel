using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageToggleUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;

    private void Start()
    {
        // �����ݒ�i�h���b�v�_�E���̕\������j
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(new System.Collections.Generic.List<string> { "���{��", "English" });

        // ���݂̌����UI�ɔ��f
        languageDropdown.value = LocalizationManager.Instance.GetCurrentLanguage() == Language.Japanese ? 0 : 1;
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index)
    {
        Language selectedLanguage = index == 0 ? Language.Japanese : Language.English;
        LocalizationManager.Instance.SetLanguage(selectedLanguage);

        // UI�X�V�C�x���g�𔭍s�i�o�^���ꂽUI���ĕ`��j
        LocalizationEventBus.RaiseLanguageChanged(selectedLanguage);
    }
}