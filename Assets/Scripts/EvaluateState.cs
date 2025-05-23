using System.Collections.Generic;
using UnityEngine;

public class EvaluateState : IGameState
{
    private GameStateManager manager;

    public EvaluateState(GameStateManager manager)
    {
        this.manager = manager;
    }

    public void Enter()
    {
        Debug.Log("EvaluateState: ���O��]�����ăG���f�B���O������");

        List<TurnDecision> logs = GameManager.Instance.GetDecisionLogs();

        // ���̕��������i���Ƃ�SO���\�j
        bool usedYuukaMemory = false;
        bool usedOnCorrectTarget = false;
        int trueMemoryCount = 0;

        foreach (var log in logs)
        {
            if (log.selectedMemoryOwner.characterName == "�J�{ ���E�J")
            {
                usedYuukaMemory = true;
            }

            if (log.selectedMemoryOwner.isMemoryTrue && log.selectedMemoryOwner != log.targetCharacter)
            {
                trueMemoryCount++;
            }

            if (log.selectedMemoryOwner.characterName == "�e�I" && log.targetCharacter.characterName == "���V �J�Y�}")
            {
                usedOnCorrectTarget = true;
            }
        }

        // ���򔻒�
        string endingId;
        if (usedYuukaMemory && usedOnCorrectTarget && trueMemoryCount >= 2)
        {
            endingId = "TRUE_END";
        }
        else if (trueMemoryCount >= 2)
        {
            endingId = "GOOD_END";
        }
        else
        {
            endingId = "BAD_END";
        }

        manager.ChangeState(new EndingState(manager, endingId));
    }

    public void Exit()
    {
        Debug.Log("EvaluateState �I��");
    }
}
