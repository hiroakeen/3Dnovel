using UnityEngine;

/// <summary>
/// �L���t�F�[�Y�F�S���ɋL����n���I�����玟�̃^�[���֐i�s�i�ŏI�^�[���Ȃ�G���f�B���O�ցj
/// </summary>
public class TurnState_MemoryPhase : ITurnState
{
    private int memoryUsedCount = 0;
    private int totalNPCs;

    public void OnStateEnter()
    {
        memoryUsedCount = 0;

        // ���݃^�[���擾
        int currentTurn = GameManager.Instance.GetTurn();

        // �Ώ�NPC���擾
        totalNPCs = GameManager.Instance.GetAllCharacters().Count;

        Debug.Log($"�yMemoryPhase�z�J�n�F�L���� {totalNPCs} �l�ɓn��");

        NarrationPlayer.Instance.PlayNarration(
            "�W�߂��L����N�ɓn���H���ׂĂ��q����Ί�]�͌����B",
            onComplete: null
        );

        UIManager.Instance.ShowTurnMessage($"�^�[�� {currentTurn}�F�L����n���i{totalNPCs}��܂Łj");
    }

    public void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory)
    {
        memoryUsedCount++;
        Debug.Log($"[MemoryPhase] �L���g�p: {from.name} �� {to.name}�i{memoryUsedCount}/{totalNPCs}�j");

        GameManager.Instance.AddDecisionLog(
            new TurnDecision(GameManager.Instance.GetTurn(), from, to, memory)
        );

        if (memoryUsedCount >= totalNPCs)
        {
            int currentTurn = GameManager.Instance.GetTurn();

            if (currentTurn >= 3)
            {
                NarrationPlayer.Instance.PlayNarration(
                    "��̐��F�Ō�̋L�����n���ꂽ�B�������}���鎞���B",
                    () => GameTurnStateManager.Instance.SetState(GameTurnState.EndingPhase)
                );
            }
            else
            {
                GameManager.Instance.IncrementTurn(); // �� ������ SetTurn() ���Ă�
                GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
            }
        }
    }


    public void OnStateExit() { }

    public void NotifyCharacterTalked(CharacterDataJson character) { }

    public void NotifyTalkFinished(CharacterDataJson character) { }
}
