using UnityEngine;

public class AutoStart : MonoBehaviour
{
    void Start()
    {
        var manager = FindObjectOfType<GameStateManager>();
        manager.ChangeState(new StartState(manager));
    }
}
