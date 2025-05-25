using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public void ShowEnding(EndingData ending)
    {
        EndingResultHolder.currentEnding = ending;
        SceneManager.LoadScene("EndingScene");
    }
}
