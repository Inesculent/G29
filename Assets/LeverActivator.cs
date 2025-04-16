using UnityEngine;

public class LeverActivator : MonoBehaviour
{
    public GameObject targetDoor; // Assign the door GameObject that has MainDoorOpener
    private MainDoorOpener doorOpener;

    public float interactionDistance = 3f;
    private Transform player;

    private bool isPlayerNear = false;
    private bool hasActivated = false;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;

        if (targetDoor != null)
        {
            doorOpener = targetDoor.GetComponent<MainDoorOpener>();
            if (doorOpener != null)
            {
                doorOpener.enabled = false; // Start disabled until lever is pulled
            }
            else
            {
                Debug.LogError("❌ MainDoorOpener not found on targetDoor!");
            }
        }
    }

    void Update()
    {
        if (hasActivated || player == null || doorOpener == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerNear = distance <= interactionDistance;

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        hasActivated = true;

        doorOpener.enabled = true;
        Debug.Log("🔓 MainDoorOpener script enabled by lever!");

        // Optional: disable lever visuals, animation, etc.
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
