using UnityEditor;
using UnityEngine;
using System.IO;

public class MemoryDataFromCharacterGenerator : EditorWindow
{
    private string memorySavePath = "Assets/ScriptableObjects/MemoryData";
    private string characterDataPath = "Assets/ScriptableObjects/CharacterMemory";

    [MenuItem("Tools/Generate MemoryData from Characters")]
    public static void OpenWindow()
    {
        GetWindow<MemoryDataFromCharacterGenerator>("MemoryData Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("CharacterMemoryData → MemoryData 自動生成", EditorStyles.boldLabel);

        if (GUILayout.Button("全キャラクター分を生成"))
        {
            GenerateFromCharacterData();
        }
    }

    private void GenerateFromCharacterData()
    {
        string[] guids = AssetDatabase.FindAssets("t:CharacterMemoryData", new[] { characterDataPath });
        if (guids.Length == 0)
        {
            Debug.LogWarning("❗ CharacterMemoryData が見つかりません。パスを確認してください。");
            return;
        }

        if (!AssetDatabase.IsValidFolder(memorySavePath))
        {
            Directory.CreateDirectory(memorySavePath);
            AssetDatabase.Refresh();
        }

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var character = AssetDatabase.LoadAssetAtPath<CharacterMemoryData>(path);
            if (character == null) continue;

            // memoryId は真偽フラグで接尾辞
            string memoryIdBase = $"memory_{character.characterName}";
            string memoryId = character.isMemoryTrue ? memoryIdBase + "_true" : memoryIdBase + "_false";
            string fileName = $"{memoryId}.asset";
            string assetPath = Path.Combine(memorySavePath, fileName);

            // 既存チェック
            var existing = AssetDatabase.LoadAssetAtPath<MemoryData>(assetPath);
            if (existing != null)
            {
                Debug.Log($"🟡 既に存在：{fileName} → スキップして expectedMemory のみ更新");
                character.expectedMemory = existing;
                EditorUtility.SetDirty(character);
                continue;
            }

            // MemoryData 作成
            var memory = ScriptableObject.CreateInstance<MemoryData>();
            memory.memoryId = memoryId;
            memory.memoryText = character.memoryFragment;
            memory.memoryImage = character.portrait;
            memory.ownerCharacter = character;

            AssetDatabase.CreateAsset(memory, assetPath);

            // expectedMemory 更新（常に）
            character.expectedMemory = memory;

            // autoGrantedMemory が未設定ならセット（初期記憶用）
            if (character.autoGrantedMemory == null)
            {
                character.autoGrantedMemory = memory;
                Debug.Log($"📘 autoGrantedMemory も設定 → {character.characterName}");
            }

            EditorUtility.SetDirty(memory);
            EditorUtility.SetDirty(character);

            Debug.Log($"✅ 生成: {fileName}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("🎉 すべての MemoryData を生成・分類し、expectedMemory / autoGrantedMemory に反映しました！");
    }
}
