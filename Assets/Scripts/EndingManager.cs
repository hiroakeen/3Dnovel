using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    public static string LastEndingId { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

    private string GetSceneNameFromEndingId(string id)
    {
        return id switch
        {
            "TRUE_END" => "EndingScene_TRUE",
            "GOOD_END" => "EndingScene_GOOD",
            "BAD_END" => "EndingScene_BAD",
            _ => null
        };
    }
}
