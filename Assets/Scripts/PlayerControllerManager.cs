using UnityEngine;

public class PlayerControllerManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour movementScript;

    private bool isPaused = false;

    public void PauseControl()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
            isPaused = true;
        }
    }

    public void ResumeControl()
    {
        if (movementScript != null)
        {
            movementScript.enabled = true;
            isPaused = false;
        }
    }

    public bool IsPaused() => isPaused;
}
