using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��b���ɃL�����C���X�g�Ɩ��O��\������UI�R���|�[�l���g�i�u����̂������H�v�p�j
/// �� �e�L�X�g�\���� UIManager �����S��
/// </summary>
public class MemoryCharacterDisplay : MonoBehaviour
{
    [Header("�L�����C���X�g")]
    [SerializeField] private Image characterImage;

    [Header("���O���x��")]
    [SerializeField] private TMP_Text nameLabel;

    private void Start()
    {
        Hide(); // ������Ԃł͔�\��
    }

    /// <summary>
    /// �L�����C���X�g�Ɩ��O��\���i�q�I�u�W�F�N�g���܂߂ėL�����j
    /// </summary>
    public void Show(Sprite sprite, string name)
    {
        if (characterImage != null)
        {
            characterImage.sprite = sprite;
            characterImage.enabled = true;

            // �q�I�u�W�F�N�g���L�����i�p�l���⑕���j
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
    /// ��\���ɂ���i�q�p�l�����܂߂āj
    /// </summary>
    public void Hide()
    {
        if (characterImage != null)
        {
            characterImage.enabled = false;

            // �q�I�u�W�F�N�g��������
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
