using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public Button playAgainButton;
    public Button mainMenuButton;

    void Start()
    {
        // Make sure time scale is normal (in case game was paused)
        Time.timeScale = 1f;
        
        // Add button listeners
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }
    public void PlayAgain()
    {
        Debug.Log("Play Again button clicked");
        MultiItemSpawner.ResetCollectedItems();
        SceneManager.LoadScene("CollectionScene");
    }
    
    public void GoToMainMenu()
    {
        Debug.Log("Main Menu button clicked");
        MultiItemSpawner.ResetCollectedItems();
        SceneManager.LoadScene("IntroScene");
    }
}