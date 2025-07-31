using UnityEngine;
using UnityEngine.SceneManagement;


public class WinLose : MonoBehaviour
{

    private int currentHealth;
    private int objectCount;
    private HealthBar healthBar;
    private SheepMovement sheep;

    // Update is called once per frame

    void Start()
    {
        healthBar = GetComponent<HealthBar>();
        sheep = GetComponent<SheepMovement>();
    } 
    void Update()
    {
        currentHealth = healthBar.currentHealth;
        objectCount = sheep.goodItemCount;
        if (currentHealth <= 0)
        {
            LooseGame();
        }
        else if (objectCount == 7)
        {
            WinGame();
        }
    }


    public void WinGame()
    {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Additive);
    }
    
    public void LooseGame()
    {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Additive);
    }
}
