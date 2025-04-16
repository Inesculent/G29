using UnityEngine;
using UnityEngine.AI;

public class EnemyCombatController : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private bool isAggro = false;
    private float attackTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true; // ✅ Let NavMeshAgent rotate the enemy
        agent.updatePosition = true; // ✅ Let it move the enemy
    }

    void Update()
    {
        if (!isAggro) return;

        attackTimer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // ✅ Let agent handle facing and pathing
            agent.SetDestination(transform.position); // Stop moving while attacking

            if (attackTimer >= attackCooldown)
            {
                Debug.Log("🗡 Enemy attacks player!");
                // TODO: Add attack logic here
                attackTimer = 0f;
            }
        }
    }

    public void TriggerAggro()
    {
        isAggro = true;

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
        agent.isStopped = false;

        Debug.Log("🔥 Enemy is now aggressive and chasing.");
    }
}
