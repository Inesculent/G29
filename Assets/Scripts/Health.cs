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

    // Trigger red flash effect
    UIManager.Instance?.ShowDamageFlash();

    if (currentHealth == 0)
    {
        HandleDeath();
    }
}
public void ResetHealth()
{
    currentHealth = maxHealth;
    UpdateHealthBar();
    Debug.Log(gameObject.name + " health reset to " + maxHealth);
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
    if (CompareTag("Player"))
    {
        Debug.Log("Player has died! Initiating time loop...");
        TimeLoopManager.Instance?.TriggerTimeLoop(transform.position);
    }
    else if (CompareTag("Enemy"))
    {
        Debug.Log(gameObject.name + " has died! Marking as defeated.");
        StateManager.Instance?.RegisterDefeatedEnemy(gameObject.name);
        gameObject.SetActive(false);
    }
}

}
