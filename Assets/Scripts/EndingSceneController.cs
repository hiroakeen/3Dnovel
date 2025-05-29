using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource audioSource;

    [Header("データパス")]
    [SerializeField] private string resourceFolderPath = "EndingPattern";


    private void Start()
    {
        LoadAutoEnding();
    }

    public void LoadAutoEnding()
    {
        string resultType = MemoryManager.Instance.GetEndingResultType();
        string endingId = resultType switch
        {
            "TrueEnding" => "TRUE_END",
            "GoodEnding" => "GOOD_END",
            _ => "BAD_END"
        };

        GameManager.Instance.SetEndingType(endingId); // これを必ず追加！
        SceneManager.LoadScene("EndingScene"); // 統一された1つのエンディングシーンへ遷移
    }

}

