using UnityEngine;
using UnityEngine.AI;

public class PatrolAfterConditions : MonoBehaviour
{
    public Health enemy2Health = null;
    public Transform player;
    public float patrolRadius = 5f;
    public float patrolSpeed = 3.5f;
    public float doorDetectionRadius = 2f;

    [Header("Aggro Settings")]
    public float aggroRange = 5f;      // When enemy notices player
    public float attackRange = 2f;     // When enemy stops to attack
    public float deaggroRange = 8f;    // When enemy gives up chasing

    private NavMeshAgent agent;
    private Vector3 startingPosition;
    private bool canPatrol = false;
    private bool isAggro = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        startingPosition = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void OnEnable()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.loopResetEvent += OnTimeLoop;
        }
    }

    void OnDisable()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.loopResetEvent -= OnTimeLoop;
        }
    }

    void OnTimeLoop()
    {
        Debug.Log("🔄 Time loop detected. Resetting patrol.");
        transform.position = startingPosition;
        canPatrol = false;
        isAggro = false;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        if (enemy2Health == null || enemy2Health.IsDead)
        {
            canPatrol = true;
            PatrolAroundPlayer();
        }
    }

    void Update()
    {
        if (!canPatrol && enemy2Health != null && enemy2Health.IsDead)
        {
            canPatrol = true;
            Debug.Log("✅ Enemy 3 starts patrolling (Enemy 2 is dead).");
        }

        if (canPatrol)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (!isAggro && distToPlayer <= aggroRange)
            {
                isAggro = true;
                Debug.Log("🔥 Enemy entered AGGRO mode!");
            }

            if (isAggro)
            {
                HandleAggro(distToPlayer);
            }
            else
            {
                PatrolAroundPlayer();
            }

            if (!agent.pathPending && agent.remainingDistance >= agent.stoppingDistance && agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                TryOpenNearbyDoor();
            }
        }
    }

    void HandleAggro(float distToPlayer)
    {
        if (distToPlayer > deaggroRange)
        {
            isAggro = false;
            agent.ResetPath();
            Debug.Log("😴 Enemy lost player. Returning to patrol.");
            return;
        }

        if (distToPlayer > attackRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
            Debug.Log("🗡 Enemy in attack range. (Add attack logic here)");
        }
    }

    void PatrolAroundPlayer()
    {
        if (!agent.pathPending && (agent.remainingDistance < 0.5f || agent.pathStatus != NavMeshPathStatus.PathComplete))
        {
            for (int i = 0; i < 3; i++) // Try 3 times for a valid point
            {
                Vector3 offset = Random.insideUnitCircle * patrolRadius;
                Vector3 rawTarget = player.position + new Vector3(offset.x, 0, offset.y);

                if (NavMesh.SamplePosition(rawTarget, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);

                    if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                    {
                        Debug.Log($"🚶 Patrol target set: {hit.position}");
                        return;
                    }
                }
            }

            Debug.Log("❌ Could not find a valid or reachable patrol point after 3 tries.");
        }
    }

    void TryOpenNearbyDoor()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, doorDetectionRadius);

        foreach (Collider col in nearby)
        {
            Door door = col.GetComponent<Door>();
            if (door != null && !door.IsOpen && !door.doorOpened)
            {
                Debug.Log($"🚪 {gameObject.name} is opening door: {door.name}");
                door.Open(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, patrolRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, doorDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, deaggroRange);
    }
}
