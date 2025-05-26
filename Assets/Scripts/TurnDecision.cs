using UnityEngine;

/// <summary>
/// �v���C���[������^�[���ŒN�̋L����N�Ɏg���������L�^����f�[�^�iJSON�x�[�X�j
/// </summary>
[System.Serializable]
public class TurnDecision
{
    public int turn;
    public CharacterDataJson selectedMemoryOwner;
    public CharacterDataJson targetCharacter;

    public TurnDecision(int turn, CharacterDataJson owner, CharacterDataJson target)
    {
        this.turn = turn;
        this.selectedMemoryOwner = owner;
        this.targetCharacter = target;
    }
}
