using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// エンディング処理を一元管理するマネージャー
/// エンディングIDによって適切なシーンに遷移し、後から参照できるよう保存も行う
/// </summary>
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    /// <summary>
    /// 遷移済みのエンディングID（後でUIに表示したい場合などに利用可能）
    /// </summary>
    public static string LastEndingId { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 指定されたエンディングIDに応じてエンディングシーンを読み込む
    /// </summary>
    public void LoadEndingScene(string endingId)
    {
        Debug.Log($"[EndingManager] エンディング遷移: {endingId}");
        LastEndingId = endingId;

        string sceneName = GetSceneNameFromEndingId(endingId);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[EndingManager] 未知のエンディングID: {endingId}");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// エンディングIDから遷移するべきシーン名を返す
    /// </summary>
    private string GetSceneNameFromEndingId(string id)
    {
        return id switch
        {
            "TRUE" => "TrueEndingScene",
            "GOOD" => "NormalEndingScene",
            "FALSE" => "FalseEndingScene",
            "NEUTRAL" => "NeutralEndingScene",
            "BAD" => "BadEndingScene",
            _ => null
        };
    }


}
