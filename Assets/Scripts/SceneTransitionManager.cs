using UnityEngine.SceneManagement;
using UnityEngine;

public static class EndingResultHolder
{
    public static EndingData currentEnding;
}

public class SceneTransitionManager : MonoBehaviour
{
    public void ShowEnding(EndingData ending)
    {
        EndingResultHolder.currentEnding = ending;
        SceneManager.LoadScene("EndingScene");
    }
}
