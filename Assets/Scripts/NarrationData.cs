using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrationData", menuName = "Story/Narration Data")]
public class NarrationData : ScriptableObject
{
    public List<NarrationLine> lines = new();
}
