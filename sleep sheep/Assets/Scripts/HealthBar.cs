using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthDisplay();
        Debug.Log($"Health decreased by {amount}. Current health: {currentHealth}");
    }

    public void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
    }
}