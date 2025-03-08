using UnityEngine;

public class CombatController : MonoBehaviour
{
    public KeyCode punchKey = KeyCode.F; // Key to punch
    public KeyCode dodgeKey = KeyCode.Space; // Key to dodge
    public float punchDamage = 10f; // Damage dealt to the enemy
    public float dodgeWindow = 0.5f; // Time frame for dodging

    private bool isDodging = false;
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
    Debug.Log("Punch() called!");

    Collider[] hitEnemies = Physics.OverlapSphere(punchPoint.position, punchRadius, enemyLayer);

    Debug.Log("Enemies hit: " + hitEnemies.Length);

    foreach (Collider enemy in hitEnemies)
    {
        Debug.Log("Hit object: " + enemy.name); // Shows what objects are hit

        Health enemyHealth = enemy.GetComponent<Health>();

        if (enemyHealth != null)
        {
            Debug.Log("Health component found on " + enemy.name);
            enemyHealth.TakeDamage((int)punchDamage);
            Debug.Log("Punch landed on " + enemy.name);
        }
        else
        {
            Debug.LogWarning("⚠️ No Health component found on " + enemy.name);
        }
    }
}


    void StartDodge()
    {
        isDodging = true;
        dodgeTimer = 0f;
        Debug.Log("Player is dodging!");
    }

    public bool IsDodging()
    {
        return isDodging;
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}
