using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonCharacterLoader : MonoBehaviour
{
    [SerializeField] private string stageFolder = "Stage1";

    public List<CharacterDataJson> LoadedCharacters { get; private set; } = new();

    private void Awake()
    {
        LoadCharactersFromJson();
    }

    private void LoadCharactersFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, stageFolder, "characters.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"JSONファイルが見つかりません: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        Debug.Log($"JSON raw: {json}"); // ← ここを追加して中身確認

        CharacterListWrapper wrapper = JsonUtility.FromJson<CharacterListWrapper>(json);
        LoadedCharacters = wrapper.characters;

        if (LoadedCharacters == null || LoadedCharacters.Count == 0)
        {
            Debug.LogError("JSONは読み込まれましたが、キャラクターが空または構文エラーです。");
            return;
        }

        Debug.Log($"キャラ数: {LoadedCharacters.Count} 件読み込み成功");
    }

}

[System.Serializable]
public class CharacterDataJson
{
    public string id;
    public string name;
    public string roleType;
    public bool isMemoryUseTarget;

    public string memoryFragmentJP;
    public string memoryFragmentEN;

    public string reactionCorrectJP;
    public string reactionCorrectEN;
    public string reactionIncorrectJP;
    public string reactionIncorrectEN;

    public List<LocalizedTurnDialogueJson> turnDialogues;

    public string grantedOnTalkMemoryId;
    public string autoGrantedMemoryId;
    public string expectedMemoryId;

    public string GetDialogueForCurrentTurn(int turn, Language lang)
    {
        if (turnDialogues == null)
        {
            Debug.LogWarning($"[GetDialogueForCurrentTurn] turnDialogues が null（キャラ: {name}）");
            return "…………?";
        }

        foreach (var entry in turnDialogues)
        {
            if (entry.turn == turn)
            {
                return lang == Language.Japanese ? entry.dialogueJP : entry.dialogueEN;
            }
        }

        return "…………?";
    }

    [System.Serializable]
    public class MemoryGrantEntry
    {
        public int turn;
        public string memoryId;
    }

    public List<MemoryGrantEntry> grantedMemoriesPerTurn;
}

/// <summary>
/// 以下、多言語対応
/// </summary>

[System.Serializable]
public class LocalizedStringJson
{
    public string jp;
    public string en;
}

[System.Serializable]
public class LocalizedDialogueJson
{
    public int turn;
    public string jp;
    public string en;
}

[System.Serializable]
public class LocalizedTurnDialogueJson
{
    public int turn;
    public string dialogueJP;
    public string dialogueEN;
}


[System.Serializable]
public class CharacterListWrapper
{
    public List<CharacterDataJson> characters;
}

