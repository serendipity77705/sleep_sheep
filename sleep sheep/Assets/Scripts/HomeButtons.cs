using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtons : MonoBehaviour {
    public void Start()
    {
        Debug.Log("HomeButtons started");

    }

    public void Update()
    {
        // Debug.Log("HomeButtons updated");
    }

    public void GoToGame()
    {
        Debug.Log("Play Button clicked");
        SceneManager.LoadScene("CollectionScene");
    }

    public void GoToInstructions()
    {
        Debug.Log("Instructions Button clicked");
        SceneManager.LoadScene("InstructionsScene");
    }
}