using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会話中にキャライラストと名前を表示するUIコンポーネント（「だれのきおく？」用）
/// ※ テキスト表示は UIManager 側が担当
/// </summary>
public class MemoryCharacterDisplay : MonoBehaviour
{
    [Header("キャライラスト")]
    [SerializeField] private Image characterImage;

    [Header("名前ラベル")]
    [SerializeField] private TMP_Text nameLabel;

    private void Start()
    {
        Hide(); // 初期状態では非表示
    }

    /// <summary>
    /// キャライラストと名前を表示（子オブジェクトも含めて有効化）
    /// </summary>
    public void Show(Sprite sprite, string name)
    {
        if (characterImage != null)
        {
            characterImage.sprite = sprite;
            characterImage.enabled = true;

            // 子オブジェクトも有効化（パネルや装飾）
            foreach (Transform child in characterImage.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (nameLabel != null)
        {
            nameLabel.text = name;
            nameLabel.enabled = true;
        }
    }

    /// <summary>
    /// 非表示にする（子パネルも含めて）
    /// </summary>
    public void Hide()
    {
        if (characterImage != null)
        {
            characterImage.enabled = false;

            // 子オブジェクトも無効化
            foreach (Transform child in characterImage.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (nameLabel != null)
        {
            nameLabel.enabled = false;
        }
    }
}
