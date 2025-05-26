using UnityEngine;

[System.Serializable]
public class LocalizedString
{
    [TextArea] public string jp;
    [TextArea] public string en;

    public LocalizedString(string jp, string en)
    {
        this.jp = jp;
        this.en = en;
    }

    public string GetLocalized(Language lang)
    {
        return lang == Language.Japanese ? jp : en;
    }
}

public enum Language
{
    Japanese,
    English
}


