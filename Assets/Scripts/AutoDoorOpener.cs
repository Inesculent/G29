using UnityEngine;

public class AutoDoorOpener : MonoBehaviour
{
    // Reference to the Door script (ensure this is assigned either manually or via auto-assignment)
    public Door door;
    // Reference to the player's transform
    public Transform player;
    // Distance threshold for the door to open (10 units)
    public float openDistance = 10f;
    
    // This flag prevents multiple triggers
    private bool doorOpened = false;

    private void Awake()
    {
        // Auto-assign player if not set in the Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player GameObject not found. Ensure the player is tagged as 'Player' or assign it manually.");
            }
        }

        // Auto-assign door if not set, assuming Door is on the same GameObject
        if (door == null)
        {
            door = GetComponent<Door>();
            if (door == null)
            {
                Debug.LogError("Door reference not assigned in AutoDoorOpener and no Door component found on this GameObject.");
            }
        }
    }

    private void Update()
    {
        if (!doorOpened && player != null && door != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= openDistance)
            {
                door.Open(player.position);
                doorOpened = true;
            }
        }
    }
}
