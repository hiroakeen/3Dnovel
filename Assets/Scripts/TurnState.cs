using System.Collections.Generic;
using UnityEngine;

public class TurnState : IGameState
{
    private GameStateManager manager;
    private int turnNumber;

    private List<CharacterMemoryData> selectedCharacters = new();
    private CharacterMemoryData selectedMemorySource;
    private CharacterMemoryData selectedActionTarget;

    private bool memoryUsed = false;

    public TurnState(GameStateManager manager, int turnNumber)
    {
        this.manager = manager;
        this.turnNumber = turnNumber;
    }

    public void Enter()
    {
        Debug.Log($"Turn {turnNumber} �J�n");
        selectedCharacters.Clear();
        memoryUsed = false;
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} �I��");
    }

    public void NotifyCharacterTalked(CharacterMemoryData character)
    {
        if (!selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            Debug.Log($"{character.characterName} �Ɖ�b�i�^�[�� {turnNumber}�j");
        }

        CheckTurnProgress();
    }

    public void NotifyMemoryUsed(CharacterMemoryData memorySource, CharacterMemoryData target)
    {
        if (memoryUsed) return; // 1��̂ݗL��

        selectedMemorySource = memorySource;
        selectedActionTarget = target;
        memoryUsed = true;

        var log = new TurnDecision(turnNumber, memorySource, target);
        GameManager.Instance.AddDecisionLog(log);

        CheckTurnProgress();
    }

    private void CheckTurnProgress()
    {
        if (selectedCharacters.Count >= 3 && memoryUsed)
        {
            if (turnNumber >= 3)
            {
                manager.ChangeState(new EvaluateState(manager));
            }
            else
            {
                manager.ChangeState(new TurnState(manager, turnNumber + 1));
            }
        }
    }
}

// �ȗ������ꂽ TurnDecision �N���X
public class TurnDecision
{
    public int turn;
    public CharacterMemoryData selectedMemoryOwner;
    public CharacterMemoryData targetCharacter;

    public TurnDecision(int turn, CharacterMemoryData owner, CharacterMemoryData target)
    {
        this.turn = turn;
        this.selectedMemoryOwner = owner;
        this.targetCharacter = target;
    }
}
