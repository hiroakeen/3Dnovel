using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��b�t�F�[�Y�F3�l�Ɖ�b���I������L���t�F�[�Y�ֈڍs
/// </summary>
public class TurnState_TalkPhase : ITurnState
{
    private HashSet<string> talkedCharacterNames = new();
    private bool narrationShown = false;

    public void OnStateEnter()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;

        Debug.Log("�yTalkPhase�z�J�n�F�v���C���[��3�l�Ƙb�����Ƃ��ł��܂��B");
        UIManager.Instance.SetTurnMessage($"��{GameManager.Instance.GetTurn()}�^�[���F�N�ɘb��������H");
    }

    public void NotifyCharacterTalked(CharacterDataJson character)
    {
        // �b���������L������ID���L�^�i�d���h�~�j
        if (!talkedCharacterNames.Contains(character.id))
        {
            talkedCharacterNames.Add(character.id);
            Debug.Log($"[TalkPhase] �b��������: {character.name}�i���v: {talkedCharacterNames.Count}�l�j");
        }
    }

    public void NotifyTalkFinished(CharacterDataJson character)
    {
        // ��b�I����A3�l�ڂȂ�i���[�V������\�����ċL���t�F�[�Y�֑J��
        if (talkedCharacterNames.Count >= 3 && !narrationShown)
        {
            narrationShown = true;

            UIManager.Instance.ShowNarration(
                "��̐��F��ɓ��ꂽ�L����n�����Ԃ��B",
                () =>
                {
                    UIManager.Instance.SetTurnMessage("�ЂƂ��I��ŁA�L����n�����I");
                    GameTurnStateManager.Instance.SetState(GameTurnState.MemoryPhase);
                });
        }
    }

    public void ResetTalkLog()
    {
        talkedCharacterNames.Clear();
        narrationShown = false;
    }

    public void OnStateExit() { }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to) { }
}
