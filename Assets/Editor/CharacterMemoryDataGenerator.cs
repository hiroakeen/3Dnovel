// Localized対応のCharacterMemoryDataGenerator（日本語＋英語）

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CharacterMemoryDataGenerator
{
    [MenuItem("Tools/Generate Localized CharacterMemoryData")]
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

        CreateCharacter("雨宮 ユウカ",
            new LocalizedString("私がスイッチを押すと、誰かが死ぬ", "If I push the switch, someone will die"),
            true, false, CharacterRoleType.Trigger,
            MemoryReactionType.True,
            new LocalizedString("……思い出した。私が押せば、みんなが出られる……！", "...I remember. If I push it, everyone can escape..."),
            new LocalizedString("たしかに、それは見た記憶があるわ", "Yes, I do remember seeing that"),
            new LocalizedString("……それは、違う気がする", "...That doesn't feel right"),
            new LocalizedString[]
            {
                new LocalizedString("……話しかけないで。まだ、思い出せない", "Don't talk to me... I can't remember yet"),
                new LocalizedString("少し……思い出してきた。何か、赤い…スイッチ？", "I'm starting to remember... something red... a switch?"),
                new LocalizedString("私が押せば……終わる。そんな気がするの", "If I push it... it will end. I just know it.")
            },
            folderPath);

        CreateCharacter("東雲 レナ",
            new LocalizedString("白い壁のどこかに“赤いスイッチ”がある", "There's a red switch somewhere on the white wall"),
            false, true, CharacterRoleType.Persuade,
            MemoryReactionType.Bad,
            new LocalizedString("", ""),
            new LocalizedString("それは確かに……怪しかったわ", "Yes, that was definitely suspicious"),
            new LocalizedString("赤いスイッチ……見たことないわ", "Red switch...? I haven't seen it"),
            new LocalizedString[]
            {
                new LocalizedString("焦らず、よく見て。どこかにヒントがあるはずよ", "Don't panic. There has to be a clue somewhere."),
                new LocalizedString("壁……赤いスイッチを見た気がするの", "The wall... I think I saw a red switch."),
                new LocalizedString("ねぇ、誰か押してきてくれない？", "Hey, can someone go press it?")
            },
            folderPath);

        CreateCharacter("五十嵐 ジョー",
            new LocalizedString("出口を開けるには自分の番号が必要やった気がする", "I think you need your number to open the exit"),
            false, false, CharacterRoleType.Investigate,
            MemoryReactionType.Good,
            new LocalizedString("", ""),
            new LocalizedString("これ……俺の番号か？たしかに使えそうやな", "This... my number? Might be useful"),
            new LocalizedString("番号……それがどうした？", "A number...? What's the point?"),
            new LocalizedString[]
            {
                new LocalizedString("この部屋、マジで脱出ゲーかよ！", "This room... is this some kind of escape game?!"),
                new LocalizedString("あれ？ポケットに……数字のタグがある？", "Huh? There's a tag with a number in my pocket?"),
                new LocalizedString("俺が行く。番号、使えるか試してくる！", "I'll go. Let me try if the number works!")
            },
            folderPath);

        CreateCharacter("黒澤 カズマ",
            new LocalizedString("この部屋には裏がある。重力が変わった気がする", "There's something behind this room. I felt the gravity shift"),
            true, false, CharacterRoleType.Investigate,
            MemoryReactionType.True,
            new LocalizedString("ああ、そうだ。あの時……床の下で、確かに揺れを感じた", "Yeah... at that moment, I felt it shake beneath the floor"),
            new LocalizedString("確かにその感覚はあった", "Yes, I had that sensation too"),
            new LocalizedString("……それは俺の記憶とは違う", "...That's not how I remember it"),
            new LocalizedString[]
            {
                new LocalizedString("部屋の構造、妙に不自然だと思わないか？", "Don't you think this room's layout is a bit off?"),
                new LocalizedString("上下感覚がズレる瞬間があった。裏があるかもしれない", "There was a moment I lost my sense of direction... maybe there's something below"),
                new LocalizedString("もしや……床の下に“もう1つの部屋”が？", "Could it be... another room beneath the floor?")
            },
            folderPath);

        CreateCharacter("テオ",
            new LocalizedString("僕はこの中で唯一、参加者じゃない", "I'm the only one here who isn't a participant"),
            false, false, CharacterRoleType.Unknown,
            MemoryReactionType.Bad,
            new LocalizedString("", ""),
            new LocalizedString("ああ、それは…僕の役割を示してるのかも", "Ah... maybe that shows what my role really is"),
            new LocalizedString("僕の記憶には、それはないな", "I don't recall that in my memory"),
            new LocalizedString[]
            {
                new LocalizedString("ここには……何度も来た気がする", "I feel like... I've been here many times"),
                new LocalizedString("僕の役割は……なんだったっけ", "What was my role again...?"),
                new LocalizedString("僕を使うなら、今が最後のチャンスだよ", "If you're going to use me, now's your last chance")
            },
            folderPath);

        CreateCharacter("プレイヤー",
            new LocalizedString("誰かの背中に数字の“0”が見えた", "I saw the number '0' on someone's back"),
            true, false, CharacterRoleType.Unknown,
            MemoryReactionType.True,
            new LocalizedString("“0”……それは最初の番号。全員をやり直せる可能性がある", "'0'... it's the starting number. It might let us reset everything"),
            new LocalizedString("ああ、たしかに“0”を見た記憶がある", "Yeah, I definitely remember seeing '0'"),
            new LocalizedString("“0”？……それって重要なのか？", "'0'? ...Is that really important?"),
            new LocalizedString[]
            {
                new LocalizedString("目が覚めた時……誰かの背中に“0”が", "When I woke up... I saw '0' on someone's back"),
                new LocalizedString("0って……何を意味してる？", "What does '0' mean...?"),
                new LocalizedString("これは“リセット”の記号……？", "Is this... a reset symbol?")
            },
            folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("LocalizedなCharacterMemoryDataを全キャラ分生成しました。");
    }

    private static void CreateCharacter(
        string name,
        LocalizedString memory,
        bool isTrue, bool isLying, CharacterRoleType type,
        MemoryReactionType reactionType,
        LocalizedString reactionTrue, LocalizedString reactionGood, LocalizedString reactionFail,
        LocalizedString[] turnLines,
        string folderPath)
    {
        var data = ScriptableObject.CreateInstance<CharacterMemoryData>();
        data.characterName = name;
        data.memoryFragmentLocalized = memory;
        data.isMemoryTrue = isTrue;
        data.isLying = isLying;
        data.roleType = type;
        data.memoryReactionType = reactionType;
        data.memoryReactionTrueLocalized = reactionTrue;
        data.memoryReactionSuccessLocalized = reactionGood;
        data.memoryReactionFailLocalized = reactionFail;

        data.turnDialoguesLocalized = new List<LocalizedTurnDialogue>();
        for (int i = 0; i < turnLines.Length; i++)
        {
            data.turnDialoguesLocalized.Add(new LocalizedTurnDialogue { turn = i + 1, dialogueLine = turnLines[i] });
        }

        AssetDatabase.CreateAsset(data, $"{folderPath}/{name}.asset");
    }
}
