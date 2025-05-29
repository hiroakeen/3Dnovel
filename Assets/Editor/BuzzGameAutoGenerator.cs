using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// バズゲーム用のEditor自動生成ツール（NPC生成・キャラ割り当て・記憶生成）
/// </summary>
public class BuzzGameAutoGenerator : MonoBehaviour
{
    private static readonly Dictionary<string, string> defaultReactionsCorrect = new()
    {
        {"theo", "……それで、僕に何を伝えたいの？"},
        {"minato", "これ、どこかで見た気がする！"},
        {"rin", "……これは……。私のこと？」"},
        {"yuu", "あっ……思い出したかもしれない……！"},
        {"ai", "……これが、私？"}
    };

    private static readonly Dictionary<string, string> defaultReactionsIncorrect = new()
    {
        {"theo", "君、何か勘違いしていない？"},
        {"minato", "うーん……ちょっと違うかも。"},
        {"rin", "嘘。そんな記憶、知らない。"},
        {"yuu", "これは……私じゃないと思う。"},
        {"ai", "………………（反応がない）"}
    };

    // ===============================================================
    // NPC生成
    // ===============================================================
    [MenuItem("Tools/バズゲーム/1. JSONからNPCを生成")]
    public static void GenerateNPCsFromJson()
    {
        var loader = Object.FindAnyObjectByType<JsonCharacterLoader>();
        if (loader == null)
        {
            Debug.LogError("JsonCharacterLoader が見つかりません。Scene に配置されていますか？");
            return;
        }

        MethodInfo loadMethod = typeof(JsonCharacterLoader).GetMethod("LoadCharactersFromJson", BindingFlags.NonPublic | BindingFlags.Instance);
        loadMethod?.Invoke(loader, null);

        if (loader.LoadedCharacters == null || loader.LoadedCharacters.Count == 0)
        {
            Debug.LogError("キャラクターが読み込まれていません。characters.json を確認してください。");
            return;
        }

        foreach (var character in loader.LoadedCharacters)
        {
            if (string.IsNullOrEmpty(character.reactionCorrectJP) && defaultReactionsCorrect.TryGetValue(character.id, out var correct))
                character.reactionCorrectJP = correct;

            if (string.IsNullOrEmpty(character.reactionIncorrectJP) && defaultReactionsIncorrect.TryGetValue(character.id, out var incorrect))
                character.reactionIncorrectJP = incorrect;
        }

        string[] guids = AssetDatabase.FindAssets("NPC_Base t:prefab");
        if (guids.Length == 0)
        {
            Debug.LogError("NPC_Base プレハブが見つかりません。Assets/Prefabs にありますか？");
            return;
        }

        string actualPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        GameObject basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(actualPath);

        GameObject parent = GameObject.Find("GeneratedNPCs");
        if (parent != null)
        {
            Undo.DestroyObjectImmediate(parent);
        }
        parent = new GameObject("GeneratedNPCs");
        Undo.RegisterCreatedObjectUndo(parent, "Create NPC Parent");

        float spacingX = 2.5f, spacingZ = 2.5f;
        int rowLimit = 5, created = 0;

        foreach (var character in loader.LoadedCharacters)
        {
            int row = created / rowLimit;
            int col = created % rowLimit;
            Vector3 pos = new Vector3(spacingX * col, 0f, spacingZ * row);

            GameObject npc = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab, parent.transform);
            Undo.RegisterCreatedObjectUndo(npc, "Create NPC");
            npc.name = "NPC_" + character.name;
            npc.transform.position = pos;

            var talkTrigger = npc.GetComponent<TalkTrigger>();
            if (talkTrigger != null)
            {
                typeof(TalkTrigger).GetField("characterData", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(talkTrigger, character);
                EditorUtility.SetDirty(talkTrigger);
            }

            created++;
        }

        Debug.Log($"{created} 体の NPC を生成しました。");
    }

    // ===============================================================
    // キャラデータ割り当て
    // ===============================================================
    [MenuItem("Tools/バズゲーム/2. NPCにキャラデータを割り当て")]
    public static void AssignCharacterDataToAllTalkTriggers()
    {
        JsonCharacterLoader loader = Object.FindAnyObjectByType<JsonCharacterLoader>();
        if (loader == null || loader.LoadedCharacters == null)
        {
            Debug.LogError("JsonCharacterLoader が見つからない、またはキャラデータ未ロードです。");
            return;
        }

        int count = 0;
        var allTriggers = Object.FindObjectsByType<TalkTrigger>(FindObjectsSortMode.None);
        foreach (var trigger in allTriggers)
        {
            string name = trigger.gameObject.name.Replace("NPC_", "").Trim();
            var character = loader.LoadedCharacters.FirstOrDefault(c => c.name == name);
            if (character != null)
            {
                Undo.RecordObject(trigger, "Assign CharacterData");
                typeof(TalkTrigger).GetField("characterData", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(trigger, character);
                EditorUtility.SetDirty(trigger);
                count++;
            }
            else
            {
                Debug.LogWarning($"キャラが見つかりません: {name}");
            }
        }

        Debug.Log($"キャラデータを {count} 件割り当てました。");
    }

    // ===============================================================
    // MemoryData生成
    // ===============================================================
    [MenuItem("Tools/バズゲーム/3. MemoryData を memory_data_list.json から生成")]
    public static void GenerateMemoryDataFromMemoryJson()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Stage1", "memory_data_list.json");
        if (!File.Exists(jsonPath))
        {
            Debug.LogError("memory_data_list.json が見つかりません: " + jsonPath);
            return;
        }

        string json = File.ReadAllText(jsonPath);
        MemoryListWrapper wrapper = JsonUtility.FromJson<MemoryListWrapper>(json);
        if (wrapper == null || wrapper.memories == null)
        {
            Debug.LogError("JSONのパースに失敗しました。構造を確認してください。");
            return;
        }

        string outputFolder = "Assets/Resources/MemoryDataFolder";
        if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

        int created = 0;
        foreach (var mem in wrapper.memories)
        {
            var asset = ScriptableObject.CreateInstance<MemoryData>();

            // 明示的に HideFlags を解除
            asset.hideFlags = HideFlags.None;

            asset.id = mem.id;
            asset.memoryText = mem.memoryText;
            asset.ownerCharacterId = mem.ownerCharacterId;
            asset.correctReceiverCharacterId = mem.correctReceiverCharacterId;
            asset.autoGrantedTurn = ParseTurnFromId(mem.id);
            asset.autoGrantedMemory = true;
            asset.originalOwner = null;

            string path = Path.Combine(outputFolder, mem.id + ".asset");
            AssetDatabase.CreateAsset(asset, path);
            created++;
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"MemoryData を {created} 件生成しました。");
    }

    private static int ParseTurnFromId(string id)
    {
        if (id.EndsWith("_t1")) return 1;
        if (id.EndsWith("_t2")) return 2;
        if (id.EndsWith("_t3")) return 3;
        return 0; 
    }


    [System.Serializable]
    public class MemoryListWrapper
    {
        public List<MemoryJsonData> memories;
    }

    [System.Serializable]
    public class MemoryJsonData
    {
        public string id;
        public string memoryText;
        public string ownerCharacterId;
        public string correctReceiverCharacterId;
    }

    [System.Serializable]
    public class CharacterListWrapper
    {
        public List<CharacterDataJson> characters;
    }
}
