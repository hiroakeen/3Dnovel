using UnityEngine;

/// <summary>
/// プレイヤーがあるターンで誰の記憶を誰に使ったかを記録するデータ
/// </summary>
[System.Serializable]
public class TurnDecision
{
    public int turn;
    public CharacterMemoryData selectedMemoryOwner;
    public CharacterMemoryData targetCharacter;

    public TurnDecision(int turn, CharacterMemoryData owner, CharacterMemoryData target)
    {
        this.turn = turn;
        this.selectedMemoryOwner = owner;
        this.targetCharacter = target;
    }
}
