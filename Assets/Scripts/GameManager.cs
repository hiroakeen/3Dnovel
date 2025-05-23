using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("キャラクター登録")]
    [SerializeField] private List<CharacterMemoryData> allCharacters;

    private List<TurnDecision> decisionLogs = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadCharacterMemoryData();
    }

    /// <summary>
    /// 指定フォルダからCharacterMemoryDataを自動読み込み
    /// </summary>
    private void LoadCharacterMemoryData()
    {
        allCharacters = new List<CharacterMemoryData>();
        CharacterMemoryData[] loaded = Resources.LoadAll<CharacterMemoryData>("Story/Characters");
        allCharacters.AddRange(loaded);
        Debug.Log($"読み込んだキャラクター数: {allCharacters.Count}");
    }

    public List<CharacterMemoryData> GetAllCharacters()
    {
        return new List<CharacterMemoryData>(allCharacters);
    }

    public void AddDecisionLog(TurnDecision decision)
    {
        decisionLogs.Add(decision);
        Debug.Log($"[ログ記録] Turn {decision.turn}: {decision.selectedMemoryOwner.characterName}の記憶を{decision.targetCharacter.characterName}に使用");
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
