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

        CreateEndingData("TRUE_END", "�^���̒E�o", "���Ȃ��͑S�ẲR���������A�F�Ƌ��ɕ������o���B", folderPath);
        CreateEndingData("GOOD_END", "�ɂ����E�o", "���Ȃ��͑����̐^����͂񂾂��A�^���ɂ͓͂��Ȃ������B", folderPath);
        CreateEndingData("BAD_END_A", "�����ꂽ�I��", "�����m��Ȃ��܂܁A���Ȃ������͕����߂�ꂽ�B", folderPath);
        CreateEndingData("BAD_END_B", "�]���̉ʂ�", "�N��������ɋ]���ƂȂ�A���Ȃ��͐������т��B", folderPath);
        CreateEndingData("BAD_END_C", "���߂̐Î�", "�N���~���Ȃ������B���ׂĂ𓊂��o���Ă��܂����B", folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("EndingData �� 5�퐶�����܂����B");
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
