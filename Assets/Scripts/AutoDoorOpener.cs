using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoorOpener : MonoBehaviour
{
    // The main door is automatically set from the parent object's Door component.
    public Door mainDoor;
    // The door base is the object this script is attached to.
    public Transform doorBase;

    // List of additional (linked) doors that will toggle together with the main door.
    public List<Door> linkedDoors = new List<Door>();

    // Reference to the player's transform (auto-assigned if not set).
    public Transform player;

    // Interaction parameters
    public float interactionDistance = 10f;
    public float facingThreshold = 0.5f;

    // Delay for linked doors only.
    public float delay = 0f;

    // Tracks the state of the main door (false = closed, true = open).
    private bool mainDoorOpened = false;

    private void Awake()
    {
        // Set the door base to be this object.
        doorBase = transform;

        // Automatically assign mainDoor from the parent.
        if (mainDoor == null)
        {
            mainDoor = GetComponentInParent<Door>();
            if (mainDoor == null)
            {
                Debug.LogError("Main door (Door component) not found in parent.");
            }
        }

        // Auto-assign the player if not set.
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("Player GameObject not found. Tag your player as 'Player' or assign it manually.");
        }
    }

    private void Update()
    {
        if (player == null || mainDoor == null)
            return;

        // Check if the player is within the interaction range from the door base.
        float distance = Vector3.Distance(player.position, doorBase.position);
        if (distance <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            // Check if the player is facing the door base.
            Vector3 directionToDoorBase = (doorBase.position - player.position).normalized;
            float dot = Vector3.Dot(player.forward, directionToDoorBase);

            if (dot >= facingThreshold)
            {
                // Toggle main door immediately.
                if (!mainDoorOpened)
                {
                    mainDoor.Open(player.position);
                    // Start coroutine to open linked doors after delay.
                    StartCoroutine(DelayedLinkedDoorsOpen());
                    mainDoorOpened = true;
                }
                else
                {
                    mainDoor.Close();
                    // Start coroutine to close linked doors after delay.
                    StartCoroutine(DelayedLinkedDoorsClose());
                    mainDoorOpened = false;
                }
            }
            else
            {
                Debug.Log("Player is not facing the door base. Dot: " + dot.ToString("F2"));
            }
        }
    }

    private IEnumerator DelayedLinkedDoorsOpen()
    {
        yield return new WaitForSeconds(delay);
        foreach (Door d in linkedDoors)
        {
            d.Open(player.position);
        }
    }

    private IEnumerator DelayedLinkedDoorsClose()
    {
        yield return new WaitForSeconds(delay);
        foreach (Door d in linkedDoors)
        {
            d.Close();
        }
    }
}
