using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float attackCooldown = 2f;
    public int attackDamage = 15;
    private float attackTimer = 0f;

    public float attackRange = 2f; 
    public Transform attackPoint;
    public LayerMask playerLayer;

    [SerializeField] private Health playerHealth; // Player Health script reference
    [SerializeField] private CombatController playerCombat; // Player Combat script reference

    void Start()
    {
        // Auto-assign player references if they are not set in the Inspector
        if (playerHealth == null)
            playerHealth = GameObject.FindWithTag("Player")?.GetComponent<Health>();

        if (playerCombat == null)
            playerCombat = GameObject.FindWithTag("Player")?.GetComponent<CombatController>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            AttemptAttack();
            attackTimer = 0f;
        }
    }

    void AttemptAttack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in hitPlayers)
        {
            if (playerCombat != null && playerCombat.IsDodging())
            {
                Debug.Log("Player dodged the attack!");
                return;
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy attacked player!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
