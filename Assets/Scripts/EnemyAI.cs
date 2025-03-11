using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float attackCooldown = 2f;
    public int attackDamage = 15;
    private float attackTimer = 0f;

    public float attackRange = 2f; 
    public Transform attackPoint;
    public LayerMask playerLayer;

    [SerializeField] private Animator animator;
    [SerializeField] private Health playerHealth; // Player Health script reference
    [SerializeField] private CombatController playerCombat; // Player Combat script reference

    void Start()
    {
        // Auto-assign player references if they are not set in the Inspector
        if (playerHealth == null)
            playerHealth = GameObject.FindWithTag("Player")?.GetComponent<Health>();

        if (playerCombat == null)
            playerCombat = GameObject.FindWithTag("Player")?.GetComponent<CombatController>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            AttemptAttack();
            attackTimer = 0f;
        }

        //Use for testing
        //Uses R to rest player's health to max
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    playerHealth.ResetHealth();
        //}
    }

    void AttemptAttack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        foreach (Collider player in hitPlayers)
        {

            if (playerCombat != null && playerCombat.IsDodging())
            {
                animator.SetTrigger("Punch");
                Debug.Log("Player dodged the attack!");
                return;
            }

            if (playerHealth != null)
            {
                animator.SetTrigger("Punch");
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
