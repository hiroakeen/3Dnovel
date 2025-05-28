using UnityEngine;
using UnityEditor;

public class EndingDataGenerator
{
    [MenuItem("Tools/�o�Y�Q�[��/EndingData ��V�d�l�Ő���")]
    public static void GenerateEndingDataAssets()
    {
        string folderPath = "Assets/EndingPattern";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "EndingPattern");
        }

        CreateEndingData("TRUE_END", "�^���̒E�o", "���Ȃ��͑S�ẲR���������A�F�Ƌ��ɕ������o���B", folderPath);
        CreateEndingData("GOOD_END", "�ɂ����E�o", "���Ȃ��͑����̐^����͂񂾂��A�^���ɂ͓͂��Ȃ������B", folderPath);
        CreateEndingData("BAD_END", "�����ꂽ�I��", "�����m��Ȃ��܂܁A���Ȃ������͕����߂�ꂽ�B", folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("�V�d�l�Ɋ�Â� EndingData �� 3�퐶�����܂����B");
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
