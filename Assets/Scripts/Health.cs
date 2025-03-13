using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; // Assign this in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value=maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log(gameObject.name + " took " + damage + " damage! Current HP: " + currentHealth);
        healthBar.value = currentHealth;

        // Trigger red flash effect when taking damage
        UIManager.Instance?.ShowDamageFlash();

        // Check if entity is dead
        if (currentHealth == 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.value = maxHealth;
        Debug.Log(gameObject.name + " health reset to " + maxHealth);
    }

    void HandleDeath()
    {
        if (CompareTag("Player")) // If the player dies, trigger the time loop
        {
            Debug.Log("Player has died! Initiating time loop...");
            TimeLoopManager.Instance?.TriggerTimeLoop();
        }
        else if (CompareTag("Enemy")) // If an enemy dies, remove them from the game
        {
            Debug.Log(gameObject.name + " has died! Marking as defeated.");
            StateManager.Instance?.RegisterDefeatedEnemy(gameObject.name);
            gameObject.SetActive(false); // Disable enemy instead of destroying it
        }
    }
}
