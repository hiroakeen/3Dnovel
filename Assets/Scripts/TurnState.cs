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
        Debug.Log($"Turn {turnNumber} 開始");
        selectedCharacters.Clear();
        memoryUsed = false;
    }

    public void Exit()
    {
        Debug.Log($"Turn {turnNumber} 終了");
    }

    public void NotifyCharacterTalked(CharacterMemoryData character)
    {
        if (!selectedCharacters.Contains(character))
        {
            selectedCharacters.Add(character);
            Debug.Log($"{character.characterName} と会話（ターン {turnNumber}）");
        }

        CheckTurnProgress();
    }

    public void NotifyMemoryUsed(CharacterMemoryData memorySource, CharacterMemoryData target)
    {
        if (memoryUsed) return; // 1回のみ有効

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

// 簡略化された TurnDecision クラス
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
