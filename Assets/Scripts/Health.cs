using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; // Assign in Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log(gameObject.name + " took " + damage + " damage! Current HP: " + currentHealth);
        UpdateHealthBar();

        // Check if the entity is dead
        if (currentHealth == 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    void HandleDeath()
    {
        if (gameObject.CompareTag("Player")) // If it's the player
        {
            Debug.Log("Player has died! Initiating time loop...");
            TimeLoopManager.Instance?.TriggerTimeLoop(transform.position);
        }
        else
        {
            Debug.Log(gameObject.name + " has died!");
            gameObject.SetActive(false); // Deactivate enemy
        }
    }
}
