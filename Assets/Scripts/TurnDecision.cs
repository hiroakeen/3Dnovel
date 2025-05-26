using UnityEngine;

/// <summary>
/// プレイヤーがあるターンで誰の記憶を誰に使ったかを記録するデータ（JSONベース）
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
