using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthBar;
    public bool IsDead => currentHealth <= 0;

    [SerializeField] private EnemyIdentity identity;

    void Start()
    {
        currentHealth = maxHealth;

        if (CompareTag("Enemy") && identity == null)
        {
            identity = GetComponent<EnemyIdentity>();
        }

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
        yield return null;

        if (healthBar != null)
        {
            healthBar.value = 0f;
            Canvas.ForceUpdateCanvases();
            healthBar.value = currentHealth;
            Canvas.ForceUpdateCanvases();
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
            if (identity != null)
            {
                Debug.Log($"[{gameObject.name}] with ID {identity.UniqueID} has died. Registering as defeated.");
                StateManager.Instance?.RegisterDefeatedEnemy(identity.UniqueID);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} has no EnemyIdentity script!");
            }

            gameObject.SetActive(false);
        }
    }
}
