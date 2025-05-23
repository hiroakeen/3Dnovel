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
        Debug.Log($"Turn {turnNumber} 開始");
        selectedCharacters.Clear();
        availableCharacters = GameManager.Instance.GetAllCharacters();

        // UIでキャラクター選択を開始
        UIManager.Instance.ShowCharacterSelection(availableCharacters, OnCharacterSelected);
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} 終了");
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
        // UIで記憶選択画面に遷移
        UIManager.Instance.ShowMemorySelection(selectedCharacters, OnMemoryChosen);
    }

    private void OnMemoryChosen(CharacterMemoryData memoryOwner)
    {
        selectedMemorySource = memoryOwner;
        List<CharacterMemoryData> targets = new(availableCharacters);
        targets.Remove(memoryOwner); // 自分自身以外に使わせる前提

        UIManager.Instance.ShowTargetSelection(targets, OnTargetChosen);
    }

    private void OnTargetChosen(CharacterMemoryData target)
    {
        selectedActionTarget = target;

        // ログとして記録
        var log = new TurnDecision
        {
            turn = turnNumber,
            talkedCharacters = selectedCharacters.ToArray(),
            selectedMemoryOwner = selectedMemorySource,
            targetCharacter = selectedActionTarget
        };

        GameManager.Instance.AddDecisionLog(log);

        // 次のターンへ
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

// 参考：DecisionLog構造
public class TurnDecision
{
    public int turn;
    public CharacterMemoryData[] talkedCharacters;
    public CharacterMemoryData selectedMemoryOwner;
    public CharacterMemoryData targetCharacter;
}