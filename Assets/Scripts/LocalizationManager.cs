using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [Header("���݂̌���ݒ�")]
    public Language currentLanguage = Language.Japanese;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        Debug.Log($"����؂�ւ�: {lang}");
    }

    public Language GetCurrentLanguage()
    {
        return currentLanguage;
    }

    public string GetLocalized(LocalizedString str)
    {
        return str.GetLocalized(currentLanguage);
    }
}
