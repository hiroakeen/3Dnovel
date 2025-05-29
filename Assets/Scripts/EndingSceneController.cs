using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI�Q��")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private AudioSource audioSource;

    [Header("�f�[�^�p�X")]
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

        GameManager.Instance.SetEndingType(endingId); // �����K���ǉ��I
        SceneManager.LoadScene("EndingScene"); // ���ꂳ�ꂽ1�̃G���f�B���O�V�[���֑J��
    }

}

