using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Animator animator;

    public GameObject target = null;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;

    public HealthBarScript healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    public int switchHand = 1;
    public bool enemyInTrigger = false;

    public bool isCombatMode = false;
    private bool tookDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //Disable colliders so that they dont trigger damage on the enemy
        //when not actively attacking
        leftHand.GetComponent<Collider>().enabled = false;
        rightHand.GetComponent<Collider>().enabled = false; 
    }

    private void FixedUpdate()
    {
        //If you hit the enemy decrease their health
        if (enemyInTrigger == true)
        {
            target.GetComponent<EnemyCombat>().TakeDamage(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //The C key switches Combat Mode on and off
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCombatMode = !isCombatMode;
        }

        //Changes the animation state for combat mode
        if (isCombatMode == true)
        {
            animator.SetBool("isCombatMode", true);
        }
        //Goes back to idle animation
        else
        {
            animator.SetBool("isCombatMode", false);
        }

        //Checks if E is pressed and Combat Mode is on to know to throwPunch
        if (Input.GetKeyDown(KeyCode.E) && isCombatMode == true)
        {
            throwPunch();
        }
    }

    public void throwPunch()
    {
        //Switches which hand the player is punching with
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
    }
    public void TakeDamage(int damage)
    {
        if (!tookDamage)
        {
            currentHealth -= damage;

            //Puts the player in combat mode if they werent all reasy
            if (!isCombatMode)
            {
                isCombatMode = true;
                animator.SetBool("isCombatMode", true);
            }

            //Need to add left animation for when the enemy punches with their left
            animator.SetTrigger("rightHit");

            healthBar.SetHealth(currentHealth - damage);
            StartCoroutine(TakeDamageCoroutine());

            //Havent set up the player death animation yet
            //if (currentHealth <= 0)
            //{
            //    animator.SetBool("isDead", true);
            //}
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
