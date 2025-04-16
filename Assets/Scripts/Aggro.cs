using UnityEngine;
using UnityEngine.AI;

public class Aggro : MonoBehaviour
{
    [Header("Player & Range Settings")]
    public Transform player;          // Reference to the player's transform
    public float detectionRange = 4f;  // Range at which the enemy detects and becomes aggro on the player
    public float deaggroRange = 15f;    // Range beyond which the enemy stops chasing the player
    public float attackRange = 2f;      // Range at which the enemy stops chasing and attacks

    [Header("Movement Settings")]
    public float chaseSpeed = 6f;     // Speed when chasing the player

    private NavMeshAgent agent;
    private bool isAggro = false;       // Tracks whether the enemy is currently aggro

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;

        // Find the player if not assigned.
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If not aggro yet, check for detection range
        if (!isAggro)
        {
            if (distanceToPlayer <= detectionRange)
            {
                isAggro = true;
                Debug.Log("Player detected. Enemy is now aggro!");
            }
            else
            {
                // Optionally, add idle behavior here.
                agent.ResetPath();
                return;
            }
        }
        else // Already aggro.
        {
            // If player moves out of deaggro range, lose aggro.
            if (distanceToPlayer > deaggroRange)
            {
                isAggro = false;
                agent.ResetPath();
                Debug.Log("Player lost. Enemy deaggros.");
                return;
            }
        }

        // If aggro, chase or attack.
        if (isAggro)
        {
            if (distanceToPlayer > attackRange)
            {
                // Chase the player.
                agent.SetDestination(player.position);
                Debug.Log("Chasing player. Destination set to: " + player.position);
            }
            else
            {
                // Stop moving and attack.
                agent.ResetPath();
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        // Insert your attack logic here.
        Debug.Log("Attacking player!");
    }

    // New method: Force the enemy to become aggressive.
    public void ForceAggro()
    {
        if (!isAggro)
        {
            isAggro = true;
            Debug.Log("ForceAggro called: Enemy is now aggro.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection (red), deaggro (magenta), and attack (green) ranges.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, deaggroRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
