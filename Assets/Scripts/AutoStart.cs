using UnityEngine;

public class AutoStart : MonoBehaviour
{
    void Start()
    {
        var manager = FindAnyObjectByType<GameStateManager>();
        manager.ChangeState(new StartState(manager));
    }
}
