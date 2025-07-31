using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Scene Names")]
    public string gameSceneName = "CollectionScene";
    public string mainMenuSceneName = "IntroScene";

    void Start()
    {
        // Make sure time scale is normal (in case game was paused)
        Time.timeScale = 1f;
        
        // Add button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void RestartGame()
    {
        // Reset time scale to normal
        Time.timeScale = 1f;
        
        // Load the main game scene
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToMainMenu()
    {
        // Reset time scale to normal
        Time.timeScale = 1f;
        
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }
}