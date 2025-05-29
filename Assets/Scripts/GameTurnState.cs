using System.Collections.Generic;

/// <summary>
/// �Q�[���̊e�^�[���t�F�[�Y��\���񋓑́B
/// </summary>
public enum GameTurnState
{
    TalkPhase,     // ��b�t�F�[�Y�i�L�����W�j
    MemoryPhase,   // �L���g�p�t�F�[�Y�i5�l�ɓn���j
    EndingPhase    // �G���f�B���O����E���o
}

/// <summary>
/// �e�^�[����Ԃ̋��ʃC���^�[�t�F�[�X�B
/// ��Ԃ��Ƃɏ������e�𕪗����邽�߂Ɏg�p�B
/// </summary>
public interface ITurnState
{
    /// <summary>��Ԃɓ������Ƃ��ɌĂ΂��</summary>
    void OnStateEnter();

    /// <summary>��Ԃ𗣂��Ƃ��ɌĂ΂��</summary>
    void OnStateExit();

    /// <summary>��b�����L�����̒ʒm�iTalkPhase�p�j</summary>
    void NotifyCharacterTalked(CharacterDataJson character);

    /// <summary>�L����n�����ʒm�iMemoryPhase�p�j</summary>
    void NotifyMemoryUsed(CharacterDataJson from, CharacterDataJson to, MemoryData memory);


    /// <summary>��b�I���ʒm�i�^�[�����őS���Ƙb���������Ǘ��j</summary>
    void NotifyTalkFinished(CharacterDataJson character);
}
