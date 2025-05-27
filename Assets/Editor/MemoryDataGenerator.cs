
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MemoryDataGenerator : MonoBehaviour
{
    [MenuItem("Tools/自動生成/MemoryData を characters.json から生成（修正版）")]
    public static void GenerateMemoryDataFromJson()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Stage1", "characters.json");
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("characters.json が見つかりません: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        CharacterListWrapper wrapper = JsonUtility.FromJson<CharacterListWrapper>(json);
        if (wrapper == null || wrapper.characters == null)
        {
            Debug.LogError("JSONの読み込みに失敗しました。構造を確認してください。");
            return;
        }

        string outputFolder = "Assets/Resources/MemoryDataFolder";
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        int created = 0;

        foreach (var character in wrapper.characters)
        {
            foreach (var entry in character.grantedMemoriesPerTurn)
            {
                string memoryId = entry.memoryId;
                if (string.IsNullOrEmpty(memoryId))
                {
                    Debug.LogWarning($"キャラ {character.name} のターン {entry.turn} に memoryId がありません。スキップします。");
                    continue;
                }

                MemoryData asset = ScriptableObject.CreateInstance<MemoryData>();
                asset.id = memoryId;
                asset.memoryText = character.memoryFragmentJP; // 必要ならターンごとに分けてもOK
                asset.ownerCharacterId = character.id;

                string assetPath = Path.Combine(outputFolder, memoryId + ".asset");
                AssetDatabase.CreateAsset(asset, assetPath);
                created++;
            }
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($" MemoryData を {created} 件自動生成しました。");
    }

    [System.Serializable]
    public class CharacterListWrapper
    {
        public List<CharacterDataJson> characters;
    }

    [System.Serializable]
    public class CharacterDataJson
    {
        public string id;
        public string name;
        public string memoryFragmentJP;
        public List<GrantedMemoryPerTurn> grantedMemoriesPerTurn;
    }

    [System.Serializable]
    public class GrantedMemoryPerTurn
    {
        public int turn;
        public string memoryId;
    }
}
