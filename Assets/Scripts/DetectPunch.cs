using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPunch : MonoBehaviour
{
    public GameObject playerModel;
    public GameObject hand;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Enemy")
        {
            playerModel.GetComponent<PlayerCombat>().target = target.gameObject;
            playerModel.GetComponent<PlayerCombat>().enemyInTrigger = true;
        }
    }

    void OnTriggerExit(Collider target)
    {
        if (target.tag == "Enemy")
        {
            playerModel.GetComponent<PlayerCombat>().enemyInTrigger = false;
            playerModel.GetComponent<PlayerCombat>().target = null;
            hand.GetComponent<Collider>().enabled = false;
        }
    }
}
