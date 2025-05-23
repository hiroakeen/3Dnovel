using System.Collections.Generic;
using UnityEngine;

public class TurnState : IGameState
{
    private GameStateManager manager;
    private int turnNumber;

    private List<CharacterMemoryData> selectedCharacters = new();
    private List<CharacterMemoryData> availableCharacters;

    private CharacterMemoryData selectedMemorySource;
    private CharacterMemoryData selectedActionTarget;

    public TurnState(GameStateManager manager, int turnNumber)
    {
        this.manager = manager;
        this.turnNumber = turnNumber;
    }

    public void Enter()
    {
        Debug.Log($"Turn {turnNumber} �J�n");
        selectedCharacters.Clear();
        availableCharacters = GameManager.Instance.GetAllCharacters();

        // UI�ŃL�����N�^�[�I�����J�n
        UIManager.Instance.ShowCharacterSelection(availableCharacters, OnCharacterSelected);
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} �I��");
    }

    private void OnCharacterSelected(CharacterMemoryData character)
    {
        if (!selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            string dialogue = character.GetDialogueForTurn(turnNumber);
            UIManager.Instance.ShowDialogue(dialogue);
        }

        if (selectedCharacters.Count >= 3)
        {
            PromptForMemoryUsage();
        }
    }

    private void PromptForMemoryUsage()
    {
        // UI�ŋL���I����ʂɑJ��
        UIManager.Instance.ShowMemorySelection(selectedCharacters, OnMemoryChosen);
    }

    private void OnMemoryChosen(CharacterMemoryData memoryOwner)
    {
        selectedMemorySource = memoryOwner;
        List<CharacterMemoryData> targets = new(availableCharacters);
        targets.Remove(memoryOwner); // �������g�ȊO�Ɏg�킹��O��

        UIManager.Instance.ShowTargetSelection(targets, OnTargetChosen);
    }

    private void OnTargetChosen(CharacterMemoryData target)
    {
        selectedActionTarget = target;

        // ���O�Ƃ��ċL�^
        var log = new TurnDecision
        {
            turn = turnNumber,
            talkedCharacters = selectedCharacters.ToArray(),
            selectedMemoryOwner = selectedMemorySource,
            targetCharacter = selectedActionTarget
        };

        GameManager.Instance.AddDecisionLog(log);

        // ���̃^�[����
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

// �Q�l�FDecisionLog�\��
public class TurnDecision
{
    public int turn;
    public CharacterMemoryData[] talkedCharacters;
    public CharacterMemoryData selectedMemoryOwner;
    public CharacterMemoryData targetCharacter;
}