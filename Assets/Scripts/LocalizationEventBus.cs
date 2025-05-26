using System;

public static class LocalizationEventBus
{
    public static event Action<Language> OnLanguageChanged;

    public static void RaiseLanguageChanged(Language lang)
    {
        OnLanguageChanged?.Invoke(lang);
    }
}