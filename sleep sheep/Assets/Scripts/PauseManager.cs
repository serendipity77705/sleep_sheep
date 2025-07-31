using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;  // Resume normal speed
        
        // Hide pause menu if it exists
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
            
        // Unload pause scene if it was loaded additively
        if (SceneManager.GetSceneByName("PauseScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("PauseScene");
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;  // Completely pause the game
        
        // Show pause menu if it exists
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
            
        // Load pause scene additively if not already loaded
        if (!SceneManager.GetSceneByName("PauseScene").isLoaded)
        {
            SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
        }
    }
}