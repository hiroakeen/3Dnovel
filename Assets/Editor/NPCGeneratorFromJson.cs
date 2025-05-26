using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class NPCGeneratorFromJson : MonoBehaviour
{
    private const string prefabPath = "Assets/Prefabs/NPC_Base.prefab";
    private const string parentName = "GeneratedNPCs";

    [MenuItem("Tools/自動生成/NPCをJSONから生成（完全版）")]
    public static void GenerateNPCsFromJson()
    {
        // JsonCharacterLoader を探す
        var loader = Object.FindAnyObjectByType<JsonCharacterLoader>();
        if (loader == null)
        {
            Debug.LogError("JsonCharacterLoader が見つかりません。Scene に配置されていますか？");
            return;
        }

        // LoadCharactersFromJson() をリフレクションで呼び出す（AwakeがEditorで走らないため）
        MethodInfo loadMethod = typeof(JsonCharacterLoader).GetMethod("LoadCharactersFromJson", BindingFlags.NonPublic | BindingFlags.Instance);
        loadMethod?.Invoke(loader, null);

        if (loader.LoadedCharacters == null || loader.LoadedCharacters.Count == 0)
        {
            Debug.LogError("キャラクターが読み込まれていません。characters.json の中身を確認してください。");
            return;
        }

        // NPCのベースプレハブを読み込む
        string[] guids = AssetDatabase.FindAssets("NPC_Base t:prefab");
        if (guids.Length == 0)
        {
            Debug.LogError("NPC_Base プレハブが見つかりません。Assets/Prefabs にありますか？");
            return;
        }

        string actualPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        GameObject basePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(actualPath);


        // 既存の親オブジェクトがあれば削除して再生成
        var existingParent = GameObject.Find(parentName);
        if (existingParent != null)
        {
            Undo.DestroyObjectImmediate(existingParent);
        }

        GameObject parent = new GameObject(parentName);
        Undo.RegisterCreatedObjectUndo(parent, "Create NPC Parent");

        float startX = 0f;
        float startZ = 0f;
        float spacingX = 2.5f;
        float spacingZ = 2.5f;
        int rowLimit = 5;

        int created = 0;

        foreach (var character in loader.LoadedCharacters)
        {
            int row = created / rowLimit;
            int col = created % rowLimit;

            Vector3 position = new Vector3(startX + spacingX * col, 0f, startZ + spacingZ * row);

            GameObject npc = (GameObject)PrefabUtility.InstantiatePrefab(basePrefab, parent.transform);
            Undo.RegisterCreatedObjectUndo(npc, "Create NPC");

            npc.name = "NPC_" + character.name;
            npc.transform.position = position;

            // TalkTriggerにキャラデータを代入
            var talkTrigger = npc.GetComponent<TalkTrigger>();
            if (talkTrigger != null)
            {
                Undo.RecordObject(talkTrigger, "Assign CharacterData");
                typeof(TalkTrigger).GetField("characterData", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(talkTrigger, character);
                EditorUtility.SetDirty(talkTrigger);
            }
            else
            {
                Debug.LogWarning($"TalkTrigger が {npc.name} にアタッチされていません。");
            }

            created++;
        }

        Debug.Log($" {created} 体のNPCを生成し、{parentName} に配置しました。");
    }
}
