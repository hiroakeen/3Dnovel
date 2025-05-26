// Localized�Ή���CharacterMemoryDataGenerator�i���{��{�p��j

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

        CreateCharacter("�J�{ ���E�J",
            new LocalizedString("�����X�C�b�`�������ƁA�N��������", "If I push the switch, someone will die"),
            true, false, CharacterRoleType.Trigger,
            MemoryReactionType.True,
            new LocalizedString("�c�c�v���o�����B���������΁A�݂�Ȃ��o����c�c�I", "...I remember. If I push it, everyone can escape..."),
            new LocalizedString("�������ɁA����͌����L���������", "Yes, I do remember seeing that"),
            new LocalizedString("�c�c����́A�Ⴄ�C������", "...That doesn't feel right"),
            new LocalizedString[]
            {
                new LocalizedString("�c�c�b�������Ȃ��ŁB�܂��A�v���o���Ȃ�", "Don't talk to me... I can't remember yet"),
                new LocalizedString("�����c�c�v���o���Ă����B�����A�Ԃ��c�X�C�b�`�H", "I'm starting to remember... something red... a switch?"),
                new LocalizedString("���������΁c�c�I���B����ȋC�������", "If I push it... it will end. I just know it.")
            },
            folderPath);

        CreateCharacter("���_ ���i",
            new LocalizedString("�����ǂ̂ǂ����Ɂg�Ԃ��X�C�b�`�h������", "There's a red switch somewhere on the white wall"),
            false, true, CharacterRoleType.Persuade,
            MemoryReactionType.Bad,
            new LocalizedString("", ""),
            new LocalizedString("����͊m���Ɂc�c������������", "Yes, that was definitely suspicious"),
            new LocalizedString("�Ԃ��X�C�b�`�c�c�������ƂȂ���", "Red switch...? I haven't seen it"),
            new LocalizedString[]
            {
                new LocalizedString("�ł炸�A�悭���āB�ǂ����Ƀq���g������͂���", "Don't panic. There has to be a clue somewhere."),
                new LocalizedString("�ǁc�c�Ԃ��X�C�b�`�������C�������", "The wall... I think I saw a red switch."),
                new LocalizedString("�˂��A�N�������Ă��Ă���Ȃ��H", "Hey, can someone go press it?")
            },
            folderPath);

        CreateCharacter("�܏\�� �W���[",
            new LocalizedString("�o�����J����ɂ͎����̔ԍ����K�v������C������", "I think you need your number to open the exit"),
            false, false, CharacterRoleType.Investigate,
            MemoryReactionType.Good,
            new LocalizedString("", ""),
            new LocalizedString("����c�c���̔ԍ����H�������Ɏg���������", "This... my number? Might be useful"),
            new LocalizedString("�ԍ��c�c���ꂪ�ǂ������H", "A number...? What's the point?"),
            new LocalizedString[]
            {
                new LocalizedString("���̕����A�}�W�ŒE�o�Q�[����I", "This room... is this some kind of escape game?!"),
                new LocalizedString("����H�|�P�b�g�Ɂc�c�����̃^�O������H", "Huh? There's a tag with a number in my pocket?"),
                new LocalizedString("�����s���B�ԍ��A�g���邩�����Ă���I", "I'll go. Let me try if the number works!")
            },
            folderPath);

        CreateCharacter("���V �J�Y�}",
            new LocalizedString("���̕����ɂ͗�������B�d�͂��ς�����C������", "There's something behind this room. I felt the gravity shift"),
            true, false, CharacterRoleType.Investigate,
            MemoryReactionType.True,
            new LocalizedString("�����A�������B���̎��c�c���̉��ŁA�m���ɗh���������", "Yeah... at that moment, I felt it shake beneath the floor"),
            new LocalizedString("�m���ɂ��̊��o�͂�����", "Yes, I had that sensation too"),
            new LocalizedString("�c�c����͉��̋L���Ƃ͈Ⴄ", "...That's not how I remember it"),
            new LocalizedString[]
            {
                new LocalizedString("�����̍\���A���ɕs���R���Ǝv��Ȃ����H", "Don't you think this room's layout is a bit off?"),
                new LocalizedString("�㉺���o���Y����u�Ԃ��������B�������邩������Ȃ�", "There was a moment I lost my sense of direction... maybe there's something below"),
                new LocalizedString("������c�c���̉��Ɂg����1�̕����h���H", "Could it be... another room beneath the floor?")
            },
            folderPath);

        CreateCharacter("�e�I",
            new LocalizedString("�l�͂��̒��ŗB��A�Q���҂���Ȃ�", "I'm the only one here who isn't a participant"),
            false, false, CharacterRoleType.Unknown,
            MemoryReactionType.Bad,
            new LocalizedString("", ""),
            new LocalizedString("�����A����́c�l�̖����������Ă�̂���", "Ah... maybe that shows what my role really is"),
            new LocalizedString("�l�̋L���ɂ́A����͂Ȃ���", "I don't recall that in my memory"),
            new LocalizedString[]
            {
                new LocalizedString("�����ɂ́c�c���x�������C������", "I feel like... I've been here many times"),
                new LocalizedString("�l�̖����́c�c�Ȃ񂾂�������", "What was my role again...?"),
                new LocalizedString("�l���g���Ȃ�A�����Ō�̃`�����X����", "If you're going to use me, now's your last chance")
            },
            folderPath);

        CreateCharacter("�v���C���[",
            new LocalizedString("�N���̔w���ɐ����́g0�h��������", "I saw the number '0' on someone's back"),
            true, false, CharacterRoleType.Unknown,
            MemoryReactionType.True,
            new LocalizedString("�g0�h�c�c����͍ŏ��̔ԍ��B�S������蒼����\��������", "'0'... it's the starting number. It might let us reset everything"),
            new LocalizedString("�����A�������Ɂg0�h�������L��������", "Yeah, I definitely remember seeing '0'"),
            new LocalizedString("�g0�h�H�c�c������ďd�v�Ȃ̂��H", "'0'? ...Is that really important?"),
            new LocalizedString[]
            {
                new LocalizedString("�ڂ��o�߂����c�c�N���̔w���Ɂg0�h��", "When I woke up... I saw '0' on someone's back"),
                new LocalizedString("0���āc�c�����Ӗ����Ă�H", "What does '0' mean...?"),
                new LocalizedString("����́g���Z�b�g�h�̋L���c�c�H", "Is this... a reset symbol?")
            },
            folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Localized��CharacterMemoryData��S�L�������������܂����B");
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
