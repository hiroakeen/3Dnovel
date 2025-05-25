using UnityEngine;
using UnityEditor;

public class EndingDataGenerator
{
    [MenuItem("Tools/Generate EndingData Samples")]
    public static void GenerateEndingDataAssets()
    {
        string folderPath = "Assets/EndingPattern";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "EndingPattern");
        }

        CreateEndingData("TRUE_END", "真実の脱出", "あなたは全ての嘘を見抜き、皆と共に部屋を出た。", folderPath);
        CreateEndingData("GOOD_END", "惜しい脱出", "あなたは多くの真実を掴んだが、真実には届かなかった。", folderPath);
        CreateEndingData("BAD_END_A", "閉ざされた終焉", "何も知らないまま、あなたたちは閉じ込められた。", folderPath);
        CreateEndingData("BAD_END_B", "犠牲の果て", "誰かが代わりに犠牲となり、あなたは生き延びた。", folderPath);
        CreateEndingData("BAD_END_C", "諦めの静寂", "誰も救えなかった。すべてを投げ出してしまった。", folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("EndingData を 5種生成しました。");
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
