using UnityEngine;

[System.Serializable]
public class NarrationLine
{
    [TextArea] public string text;
    public float delayBeforeNext = 2f; // ���̍s�܂ł̑҂����ԁi�C�Ӂj
}
