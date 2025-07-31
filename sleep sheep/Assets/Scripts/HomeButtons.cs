using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtons : MonoBehaviour {
    public void GoToGame()
    {
        SceneManager.LoadScene("CollectionScene");
    }

    public void GoToInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }
}