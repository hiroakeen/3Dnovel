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


        CreateCharacter("�J�{ ���E�J", "�����X�C�b�`�������ƁA�N��������", true, false, CharacterRoleType.Trigger,
            new string[] {
                "�c�c�b�������Ȃ��ŁB�܂��A�v���o���Ȃ�",
                "�����c�c�v���o���Ă����B�����A�Ԃ��c�X�C�b�`�H",
                "���������΁c�c�I���B����ȋC�������"
            }, folderPath);

        CreateCharacter("���_ ���i", "�����ǂ̂ǂ����Ɂg�Ԃ��X�C�b�`�h������", false, true, CharacterRoleType.Persuade,
            new string[] {
                "�ł炸�A�悭���āB�ǂ����Ƀq���g������͂���",
                "�ǁc�c�Ԃ��X�C�b�`�������C�������",
                "�˂��A�N�������Ă��Ă���Ȃ��H"
            }, folderPath);

        CreateCharacter("�܏\�� �W���[", "�o�����J����ɂ͎����̔ԍ����K�v������C������", false, false, CharacterRoleType.Investigate,
            new string[] {
                "���̕����A�}�W�ŒE�o�Q�[����I",
                "����H�|�P�b�g�Ɂc�c�����̃^�O������H",
                "�����s���B�ԍ��A�g���邩�����Ă���I"
            }, folderPath);

        CreateCharacter("���V �J�Y�}", "���̕����ɂ͗�������B�d�͂��ς�����C������", true, false, CharacterRoleType.Investigate,
            new string[] {
                "�����̍\���A���ɕs���R���Ǝv��Ȃ����H",
                "�㉺���o���Y����u�Ԃ��������B�������邩������Ȃ�",
                "������c�c���̉��Ɂg����1�̕����h���H"
            }, folderPath);

        CreateCharacter("�e�I", "�l�͂��̒��ŗB��A�Q���҂���Ȃ�", false, false, CharacterRoleType.Unknown,
            new string[] {
                "�����ɂ́c�c���x�������C������",
                "�l�̖����́c�c�Ȃ񂾂�������",
                "�l���g���Ȃ�A�����Ō�̃`�����X����"
            }, folderPath);

        CreateCharacter("�v���C���[", "�N���̔w���ɐ����́g0�h��������", true, false, CharacterRoleType.Unknown,
            new string[] {
                "�ڂ��o�߂����c�c�N���̔w���Ɂg0�h��",
                "0���āc�c�����Ӗ����Ă�H",
                "����́g���Z�b�g�h�̋L���c�c�H"
            }, folderPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("6�l����CharacterMemoryData�𐶐����܂����B");
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
