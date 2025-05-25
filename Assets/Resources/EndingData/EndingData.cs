using UnityEngine;

[CreateAssetMenu(fileName = "EndingData", menuName = "Story/Ending Data")]
public class EndingData : ScriptableObject
{
    public string endingId; // —á: "TRUE_END", "BAD_END"
    public string title;
    [TextArea]
    public string description;
    public Sprite backgroundImage;
    public AudioClip endingBGM;
}
