using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �G���f�B���O�������ꌳ�Ǘ�����}�l�[�W���[
/// �G���f�B���OID�ɂ���ēK�؂ȃV�[���ɑJ�ڂ��A�ォ��Q�Ƃł���悤�ۑ����s��
/// </summary>
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    /// <summary>
    /// �J�ڍς݂̃G���f�B���OID�i���UI�ɕ\���������ꍇ�Ȃǂɗ��p�\�j
    /// </summary>
    public static string LastEndingId { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// �w�肳�ꂽ�G���f�B���OID�ɉ����ăG���f�B���O�V�[����ǂݍ���
    /// </summary>
    public void LoadEndingScene(string endingId)
    {
        Debug.Log($"[EndingManager] �G���f�B���O�J��: {endingId}");
        LastEndingId = endingId;

        string sceneName = GetSceneNameFromEndingId(endingId);
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"[EndingManager] ���m�̃G���f�B���OID: {endingId}");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// �G���f�B���OID����J�ڂ���ׂ��V�[������Ԃ�
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
