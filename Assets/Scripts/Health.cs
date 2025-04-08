using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar; // Assign this in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.value = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log(gameObject.name + " took " + damage + " damage! Current HP: " + currentHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        UIManager.Instance?.ShowDamageFlash();

        if (currentHealth == 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        StartCoroutine(DelayedHealthBarReset());
        Debug.Log(gameObject.name + " health reset to " + maxHealth);
    }

    private IEnumerator DelayedHealthBarReset()
    {
        yield return null; // Wait 1 frame to ensure GameObject and Canvas are fully active

        if (healthBar != null)
        {
            healthBar.value = 0f; // Force UI update (helps world-space sliders)
            Canvas.ForceUpdateCanvases();
            healthBar.value = currentHealth;
            Canvas.ForceUpdateCanvases(); // Ensure redraw

            Debug.Log($"{gameObject.name} health bar visibly updated to {currentHealth}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no healthBar assigned!");
        }
    }

    void HandleDeath()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("Player has died! Initiating time loop...");
            TimeLoopManager.Instance?.TriggerTimeLoop();
        }
        else if (CompareTag("Enemy"))
        {
            Debug.Log(gameObject.name + " has died! Marking as defeated.");
            StateManager.Instance?.RegisterDefeatedEnemy(gameObject.name);
            gameObject.SetActive(false); // Disable enemy instead of destroying
        }
    }
}
