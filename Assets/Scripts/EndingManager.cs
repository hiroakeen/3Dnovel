using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LoadEndingScene(string endingId)
    {
        string sceneName = endingId switch
        {
            "TRUE_END" => "EndingScene_TRUE",
            "GOOD_END" => "EndingScene_GOOD",
            "BAD_END" => "EndingScene_BAD",
            _ => "EndingScene_BAD"
        };

        Debug.Log($"[EndingManager] エンディングシーンへ移動: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
