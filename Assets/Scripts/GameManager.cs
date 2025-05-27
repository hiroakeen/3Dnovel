using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("�L�����N�^�[�o�^�iJSON�����j")]
    [SerializeField] private JsonCharacterLoader characterLoader;

    private List<TurnDecision> decisionLogs = new();

    [SerializeField] private int currentTurn = 1;
    public static int CurrentTurn => Instance != null ? Instance.currentTurn : 1;

    private bool isGameplayStarted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (characterLoader == null)
        {
            characterLoader = FindAnyObjectByType<JsonCharacterLoader>();
        }
    }

    /// <summary>
    /// �i���[�V�����I����ɌĂ΂��A�Q�[���{�҂̊J�n����
    /// </summary>
    public void StartGameplay()
    {
        if (isGameplayStarted) return;
        isGameplayStarted = true;

        Debug.Log("�Q�[���{�҃X�^�[�g�I");
        currentTurn = 1;
    }

    /// <summary>
    /// �o�^�ς݃L�����S�����擾�i�R�s�[�n���j
    /// </summary>
    public List<CharacterDataJson> GetAllCharacters()
    {
        return characterLoader != null
            ? new List<CharacterDataJson>(characterLoader.LoadedCharacters)
            : new List<CharacterDataJson>();
    }

    /// <summary>
    /// �L����ID����f�[�^���擾
    /// </summary>
    public CharacterDataJson FindCharacterById(string id)
    {
        return characterLoader != null
            ? characterLoader.LoadedCharacters.Find(c => c.id == id)
            : null;
    }

    public void AddDecisionLog(TurnDecision decision)
    {
        decisionLogs.Add(decision);
        Debug.Log($"[���O�L�^] Turn {decision.turn}: {decision.selectedMemoryOwner.name}�̋L����{decision.targetCharacter.name}�Ɏg�p");
    }

    public List<TurnDecision> GetDecisionLogs()
    {
        return new List<TurnDecision>(decisionLogs);
    }

    public void ResetGame()
    {
        decisionLogs.Clear();
        currentTurn = 1;
    }

    public void SetTurn(int turn)
    {
        currentTurn = turn;
    }

    public int GetTurn()
    {
        return currentTurn;
    }
}
