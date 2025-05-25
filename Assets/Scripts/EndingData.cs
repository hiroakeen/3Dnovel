using UnityEngine;

[CreateAssetMenu(fileName = "EndingData", menuName = "Story/Ending Data")]
public class EndingData : ScriptableObject
{
    public string endingId; // "TRUE_END", "BAD_1", etc.
    public string title;
    [TextArea] public string description;
    public Sprite backgroundImage;
    public AudioClip endingBGM;
}
