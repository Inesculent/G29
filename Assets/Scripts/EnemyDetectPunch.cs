using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectPunch : MonoBehaviour
{
    public GameObject enemyModel;
    public GameObject hand;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player") { 
            enemyModel.GetComponent<EnemyCombat>().playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider target)
    {
        enemyModel.GetComponent<EnemyCombat>().playerInTrigger = false;
        hand.GetComponent<Collider>().enabled = false;    
    }
}
