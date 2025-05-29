using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("キャラクター登録（JSON方式）")]
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
    /// ナレーション終了後に呼ばれる、ゲーム本編の開始処理
    /// </summary>
    public void StartGameplay()
    {
        if (isGameplayStarted) return;
        isGameplayStarted = true;

        Debug.Log("ゲーム本編スタート！");
        currentTurn = 1;

        NarrationPlayer.Instance.PlayNarration(
            "謎の声：記憶を集め、誰かに渡せば出口が見えるかもしれない…",
            () =>
            {
                GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
            });
    }





    /// <summary>
    /// 登録済みキャラ全員を取得（コピー渡し）
    /// </summary>
    public List<CharacterDataJson> GetAllCharacters()
    {
        return characterLoader != null
            ? new List<CharacterDataJson>(characterLoader.LoadedCharacters)
            : new List<CharacterDataJson>();
    }

    /// <summary>
    /// キャラIDからデータを取得
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
        Debug.Log($"[ログ記録] Turn {decision.turn}: {decision.selectedMemoryOwner.name}の記憶を{decision.targetCharacter.name}に使用");
    }

    public List<TurnDecision> GetDecisionLogs()
    {
        return new List<TurnDecision>(decisionLogs);
    }

    /// <summary>
    /// エンディングタイプ（TRUE / GOOD / BAD）
    /// </summary>
    private string currentEndingType = "UNKNOWN";

    public void SetEndingType(string type)
    {
        currentEndingType = type;
        Debug.Log($"[GameManager] エンディングタイプを設定: {type}");
    }

    public string GetEndingType()
    {
        return currentEndingType;
    }

    public void ResetGame()
    {
        decisionLogs.Clear();
        currentTurn = 1;
        currentEndingType = "UNKNOWN";
    }

    public void SetTurn(int turn)
    {
        currentTurn = turn;

        MemoryManager.Instance.AutoGrantMemoriesForTurn(turn);
        foreach (var mem in MemoryManager.Instance.GetCollectedMemories())
        {
            var inventory = Object.FindFirstObjectByType<PlayerMemoryInventory>();
        }

        if (turn > 1)
        {
            // ナレーション表示（NarrationPlayer経由）
            NarrationPlayer.Instance.PlayNarration(
                $"謎の声：第{turn}ターンが始まった。",
                () => GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase)
            );
        }
        else
        {
            // 初回はナレーションPlayerのStart()で表示済み
            GameTurnStateManager.Instance.SetState(GameTurnState.TalkPhase);
        }


    }





    public int GetTurn()
    {
        return currentTurn;
    }

    public void IncrementTurn()
    {
        currentTurn++;
        Debug.Log($"[ターン進行] 現在のターン: {currentTurn}");

        if (currentTurn > 3)
        {
            EvaluateEnding(); 
        }
        else
        {
            SetTurn(currentTurn); 
        }
    }

    public int GetTotalNPCCount()
    {
        return GetAllCharacters().Count;
    }



    /// <summary>
    /// 記憶使用の正解数に基づいてエンディングを判定する
    /// </summary>
    public void EvaluateEnding()
    {
        List<TurnDecision> logs = GetDecisionLogs();
        int correctCount = 0;

        foreach (var log in logs)
        {
            if (log.usedMemory != null)
            {
                correctCount++;
            }
        }

        Debug.Log($"[Ending Evaluation] 正答数: {correctCount}");

        if (correctCount == 15)
        {
            SetEndingType("TRUE");
        }
        else if (correctCount >= 10)
        {
            SetEndingType("GOOD");
        }
        else
        {
            SetEndingType("BAD");
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("EndingScene");
    }


}
