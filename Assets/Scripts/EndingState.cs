using UnityEngine;

public class EndingState : IGameState
{
    private GameStateManager manager;
    private string endingId;

    public EndingState(GameStateManager manager, string endingId)
    {
        this.manager = manager;
        this.endingId = endingId;
    }

    public void Enter()
    {
        Debug.Log($"EndingState: �G���f�B���O {endingId} ��\����");

        // ���f�[�^�ŕ\���i�{�Ԃł� ScriptableObject ����Q�Ɓj
        string title = "";
        string description = "";

        switch (endingId)
        {
            case "TRUE_END":
                title = "�^���̒E�o";
                description = "���Ȃ��͑S�ẲR���������A�F�Ƌ��ɕ������o���B";
                break;
            case "GOOD_END":
                title = "�ꕔ�E�o";
                description = "���Ȃ��͏����������A���̒N�����]���ɂȂ����B";
                break;
            case "BAD_END":
            default:
                title = "�����ꂽ�I��";
                description = "�����m��Ȃ��܂܁A���Ȃ������͕����߂�ꂽ�B";
                break;
        }

        UIManager.Instance.ShowEnding(title, description);
    }

    public void Exit()
    {
        Debug.Log("EndingState �I��");
    }
}