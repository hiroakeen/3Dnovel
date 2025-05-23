using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterMemoryDataGenerator
{
    [MenuItem("Tools/Generate Sample CharacterMemoryData")]
    public static void GenerateCharacters()
    {
        string storyPath = "Assets/Story";
        if (!AssetDatabase.IsValidFolder(storyPath))
        {
            AssetDatabase.CreateFolder("Assets", "Story");
        }

        string folderPath = storyPath + "/Characters";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(storyPath, "Characters");
        }


        CreateCharacter("雨宮 ユウカ", "私がスイッチを押すと、誰かが死ぬ", true, false, CharacterRoleType.Trigger,
            new string[] {
                "……話しかけないで。まだ、思い出せない",
                "少し……思い出してきた。何か、赤い…スイッチ？",
                "私が押せば……終わる。そんな気がするの"
            }, folderPath);

        CreateCharacter("東雲 レナ", "白い壁のどこかに“赤いスイッチ”がある", false, true, CharacterRoleType.Persuade,
            new string[] {
                "焦らず、よく見て。どこかにヒントがあるはずよ",
                "壁……赤いスイッチを見た気がするの",
                "ねぇ、誰か押してきてくれない？"
            }, folderPath);

        CreateCharacter("五十嵐 ジョー", "出口を開けるには自分の番号が必要やった気がする", false, false, CharacterRoleType.Investigate,
            new string[] {
                "この部屋、マジで脱出ゲーかよ！",
                "あれ？ポケットに……数字のタグがある？",
                "俺が行く。番号、使えるか試してくる！"
            }, folderPath);

        CreateCharacter("黒澤 カズマ", "この部屋には裏がある。重力が変わった気がする", true, false, CharacterRoleType.Investigate,
            new string[] {
                "部屋の構造、妙に不自然だと思わないか？",
                "上下感覚がズレる瞬間があった。裏があるかもしれない",
                "もしや……床の下に“もう1つの部屋”が？"
            }, folderPath);

        CreateCharacter("テオ", "僕はこの中で唯一、参加者じゃない", false, false, CharacterRoleType.Unknown,
            new string[] {
                "ここには……何度も来た気がする",
                "僕の役割は……なんだったっけ",
                "僕を使うなら、今が最後のチャンスだよ"
            }, folderPath);

        CreateCharacter("プレイヤー", "誰かの背中に数字の“0”が見えた", true, false, CharacterRoleType.Unknown,
            new string[] {
                "目が覚めた時……誰かの背中に“0”が",
                "0って……何を意味してる？",
                "これは“リセット”の記号……？"
            }, folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("6人分のCharacterMemoryDataを生成しました。");
    }

    private static void CreateCharacter(string name, string memory, bool isTrue, bool isLying, CharacterRoleType type, string[] dialogues, string folderPath)
    {
        var data = ScriptableObject.CreateInstance<CharacterMemoryData>();
        data.characterName = name;
        data.memoryFragment = memory;
        data.isMemoryTrue = isTrue;
        data.isLying = isLying;
        data.roleType = type;
        data.turnDialogues = new List<TurnDialogue>();

        for (int i = 0; i < dialogues.Length; i++)
        {
            data.turnDialogues.Add(new TurnDialogue { turn = i + 1, dialogueLine = dialogues[i] });
        }

        AssetDatabase.CreateAsset(data, $"{folderPath}/{name}.asset");
    }
}
