using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    [Header("�t�F�[�h�p")]
    [SerializeField] private CanvasGroup blackFadeCanvasGroup; // ���Ŗڊo�߉��o
    [SerializeField] private CanvasGroup whiteFadeCanvasGroup; // ���őJ�ډ��o

    [Header("UI")]
    [SerializeField] private Button startButton;

    [Header("�ݒ�")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private string mainSceneName = "MainScene";

    private void Start()
    {
        // �ŏ��͍� �� �t�F�[�h�C��
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
        // �����t�F�[�h�A�E�g���Ă���V�[���J��
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
            // �O�̂��� fallback
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
