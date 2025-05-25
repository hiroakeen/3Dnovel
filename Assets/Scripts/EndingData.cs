using UnityEngine;

[CreateAssetMenu(menuName = "Ending/EndingData")]
public class EndingData : ScriptableObject
{
    public string endingId;
    public string title;
    [TextArea]
    public string description;
    public Sprite backgroundImage;
    public AudioClip endingBGM;
}
