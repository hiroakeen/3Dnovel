using UnityEngine;
using UnityEditor;

public class EndingDataGenerator
{
    [MenuItem("Tools/バズゲーム/EndingData を新仕様で生成")]
    public static void GenerateEndingDataAssets()
    {
        string folderPath = "Assets/EndingPattern";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "EndingPattern");
        }

        CreateEndingData("TRUE_END", "真実の脱出", "あなたは全ての嘘を見抜き、皆と共に部屋を出た。", folderPath);
        CreateEndingData("GOOD_END", "惜しい脱出", "あなたは多くの真実を掴んだが、真実には届かなかった。", folderPath);
        CreateEndingData("BAD_END", "閉ざされた終焉", "何も知らないまま、あなたたちは閉じ込められた。", folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("新仕様に基づく EndingData を 3種生成しました。");
    }

    private static void CreateEndingData(string id, string title, string description, string folderPath)
    {
        EndingData data = ScriptableObject.CreateInstance<EndingData>();
        data.endingId = id;
        data.title = title;
        data.description = description;

        string assetPath = $"{folderPath}/{id}.asset";
        AssetDatabase.CreateAsset(data, assetPath);
    }
}
