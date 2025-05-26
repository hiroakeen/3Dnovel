using UnityEditor;
using UnityEngine;
using System.Linq;

public class CharacterDataAutoAssigner : MonoBehaviour
{
    [MenuItem("Tools/自動割当/すべてのNPCにキャラデータを割り当て")]
    public static void AssignCharacterDataToAllTalkTriggers()
    {
        JsonCharacterLoader loader = Object.FindAnyObjectByType<JsonCharacterLoader>();
        if (loader == null || loader.LoadedCharacters == null)
        {
            Debug.LogError("JsonCharacterLoader が見つからない、またはキャラデータが読み込まれていません。");
            return;
        }

        int assignedCount = 0;

        // 新しい形式に変更（高速・非ソート）
        var allTalkTriggers = Object.FindObjectsByType<TalkTrigger>(FindObjectsSortMode.None);

        foreach (var talkTrigger in allTalkTriggers)
        {
            string objectName = talkTrigger.gameObject.name.Replace("NPC_", "").Trim();

            var matchedCharacter = loader.LoadedCharacters
                .FirstOrDefault(c => c.name == objectName);

            if (matchedCharacter != null)
            {
                Undo.RecordObject(talkTrigger, "Assign CharacterData");
                talkTrigger.GetType()
                    .GetField("characterData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(talkTrigger, matchedCharacter);

                EditorUtility.SetDirty(talkTrigger);
                assignedCount++;
            }
            else
            {
                Debug.LogWarning($"一致するキャラデータが見つかりませんでした: {objectName}");
            }
        }

        Debug.Log($"キャラデータの割当が完了しました。合計: {assignedCount} 件");
    }
}
