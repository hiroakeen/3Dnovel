using UnityEditor;
using UnityEngine;
using System.IO;

public class MemoryDataAutoGenerator : EditorWindow
{
    private string memoryId = "memory_001";
    private string memoryText = "思い出の風景";
    private Sprite memoryImage;
    private CharacterMemoryData ownerCharacter;
    private string savePath = "Assets/ScriptableObjects/MemoryData";

    [MenuItem("Tools/MemoryData Generator")]
    public static void OpenWindow()
    {
        GetWindow<MemoryDataAutoGenerator>("MemoryData Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("MemoryData 自動生成", EditorStyles.boldLabel);

        memoryId = EditorGUILayout.TextField("Memory ID", memoryId);
        memoryText = EditorGUILayout.TextField("Memory Text", memoryText);
        memoryImage = (Sprite)EditorGUILayout.ObjectField("Memory Image", memoryImage, typeof(Sprite), false);
        ownerCharacter = (CharacterMemoryData)EditorGUILayout.ObjectField("Owner Character", ownerCharacter, typeof(CharacterMemoryData), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("MemoryData を生成"))
        {
            GenerateMemoryData();
        }
    }

    private void GenerateMemoryData()
    {
        if (!AssetDatabase.IsValidFolder(savePath))
        {
            Directory.CreateDirectory(savePath);
            AssetDatabase.Refresh();
        }

        MemoryData memory = ScriptableObject.CreateInstance<MemoryData>();
        memory.memoryId = memoryId;
        memory.memoryText = memoryText;
        memory.memoryImage = memoryImage;
        memory.ownerCharacter = ownerCharacter;

        string fileName = $"{memoryId}_{ownerCharacter?.characterName ?? "NoOwner"}";
        string assetPath = Path.Combine(savePath, fileName + ".asset");

        AssetDatabase.CreateAsset(memory, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"MemoryData 生成: {assetPath}");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = memory;
    }
}
