using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPausedScene;
    public GameObject pauseMenuUI;

    void Start()
    {
        // Detect whether we're in the PauseScene
        isPausedScene = SceneManager.GetActiveScene().name == "PauseScene";
        
        if (!isPausedScene)
            Time.timeScale = 1f;  // Normal speed
        else
            Time.timeScale = 0f;  // Game is paused
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPausedScene)
            {
                // Resume game: go back to gameplay scene
                ResumeGame();
            }
            else
            {
                // Pause game: go to pause scene
                PauseGame();
            }
        }
    }
    
    public void ResumeGame()
    {
        // SceneManager.LoadScene("CollectionScene");
        // SceneManager.UnloadSceneAsync("PauseScene");
        SceneManager.UnloadSceneAsync("PauseScene");
    }

    public void PauseGame()
    {
        SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
    }
}
