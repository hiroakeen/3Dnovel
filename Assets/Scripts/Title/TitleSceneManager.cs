using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    [Header("フェード用")]
    [SerializeField] private CanvasGroup blackFadeCanvasGroup; // 黒で目覚め演出
    [SerializeField] private CanvasGroup whiteFadeCanvasGroup; // 白で遷移演出

    [Header("UI")]
    [SerializeField] private Button startButton;

    [Header("設定")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private string mainSceneName = "MainScene";

    private void Start()
    {
        // 最初は黒 → フェードイン
        if (blackFadeCanvasGroup != null)
        {
            blackFadeCanvasGroup.alpha = 1;
            blackFadeCanvasGroup.DOFade(0, fadeDuration);
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButtonPressed);
        }
    }

    private void OnStartButtonPressed()
    {
        // 白くフェードアウトしてからシーン遷移
        if (whiteFadeCanvasGroup != null)
        {
            whiteFadeCanvasGroup.alpha = 0;
            whiteFadeCanvasGroup.gameObject.SetActive(true);

            whiteFadeCanvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
            {
                SceneManager.LoadScene(mainSceneName);
            });
        }
        else
        {
            // 念のため fallback
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
