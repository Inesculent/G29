using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Health healthComponent;
    private Rigidbody rb;
    private PlayerMovement playerMovement; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        healthComponent = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>(); // Reference to movement script
    }

    public void RespawnAt(Vector3 position)
    {
        Debug.Log("Respawning player...");

        // Disable movement temporarily
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Freeze Rigidbody before teleporting to avoid physics glitches
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Move the player to respawn location
        transform.position = position;

        // Restore health
        healthComponent.ResetHealth();

        // Reactivate physics and movement after a short delay
        Invoke(nameof(ReenableMovement), 0.1f);
    }

    private void ReenableMovement()
    {
        if (rb != null)
            rb.isKinematic = false; // Re-enable Rigidbody physics

        if (playerMovement != null)
            playerMovement.enabled = true; // Re-enable movement script

        Debug.Log("Movement re-enabled after respawn.");
    }
}