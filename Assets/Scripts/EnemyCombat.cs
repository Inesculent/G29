using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCombat : MonoBehaviour
{
    Animator animator;

    public GameObject enemy;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;

    public HealthBarScript healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    public int switchHand = 1;
    public bool playerInTrigger = false;
    public bool isDead = false;

    public float sightRange;
    public float attackRange;
    private bool alreadyAttacked;
    public float timeBetweenAttacks;
    private bool tookDamage = false;

    public LayerMask groundLayer, PlayerLayer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        alreadyAttacked = false;

        //Disable colliders so that they dont trigger damage on the player
        //when not actively attacking
        leftHand.GetComponent<Collider>().enabled = false;
        rightHand.GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);

        //Go into combat mode and attempt to attack player
        if (playerInAttackRange && playerInSightRange)
        {
            animator.SetBool("playerInRange", true);
            AttackPlayer();
        }

        //Goes into combat mode once the player is in sight
        if (playerInSightRange)
        {
            animator.SetBool("playerInRange", true);
        }

        //Back to idle
        else
        {
            animator.SetBool("playerInRange", false);
        }
    }

    private void FixedUpdate()
    {
        //If the player is hit deccrease their health
        if (playerInTrigger == true)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(10);
        }
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            transform.LookAt(player.transform.position);
            alreadyAttacked = true;

            //Switches which hand the enemy is punching with
            switch (switchHand)
            {
                //Plays the right punch animation
                case 1:
                    rightHand.GetComponent<Collider>().enabled = true;
                    animator.SetTrigger("rightAttack");
                    switchHand = 2;
                    break;

                //Plays the left punch animation
                case 2:
                    leftHand.GetComponent<Collider>().enabled = true;
                    animator.SetTrigger("leftAttack");
                    switchHand = 1;
                    break;
            }

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        //Disable colliders again
        rightHand.GetComponent<Collider>().enabled = false;
        leftHand.GetComponent<Collider>().enabled = false;
        alreadyAttacked = false;
    }

    //Plays the hit animation and decreases health
    public void TakeDamage(int damage)
    {
        if (!tookDamage)
        {
            currentHealth -= damage;

            //Plays death animation once at 0 health
            if (currentHealth <= 0)
            {
                animator.SetBool("isDead", true);
            }

            else
            {
                //Gets which hand the player punched with then plays the 
                //repective damage animation
                switch (player.GetComponent<PlayerCombat>().switchHand)
                {
                    case 1:
                        animator.SetTrigger("leftHit");
                        break;

                    case 2:
                        animator.SetTrigger("rightHit");
                        break;
                }
            }

            healthBar.SetHealth(currentHealth - damage);
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    //Wait to take damage again
    private IEnumerator TakeDamageCoroutine()
    {
        tookDamage = true;
        yield return new WaitForSeconds(2f);
        tookDamage = false;
    }
}
