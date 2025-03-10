using System.Collections;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public KeyCode punchKey = KeyCode.F; // Key to punch
    public KeyCode dodgeKey = KeyCode.Space; // Key to dodge
    public float punchDamage = 10f; // Damage dealt to the enemy
    public float dodgeWindow = 0.5f; // Time frame for dodging

    private bool isDodging = false; // Tracks whether the player is dodging
    private float dodgeTimer = 0f;

    public Transform punchPoint; // Empty GameObject positioned at the player's fist
    public float punchRadius = 0.8f; // Range of the punch
    public LayerMask enemyLayer; // Set to "Enemy" in Unity

    void Update()
    {
        if (Input.GetKeyDown(punchKey))
        {
            Punch();
        }

        if (Input.GetKeyDown(dodgeKey) && !isDodging)
        {
            StartDodge();
        }

        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;
            if (dodgeTimer >= dodgeWindow)
            {
                isDodging = false;
            }
        }
    }

    void Punch()
    {
        Debug.Log("🔴 Punch attack triggered!");

        // Delay hit detection slightly for animation sync
        StartCoroutine(DelayedPunchDamage(0.2f)); // Adjust timing based on animation
    }

    IEnumerator DelayedPunchDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRadius, enemyLayer);

        Debug.Log("🟠 Objects hit: " + hitEnemies.Length);

        foreach (Collider hit in hitEnemies)
        {
            Debug.Log("🟡 Hit object: " + hit.name);

            Health enemyHealth = hit.GetComponent<Health>() ?? hit.GetComponentInParent<Health>();

            if (enemyHealth != null)
            {
                Debug.Log("✅ Health component found on " + hit.name);
                enemyHealth.TakeDamage((int)punchDamage);
            }
            else
            {
                Debug.LogWarning("⚠️ No Health component found on " + hit.name);
            }
        }
    }

    void StartDodge()
    {
        isDodging = true;
        dodgeTimer = 0f;
        Debug.Log("Player is dodging!");
    }

    // Public method so EnemyAI can check if the player is dodging
    public bool IsDodging()
    {
        return isDodging;
    }
}
