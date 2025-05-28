using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Narration/NarrationData")]
public class NarrationData : ScriptableObject
{
    public List<NarrationLine> lines;
}

