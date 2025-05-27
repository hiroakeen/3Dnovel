using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadEndingController : MonoBehaviour
{
    [Header("背景画像候補（3枚）")]
    [SerializeField] private Sprite[] backgroundSprites;

    [Header("参照UI")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private AudioSource bgmSource;

    [Header("BGMランダム候補（任意）")]
    [SerializeField] private AudioClip[] bgmClips;

    private void Start()
    {
        // ✅ 背景をランダムに選択
        if (backgroundSprites.Length > 0 && backgroundImage != null)
        {
            int index = Random.Range(0, backgroundSprites.Length);
            backgroundImage.sprite = backgroundSprites[index];
        }

        // ✅ ナレーション（固定でもランダムでもOK）
        string[] messages =
        {
            "……誰も救えなかった。",
            "記憶は闇に消えた。",
            "そして誰も真実を知らないまま、終わりを迎えた。"
        };
        titleText.text = "BAD END";
        descriptionText.text = messages[Random.Range(0, messages.Length)];

        // ✅ BGMランダム再生（任意）
        if (bgmClips.Length > 0 && bgmSource != null)
        {
            int bgmIndex = Random.Range(0, bgmClips.Length);
            bgmSource.clip = bgmClips[bgmIndex];
            bgmSource.Play();
        }
    }
}
