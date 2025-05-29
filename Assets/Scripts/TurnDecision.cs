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
    public MemoryData usedMemory; 

    public TurnDecision(int turn, CharacterDataJson owner, CharacterDataJson target, MemoryData memory)
    {
        this.turn = turn;
        this.selectedMemoryOwner = owner;
        this.targetCharacter = target;
        this.usedMemory = memory;
    }

    //オーバーロード
    public TurnDecision(int turn, CharacterDataJson owner, CharacterDataJson target)
    {
        this.turn = turn;
        this.selectedMemoryOwner = owner;
        this.targetCharacter = target;
        this.usedMemory = null;
    }


}
