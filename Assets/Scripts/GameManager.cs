using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("�L�����N�^�[�o�^")]
    [SerializeField] private List<CharacterMemoryData> allCharacters;

    private List<TurnDecision> decisionLogs = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadCharacterMemoryData();
    }

    /// <summary>
    /// �w��t�H���_����CharacterMemoryData�������ǂݍ���
    /// </summary>
    private void LoadCharacterMemoryData()
    {
        allCharacters = new List<CharacterMemoryData>();
        CharacterMemoryData[] loaded = Resources.LoadAll<CharacterMemoryData>("Story/Characters");
        allCharacters.AddRange(loaded);
        Debug.Log($"�ǂݍ��񂾃L�����N�^�[��: {allCharacters.Count}");
    }

    public List<CharacterMemoryData> GetAllCharacters()
    {
        return new List<CharacterMemoryData>(allCharacters);
    }

    public void AddDecisionLog(TurnDecision decision)
    {
        decisionLogs.Add(decision);
        Debug.Log($"[���O�L�^] Turn {decision.turn}: {decision.selectedMemoryOwner.characterName}�̋L����{decision.targetCharacter.characterName}�Ɏg�p");
    }

    public List<TurnDecision> GetDecisionLogs()
    {
        return new List<TurnDecision>(decisionLogs);
    }

    public void ResetGame()
    {
        decisionLogs.Clear();
    }
}
