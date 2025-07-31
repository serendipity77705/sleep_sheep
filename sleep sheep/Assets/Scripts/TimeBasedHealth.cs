using UnityEngine;
using UnityEngine.UI;

public class TimeBasedHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float healthDecayRate = 1.67f; // Health lost per second
    [SerializeField] private float decayDelay = 1f; // Wait time before decay starts
    
    [Header("UI References")]
    public Scrollbar healthBarScrollbar; // For visual health bar
    
    [Header("Game Over")]
    public bool pauseGameOnDeath = true;
    
    private float currentHealth;
    private float decayTimer;
    private bool isDead = false;
    
    void Start()
    {
        currentHealth = maxHealth;
        decayTimer = decayDelay;
        UpdateHealthDisplay();
    }
    
    void Update()
    {
        if (isDead) return;
        
        // Countdown decay timer
        decayTimer -= Time.deltaTime;
        
        // Start decaying health after delay
        if (decayTimer <= 0f)
        {
            DecayHealth();
        }
        
        UpdateHealthDisplay();
        
        // Check for death
        if (currentHealth <= 0f && !isDead)
        {
            HandleDeath();
        }
        
        // Test controls (remove these later)
        TestControls();
    }
    
    void DecayHealth()
    {
        currentHealth -= healthDecayRate * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
    
    void UpdateHealthDisplay()
    {
        // Update scrollbar (visual health bar)
        if (healthBarScrollbar != null)
        {
            healthBarScrollbar.size = currentHealth / maxHealth;
        }
    }
    
    void HandleDeath()
    {
        isDead = true;
        Debug.Log("Sheep is dead");
        
        if (pauseGameOnDeath)
        {
            Time.timeScale = 0f; // Pause game
        }
        
        // Add death effects here (restart level, show game over screen, etc.)
    }
    
    // Call this to add health (from pickups, etc.)
    public void AddHealth(float amount)
    {
        if (isDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        // Reset decay timer when gaining health
        decayTimer = decayDelay;
    }
    
    // Call this to remove health instantly (from damage)
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        // Reset decay timer when taking damage
        decayTimer = decayDelay;
    }
    
    // Stop health decay (useful for safe zones, pausing, etc.)
    public void StopDecay()
    {
        decayTimer = float.MaxValue;
    }
    
    // Resume health decay
    public void ResumeDecay()
    {
        decayTimer = decayDelay;
    }
    
    // Get current health percentage (0-1)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    // Check if player is alive
    public bool IsAlive()
    {
        return !isDead;
    }
    
    // Reset health to full (for respawning)
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        decayTimer = decayDelay;
        Time.timeScale = 1f; // Resume game if paused
    }
    
    // Test controls - remove these in final game
    void TestControls()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddHealth(20f); // H = Heal
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(15f); // J = Damage
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetHealth(); // R = Reset
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (decayTimer == float.MaxValue)
                ResumeDecay(); // Resume decay
            else
                StopDecay(); // Pause decay
        }
    }
}